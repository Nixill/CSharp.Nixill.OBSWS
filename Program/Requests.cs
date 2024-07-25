using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  static Dictionary<string, TaskCompletionSource<JsonObject>> WaitingResponses = new();

  public Task<OBSRequestResult> SendRequest(OBSRequest requestD)
  {
    string id = requestD.RequestID;
    TaskCompletionSource<JsonObject> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = requestD.ToJson()
    };

    WaitingResponses[id] = dataTask;
    Client.Send(request.ToString());

    if (dataTask.Task.Wait(30_000))
    {
      return Task.FromResult(requestD.ParseResult(dataTask.Task.Result));
    }
    else
    {
      throw new RequestTimedOutException(id);
    }
  }

  public async Task<T> SendRequest<T>(OBSRequest<T> requestD) where T : OBSRequestResult
    => (T)(await SendRequest((OBSRequest)requestD));


  public Task<OBSRequestResult> SendRequest(string requestType, JsonObject requestData)
    => SendRequest(new OBSRequest(requestType, requestData));

  void HandleResponse(JsonObject data)
  {
    string requestId = (string)data["requestId"]!;
    JsonObject requestStatus = (JsonObject)data["requestStatus"]!;
    bool isSuccessful = (bool)requestStatus["result"]!;

    if (WaitingResponses.ContainsKey(requestId))
    {
      var response = WaitingResponses[requestId];
      WaitingResponses.Remove(requestId);
      if (isSuccessful)
      {
        JsonObject responseData = (JsonObject)data["responseData"]!;
        response.SetResult(responseData);
      }
      else
      {
        int resultCode = (int)requestStatus["code"]!;
        string comment = (string)requestStatus["comment"]!;
        response.SetException(new RequestFailedException(resultCode, comment));
      }
    }
    else
    {
      Logger?.LogWarning($"Received response to request {requestId} of type {(data["requestType"]
        )}, which wasn't queued for a response.");
    }
  }

  Dictionary<string, TaskCompletionSource<JsonArray>> WaitingBatchResponses = new();

  public Task SendBatchRequest(JsonArray batchData, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime)
  {
    string id = Guid.NewGuid().ToString();
    TaskCompletionSource<JsonObject> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = new JsonObject
      {
        ["requestId"] = id,
        ["haltOnFailure"] = haltOnFailure,
        ["executionType"] = (int)executionType,
        ["requests"] = batchData
      }
    };

    WaitingResponses[id] = dataTask;
    Client.Send(request.ToString());

    if (dataTask.Task.Wait(30_000))
    {
      return Task.FromResult(dataTask.Task.Result);
    }
    else
    {
      throw new RequestTimedOutException(id);
    }
  }

  public void HandleBatchResponse(JsonObject data)
  {
    string requestId = (string)data["requestId"]!;
    JsonArray results = (JsonArray)data["results"]!;

    if (WaitingBatchResponses.ContainsKey(requestId))
    {
      var response = WaitingBatchResponses[requestId];
      WaitingBatchResponses.Remove(requestId);
      response.SetResult(results);
    }
    else
    {
      Logger?.LogWarning($"Received response to batch request {requestId}, which wasn't queued for a response.");
    }
  }
}

public class OBSRequest
{
  public required string RequestType { get; init; }
  public JsonObject? RequestData { get; init; } = null;
  public string RequestID { get; init; } = Guid.NewGuid().ToString();

  public OBSRequest() { }

  [SetsRequiredMembersAttribute]
  public OBSRequest(string requestType, JsonObject? requestData = null, string? requestID = null)
  {
    RequestType = requestType;
    RequestData = requestData;
    RequestID = requestID ?? RequestID;
  }

  public JsonObject ToJson()
  {
    JsonObject ret = new JsonObject
    {
      ["requestType"] = RequestType,
      ["requestId"] = RequestID,
    };

    if (RequestData != null)
    {
      ret["requestData"] = RequestData;
    }

    return ret;
  }

  public virtual OBSRequestResult ParseResult(JsonObject data)
    => new OBSRequestResult(data);
}

public class OBSRequest<T> : OBSRequest where T : OBSRequestResult
{
  public required Func<OBSRequestResult, T> CastResult { get; init; }

  public OBSRequest() { }

  [SetsRequiredMembersAttribute]
  public OBSRequest(Func<OBSRequestResult, T> caster, string requestType, JsonObject? requestData = null, string? requestID = null)
    : base(requestType, requestData, requestID)
  {
    CastResult = caster;
  }

  public override T ParseResult(JsonObject data)
  {
    OBSRequestResult result = base.ParseResult(data);
    return CastResult(result);
  }
}

public class OBSRequestResult
{
  public required string RequestType { get; init; }
  public required string RequestID { get; init; }
  public required bool Success { get; init; }
  public string? Comment { get; init; } = null;
  public required RequestStatus RequestStatusCode { get; init; }
  public JsonObject? ResponseData { get; init; } = null;

  public OBSRequestResult() { }

  [SetsRequiredMembers]
  public OBSRequestResult(JsonObject result)
  {
    RequestType = (string?)result["requestType"] ??
      throw new NullReferenceException("Missing field 'requestType' in json");
    RequestID = (string?)result["requestId"] ??
      throw new NullReferenceException("Missing field 'requestId' in json");
    JsonObject requestStatus = (JsonObject?)result["requestStatus"] ??
      throw new NullReferenceException("Missing object 'requestStatus' in json");
    Success = (bool?)requestStatus["result"] ??
      throw new NullReferenceException("Missing field 'requestStatus'.'result' in json");
    RequestStatusCode = (RequestStatus?)(int?)requestStatus["code"] ??
      throw new NullReferenceException("Missing field 'requestStatus'.'code' in json");
    Comment = (string?)requestStatus["comment"];
    ResponseData = (JsonObject?)result["responseData"];
  }
}