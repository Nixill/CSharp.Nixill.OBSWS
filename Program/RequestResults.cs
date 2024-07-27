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
  [JsonExtensionData]
  public Dictionary<string, object> ExtraOptions { get; init; } = new();

  public OBSRequestResult() { }
}

public class OBSSingleValueResult<T> : OBSRequestResult
{
  public required T Result { get; init; }

  public OBSSingleValueResult() { }

  [SetsRequiredMembers]
  public OBSSingleValueResult(JsonObject obj, Func<JsonNode, T> cast)
  {
    Result = cast(obj.Single().Value!);
  }

  [SetsRequiredMembers]
  public OBSSingleValueResult(JsonObject obj, string key, Func<JsonNode, T> cast)
  {
    Result = cast(obj[key] ?? throw new MissingFieldException(obj.Single().Key));
    foreach (var kvp in obj.Where(kvp => kvp.Key != key))
    {
      ExtraOptions.Add(kvp.Key, kvp.Value!);
    }
  }

  public static implicit operator T(OBSSingleValueResult<T> result) => result.Result;

  public static Func<JsonObject, OBSSingleValueResult<T>> RequestCastFunction(Func<JsonNode, T> innerCastFunction)
    => obj => new OBSSingleValueResult<T>(obj, innerCastFunction);

  public static Func<JsonObject, OBSSingleValueResult<T>> DeserializeFunction()
    => obj => new OBSSingleValueResult<T>(obj, node => JsonSerializer.Deserialize<T>(node, JsonSerializerOptionProvider.Options)!);
}

public class OBSListResult<T> : OBSRequestResult, IEnumerable<T>
{
  public required IEnumerable<T> Results { get; init; }

  public OBSListResult() { }

  [SetsRequiredMembers]
  public OBSListResult(JsonObject obj, Func<JsonNode, T> cast)
  {
    Results = ((JsonArray)obj.Single().Value!).Select(cast!);
  }

  [SetsRequiredMembers]
  public OBSListResult(JsonObject obj, string key, Func<JsonNode, T> cast)
  {
    Results = ((JsonArray?)obj[key] ?? throw new MissingFieldException(obj.Single().Key)).Select(cast!);
    foreach (var kvp in obj.Where(kvp => kvp.Key != key))
    {
      ExtraOptions.Add(kvp.Key, kvp.Value!);
    }
  }

  public IEnumerator<T> GetEnumerator() => Results.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Results.GetEnumerator();

  public static Func<JsonObject, OBSListResult<T>> RequestCastFunction(Func<JsonNode, T> innerCastFunction)
    => obj => new OBSListResult<T>(obj, innerCastFunction);

  public static Func<JsonObject, OBSListResult<T>> DeserializeFunction()
    => obj => new OBSListResult<T>(obj, node => JsonSerializer.Deserialize<T>(node, JsonSerializerOptionProvider.Options)!);
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