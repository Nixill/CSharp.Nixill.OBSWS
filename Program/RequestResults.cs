using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Nixill.OBSWS;


public class OBSRequestResult
{
  public JsonObject? ResponseData { get; init; } = null;

  public OBSRequestResult() { }

  public OBSRequestResult(JsonObject result)
  {
    ResponseData = result;
  }

  [return: NotNull]
  protected JsonNode GetRequiredNode(string node)
  {
    return ResponseData![node] ?? throw new MissingFieldException(node);
  }
}

public class OBSSingleValueResult<T> : OBSRequestResult
{
  public required T Result { get; init; }

  public OBSSingleValueResult() { }

  [SetsRequiredMembers]
  public OBSSingleValueResult(JsonObject obj, Func<JsonNode, T> cast) : base(obj)
  {
    Result = cast(obj.Single().Value!);
  }

  [SetsRequiredMembers]
  public OBSSingleValueResult(JsonObject obj, string key, Func<JsonNode, T> cast) : base(obj)
  {
    Result = cast(obj[key] ?? throw new MissingFieldException(key));
  }

  public static implicit operator T(OBSSingleValueResult<T> result) => result.Result;

  public static Func<JsonObject, OBSSingleValueResult<T>> CastFunc(Func<JsonNode, T> innerCastFunc)
    => obj => new OBSSingleValueResult<T>(obj, innerCastFunc);
}

public class OBSListResult<T> : OBSRequestResult, IEnumerable<T>
{
  public required IEnumerable<T> Results { get; init; }

  public OBSListResult() { }

  [SetsRequiredMembers]
  public OBSListResult(JsonObject obj, Func<JsonNode, T> cast) : base(obj)
  {
    Results = ((JsonArray)obj.Single().Value!).Select(n => cast(n!));
  }

  [SetsRequiredMembers]
  public OBSListResult(JsonObject obj, string key, Func<JsonNode, T> cast) : base(obj)
  {
    Results = ((JsonArray?)obj[key] ?? throw new MissingFieldException(key)).Select(n => cast(n!));
  }

  public IEnumerator<T> GetEnumerator() => Results.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();

  public static Func<JsonObject, OBSListResult<T>> CastFunc(Func<JsonNode, T> innerCastFunc)
    => obj => new OBSListResult<T>(obj, innerCastFunc);
}

public class OBSBatchRequestResult
{
  public required IEnumerable<OBSRequestResponse> Results { get; init; }
  public required bool FinishedWithoutErrors { get; init; }

  public OBSBatchRequestResult() { }

  [SetsRequiredMembers]
  public OBSBatchRequestResult(JsonArray array)
  {
    bool errors = false;
    Results = array.Select(o =>
    {
      JsonObject obj = (JsonObject)o!;
      var ret = new OBSRequestResponse(obj);
      if (!ret.RequestSuccessful) errors = true;
      return ret;
    });
    FinishedWithoutErrors = !errors;
  }
}

public class OBSRequestResponse
{
  public required string RequestType { get; init; }

  // This is technically not required in the OBS WebSocket spec, because
  // it mirrors what is provided in the original request and sub-requests
  // that are part of a batch may omit an ID. However, this library will
  // always assign an ID to a request, therefore it will always receive
  // one back.
  public required string RequestID { get; init; }

  public required bool RequestSuccessful { get; init; }
  public required RequestStatus RequestStatusCode { get; init; }
  public string? RequestComment { get; init; } = null;
  public JsonObject? ResponseData { get; init; } = null;

  public OBSRequestResponse() { }

  [SetsRequiredMembers]
  public OBSRequestResponse(JsonObject obj)
  {
    RequestType = (string?)obj["requestType"] ?? throw new MissingFieldException("requestType");
    RequestID = (string?)obj["requestId"] ?? throw new MissingFieldException("requestId");
    JsonObject requestStatus = (JsonObject?)obj["requestStatus"] ?? throw new MissingFieldException("requestStatus");
    RequestSuccessful = (bool?)requestStatus["result"] ?? throw new MissingFieldException("result");
    RequestStatusCode = (RequestStatus?)(int?)requestStatus["code"] ?? throw new MissingFieldException("code");
    RequestComment = (string?)requestStatus["comment"];
    ResponseData = (JsonObject?)obj["responseData"];
  }
}