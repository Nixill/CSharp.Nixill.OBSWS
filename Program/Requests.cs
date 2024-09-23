using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  static ConcurrentDictionary<string, OBSRequestCompletionSource> WaitingResponses = new();

  public async Task<OBSRequestResult> SendRequest(OBSRequest request, int timeout = 30)
  {
    await Task.Delay(0);
    string id = request.RequestID;
    TaskCompletionSource<OBSRequestResponse> dataTask = new();
    JsonObject jsonRequest = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = request.ToJson()
    };

    WaitingResponses[id] = new() { OriginalRequest = request, ResponseCompletionSource = dataTask };
    Client.Send(jsonRequest.ToString());

    if (timeout == -1)
    {
      dataTask.Task.Wait();
      timeout = 0;
    }

    if (dataTask.Task.Wait(TimeSpan.FromSeconds(timeout)))
    {
      OBSRequestResponse response = dataTask.Task.Result;
      if (response.RequestSuccessful) return response.RequestResult!;
      else throw new RequestFailedException(request, response.RequestStatusCode, response.RequestComment);
    }
    else
    {
      throw new RequestTimedOutException(request);
    }
  }

  public async Task<T> SendRequest<T>(OBSRequest<T> requestD, int timeout = 30) where T : OBSRequestResult
    => (T)await SendRequest((OBSRequest)requestD, timeout);

  public Task<OBSRequestResult> SendRequest(string requestType, JsonObject requestData, int timeout = 30)
    => SendRequest(new OBSRequest(requestType, requestData), timeout);

  public Task SendRequestWithoutWaiting(OBSRequest request)
  {
    string id = request.RequestID;
    JsonObject requestJson = new JsonObject
    {
      ["op"] = (int)OpCode.Request,
      ["d"] = request.ToJson()
    };

    // I'm still adding to the WaitingResponses list so that there isn't
    // a complaint of "received a response with an unqueued ID".
    WaitingResponses[id] = new OBSRequestCompletionSource() { OriginalRequest = request, ResponseCompletionSource = new() };
    Client.Send(requestJson.ToString());

    return Task.CompletedTask;
  }

  void HandleResponse(JsonObject data)
  {
    string requestId = (string)data["requestId"]!;
    // JsonObject requestStatus = (JsonObject)data["requestStatus"]!;
    // bool isSuccessful = (bool)requestStatus["result"]!;

    if (WaitingResponses.ContainsKey(requestId))
    {
      var responseTask = WaitingResponses[requestId];
      WaitingResponses.TryRemove(new(requestId, responseTask));

      OBSRequestResponse response = new OBSRequestResponse(responseTask.OriginalRequest, data);

      responseTask.ResponseCompletionSource.SetResult(response);
    }
    else
    {
      Logger?.LogWarning($"Received response to request {requestId} of type {data["requestType"]}, which wasn't queued for a response.");
    }
  }

  ConcurrentDictionary<string, OBSRequestBatchCompletionSource> WaitingBatchResponses = new();

  // TODO make improvements to this, including allowing OBSRequest[] and
  // updating timeout for Sleeps
  public Task<OBSRequestBatchResult> SendBatchRequest(OBSRequestBatch requestBatch, int timeout = 15)
  {
    int millisTimeout = requestBatch
      .Where(r => r.RequestType == "Sleep")
      .Select(r => (int?)r.RequestData?["sleepMillis"] ?? 0)
      .Sum();
    int framesTimeout = requestBatch
      .Where(r => r.RequestType == "Sleep")
      .Select(r => (int?)r.RequestData?["sleepFrames"] ?? 0)
      .Sum();

    TaskCompletionSource<OBSRequestBatchResult> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.RequestBatch,
      ["d"] = new JsonObject
      {
        ["requestId"] = requestBatch.ID,
        ["haltOnFailure"] = requestBatch.HaltOnFailure,
        ["executionType"] = (int)requestBatch.ExecutionType,
        ["requests"] = new JsonArray(requestBatch.Select(r => r.ToJson()).ToArray())
      }
    };

    WaitingBatchResponses[requestBatch.ID] = new OBSRequestBatchCompletionSource { OriginalRequest = requestBatch, ResponseCompletionSource = dataTask };
    Client.Send(request.ToString());

    if (dataTask.Task.Wait(TimeSpan.FromSeconds(timeout) + TimeSpan.FromMilliseconds(millisTimeout)
      + TimeSpan.FromSeconds(framesTimeout / 30)))
    {
      return Task.FromResult(dataTask.Task.Result);
    }
    else
    {
      throw new RequestBatchTimedOutException(requestBatch);
    }
  }

  public void SendBatchRequestWithoutWaiting(OBSRequestBatch batchData, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime)
  {
    string id = Guid.NewGuid().ToString();
    TaskCompletionSource<OBSRequestBatchResult> dataTask = new();
    JsonObject request = new JsonObject
    {
      ["op"] = (int)OpCode.RequestBatch,
      ["d"] = new JsonObject
      {
        ["requestId"] = id,
        ["haltOnFailure"] = haltOnFailure,
        ["executionType"] = (int)executionType,
        ["requests"] = new JsonArray(batchData.Select(r => r.ToJson()).ToArray())
      }
    };

    WaitingBatchResponses[id] = new OBSRequestBatchCompletionSource { OriginalRequest = batchData, ResponseCompletionSource = dataTask };
    Client.Send(request.ToString());
  }

  public void HandleBatchResponse(JsonObject data)
  {
    string requestId = (string)data["requestId"]!;
    JsonArray results = (JsonArray)data["results"]!;

    if (WaitingBatchResponses.ContainsKey(requestId))
    {
      var response = WaitingBatchResponses[requestId];
      WaitingBatchResponses.TryRemove(new(requestId, response));
      response.ResponseCompletionSource.SetResult(new OBSRequestBatchResult(response.OriginalRequest.Requests, results));
    }
    else
    {
      Logger?.LogWarning($"Received response to batch request {requestId}, which wasn't queued for a response.");
    }
  }
}

