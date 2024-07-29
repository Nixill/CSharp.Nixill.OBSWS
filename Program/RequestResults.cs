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
  public required OBSRequest OriginalRequest { get; init; }
  public JsonObject? ResponseData { get; init; } = null;

  public OBSRequestResult() { }

  [SetsRequiredMembers]
  public OBSRequestResult(OBSRequest request, JsonObject? result)
  {
    OriginalRequest = request;
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
  public OBSSingleValueResult(OBSRequest request, JsonObject obj, Func<JsonNode, T> cast) : base(request, obj)
  {
    Result = cast(obj.Single().Value!);
  }

  [SetsRequiredMembers]
  public OBSSingleValueResult(OBSRequest request, JsonObject obj, string key, Func<JsonNode, T> cast) : base(request, obj)
  {
    Result = cast(obj[key] ?? throw new MissingFieldException(key));
  }

  public static implicit operator T(OBSSingleValueResult<T> result) => result.Result;

  public static Func<OBSRequest, JsonObject, OBSSingleValueResult<T>> CastFunc(Func<JsonNode, T> innerCastFunc)
    => (req, obj) => new OBSSingleValueResult<T>(req, obj, innerCastFunc);
}

public class OBSListResult<T> : OBSRequestResult, IEnumerable<T>
{
  public required IEnumerable<T> Results { get; init; }

  public OBSListResult() { }

  [SetsRequiredMembers]
  public OBSListResult(OBSRequest req, JsonObject obj, Func<JsonNode, T> cast) : base(req, obj)
  {
    Results = ((JsonArray)obj.Single().Value!).Select(n => cast(n!));
  }

  [SetsRequiredMembers]
  public OBSListResult(OBSRequest req, JsonObject obj, string key, Func<JsonNode, T> cast) : base(req, obj)
  {
    Results = ((JsonArray?)obj[key] ?? throw new MissingFieldException(key)).Select(n => cast(n!));
  }

  public IEnumerator<T> GetEnumerator() => Results.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();

  public static Func<OBSRequest, JsonObject, OBSListResult<T>> CastFunc(Func<JsonNode, T> innerCastFunc)
    => (req, obj) => new OBSListResult<T>(req, obj, innerCastFunc);
}

public class OBSRequestBatchResult : IEnumerable<OBSRequestResponse>
{
  public required List<OBSRequest> OriginalRequests { get; init; }
  public required IEnumerable<OBSRequestResponse> Results { get; init; }
  public required bool FinishedWithoutErrors { get; init; }

  public OBSRequestBatchResult() { }

  [SetsRequiredMembers]
  public OBSRequestBatchResult(List<OBSRequest> requests, JsonArray array)
  {
    OriginalRequests = requests;
    bool errors = false;
    Results = requests
      .Join(array,
        r => r.RequestID,
        a => (string)a!["requestId"]!,
        (r, a) =>
        {
          JsonObject obj = (JsonObject)a!;
          var ret = new OBSRequestResponse(r, obj);
          if (!ret.RequestSuccessful) errors = true;
          return ret;
        }
      );
    FinishedWithoutErrors = !errors;
  }

  public IEnumerator<OBSRequestResponse> GetEnumerator() => Results.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();
}

public class OBSRequestResponse
{
  public required OBSRequest OriginalRequest { get; init; }
  public required OBSRequestResult? RequestResult { get; init; }
  public required bool RequestSuccessful { get; init; }
  public required RequestStatus RequestStatusCode { get; init; }
  public required string? RequestComment { get; init; } = null;
  public required JsonObject? ResponseData { get; init; } = null;

  public OBSRequestResponse() { }

  [SetsRequiredMembers]
  public OBSRequestResponse(OBSRequest request, JsonObject obj)
  {
    OriginalRequest = request;

    JsonObject requestStatus = (JsonObject?)obj["requestStatus"] ?? throw new MissingFieldException("requestStatus");
    RequestSuccessful = (bool?)requestStatus["result"] ?? throw new MissingFieldException("result");
    RequestStatusCode = (RequestStatus?)(int?)requestStatus["code"] ?? throw new MissingFieldException("code");
    RequestComment = (string?)requestStatus["comment"];
    ResponseData = (JsonObject?)obj["responseData"];

    if (ResponseData != null)
      RequestResult = OriginalRequest.ParseResult(ResponseData);
    else
      RequestResult = null;
  }
}