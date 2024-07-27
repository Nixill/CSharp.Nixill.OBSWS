using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  static ConcurrentDictionary<string, TaskCompletionSource<JsonObject>> WaitingResponses = new();

  public Task<OBSRequestResult> SendRequest(OBSRequest requestD, int timeout = 30)
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

    if (timeout == -1)
    {
      dataTask.Task.Wait();
      timeout = 0;
    }

    if (dataTask.Task.Wait(TimeSpan.FromSeconds(timeout)))
    {
      return Task.FromResult(requestD.ParseResult(dataTask.Task.Result));
    }
    else
    {
      throw new RequestTimedOutException(id);
    }
  }

  public async Task<T> SendRequest<T>(OBSRequest<T> requestD, int timeout = 30) where T : OBSRequestResult
    => (T)(await SendRequest((OBSRequest)requestD, timeout));

  public Task<OBSRequestResult> SendRequest(string requestType, JsonObject requestData, int timeout = 30)
    => SendRequest(new OBSRequest(requestType, requestData), timeout);

  public Task SendRequestWithoutWaiting(OBSRequest requestD)
  {
    string id = requestD.RequestID;
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = requestD.ToJson()
    };

    // I'm still adding to the WaitingResponses list so that there isn't
    // a complaint of "received a response with an unqueued ID".
    WaitingResponses[id] = new();
    Client.Send(request.ToString());

    return Task.CompletedTask;
  }

  void HandleResponse(JsonObject data)
  {
    string requestId = (string)data["requestId"]!;
    JsonObject requestStatus = (JsonObject)data["requestStatus"]!;
    bool isSuccessful = (bool)requestStatus["result"]!;

    if (WaitingResponses.ContainsKey(requestId))
    {
      var response = WaitingResponses[requestId];
      WaitingResponses.TryRemove(new(requestId, response));
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

  // TODO make improvements to this, including allowing OBSRequest[] and
  // updating timeout for Sleeps
  public Task<OBSBatchRequestResult> SendBatchRequest(IEnumerable<OBSRequest> batchData, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime, int timeout = 15)
  {
    int millisTimeout = batchData
      .Where(r => r.RequestType == "Sleep")
      .Select(r => (int?)r.RequestData?["sleepMillis"] ?? 0)
      .Sum();
    int framesTimeout = batchData
      .Where(r => r.RequestType == "Sleep")
      .Select(r => (int?)r.RequestData?["sleepFrames"] ?? 0)
      .Sum();

    string id = Guid.NewGuid().ToString();
    TaskCompletionSource<JsonArray> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = new JsonObject
      {
        ["requestId"] = id,
        ["haltOnFailure"] = haltOnFailure,
        ["executionType"] = (int)executionType,
        ["requests"] = new JsonArray(batchData.Select(r => r.ToJson()).ToArray())
      }
    };

    WaitingBatchResponses[id] = dataTask;
    Client.Send(request.ToString());

    if (dataTask.Task.Wait(TimeSpan.FromSeconds(timeout) + TimeSpan.FromMilliseconds(millisTimeout)
      + TimeSpan.FromSeconds(framesTimeout / 30)))
    {
      return Task.FromResult(new OBSBatchRequestResult(dataTask.Task.Result));
    }
    else
    {
      throw new RequestTimedOutException(id);
    }
  }

  public void SendBatchRequestWithoutWaiting(IEnumerable<OBSRequest> batchData, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime)
  {
    string id = Guid.NewGuid().ToString();
    TaskCompletionSource<JsonArray> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = new JsonObject
      {
        ["requestId"] = id,
        ["haltOnFailure"] = haltOnFailure,
        ["executionType"] = (int)executionType,
        ["requests"] = new JsonArray(batchData.Select(r => r.ToJson()).ToArray())
      }
    };

    WaitingBatchResponses[id] = dataTask;
    Client.Send(request.ToString());
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
    => JsonSerializer.Deserialize<OBSRequestResult>(data, JsonSerializerOptionProvider.Options)!;
}

public class OBSRequest<T> : OBSRequest where T : OBSRequestResult
{
  public Func<JsonObject, T> CastResult { get; init; } = j => JsonSerializer.Deserialize<T>(j, JsonSerializerOptionProvider.Options)!;

  public OBSRequest() { }

  [SetsRequiredMembersAttribute]
  public OBSRequest(string requestType, Func<JsonObject, T>? caster = null, JsonObject? requestData = null, string? requestID = null)
    : base(requestType, requestData, requestID)
  {
    CastResult = caster ?? CastResult;
  }

  public override T ParseResult(JsonObject data)
  {
    return CastResult(data);
  }
}

// This is simply a "marker" class that says that indicates there will be
// no return data.
public class OBSVoidRequest : OBSRequest
{
  public OBSVoidRequest() : base() { }
  [SetsRequiredMembers]
  public OBSVoidRequest(string requestType, JsonObject? requestData = null, string? requestID = null)
    : base(requestType, requestData, requestID)
  { }
}