internal struct OBSRequestCompletionSource
{
  internal OBSRequest OriginalRequest;
  internal TaskCompletionSource<OBSRequestResponse> ResponseCompletionSource;
}

internal struct OBSRequestBatchCompletionSource
{
  internal OBSRequestBatch OriginalRequest;
  internal TaskCompletionSource<OBSRequestBatchResult> ResponseCompletionSource;
}

public class OBSRequest
{
  public required string RequestType { get; init; }
  public JsonObject? RequestData { get; init; } = null;
  public string RequestID { get; init; } = Guid.NewGuid().ToString();

  public OBSRequest() { }

  [SetsRequiredMembers]
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
    => new OBSRequestResult(this, data);
}

public class OBSRequest<T> : OBSRequest where T : OBSRequestResult
{
  public required Func<OBSRequest, JsonObject, T> CastResult { get; init; }

  public OBSRequest() { }

  [SetsRequiredMembersAttribute]
  public OBSRequest(Func<OBSRequest, JsonObject, T> caster, string requestType, JsonObject? requestData = null,
    string? requestID = null) : base(requestType, requestData, requestID)
  {
    CastResult = caster;
  }

  public override T ParseResult(JsonObject data)
  {
    return CastResult(this, data);
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

public class OBSRequestBatch : IEnumerable<OBSRequest>
{
  public required List<OBSRequest> Requests { get; init; }
  public string ID { get; init; } = Guid.NewGuid().ToString();
  public bool HaltOnFailure { get; init; } = false;
  public RequestBatchExecutionType ExecutionType { get; init; } = RequestBatchExecutionType.SerialRealtime;

  public OBSRequestBatch() { }

  [SetsRequiredMembers]
  public OBSRequestBatch(IEnumerable<OBSRequest> requests, string? uuid = null, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime)
  {
    Requests = requests.ToList();
    ID = uuid ?? ID;
    HaltOnFailure = haltOnFailure;
    ExecutionType = executionType;
  }

  [SetsRequiredMembers]
  public OBSRequestBatch(params OBSRequest[] requests)
  {
    Requests = requests.ToList();
  }

  public IEnumerator<OBSRequest> GetEnumerator() => Requests.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Requests.GetEnumerator();
}