using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  private static JsonObject GetData(IEnumerable<OBSRequestParameter> values)
  {
    JsonObject data = [];
    foreach (var value in values)
    {
      if (value.Condition) data[value.Key] = value.Value;
    }
    return data;
  }

  private static OBSVoidRequest NewRequest(string type) => new OBSVoidRequest { RequestType = type };

  private static OBSVoidRequest NewRequest(string type, params IEnumerable<OBSRequestParameter> values) => new OBSVoidRequest
  {
    RequestType = type,
    RequestData = GetData(values)
  };

  private static OBSRequest<T> NewRequest<T>(string type, Func<OBSRequest, JsonObject, T> cast)
    where T : OBSRequestResult
    => new OBSRequest<T>
    {
      CastResult = cast,
      RequestType = type
    };

  private static OBSRequest<T> NewRequest<T>(string type, Func<OBSRequest, JsonObject, T> cast,
    params IEnumerable<OBSRequestParameter> values) where T : OBSRequestResult
    => new OBSRequest<T>
    {
      CastResult = cast,
      RequestType = type,
      RequestData = GetData(values)
    };

  private static OBSRequest<OBSSingleValueResult<T>> NewSVRequest<T>(string type, Func<JsonNode, T> innerCast)
    => new OBSRequest<OBSSingleValueResult<T>>
    {
      CastResult = ResultCasts.SingleValue(innerCast),
      RequestType = type
    };

  private static OBSRequest<OBSSingleValueResult<T>> NewSVRequest<T>(string type, Func<JsonNode, T> innerCast,
    params IEnumerable<OBSRequestParameter> values)
    => new OBSRequest<OBSSingleValueResult<T>>
    {
      CastResult = ResultCasts.SingleValue(innerCast),
      RequestType = type,
      RequestData = GetData(values)
    };

  private static OBSRequest<OBSListResult<T>> NewListRequest<T>(string type, Func<JsonNode, T> innerCast)
    => new OBSRequest<OBSListResult<T>>
    {
      CastResult = ResultCasts.List(innerCast),
      RequestType = type
    };

  private static OBSRequest<OBSListResult<T>> NewListRequest<T>(string type, Func<JsonNode, T> innerCast,
    params IEnumerable<OBSRequestParameter> values)
    => new OBSRequest<OBSListResult<T>>
    {
      CastResult = ResultCasts.List(innerCast),
      RequestType = type,
      RequestData = GetData(values)
    };
}

internal record OBSRequestParameter(string Key, JsonNode Value, bool Condition = true)
{
  public OBSRequestParameter(string key, string value, bool condition = true) : this(key, (JsonNode)value, condition) { }
  public OBSRequestParameter(string key, ID value, bool condition = true) : this(key + value.Key, value.Value, condition) { }

  public static implicit operator OBSRequestParameter((string Key, JsonNode Value) tuple) => new(tuple.Key, tuple.Value);
  public static implicit operator OBSRequestParameter((string Key, JsonNode Value, bool Condition) tuple) => new(tuple.Key, tuple.Value, tuple.Condition);

  public static implicit operator OBSRequestParameter((string Key, string Value) tuple) => new(tuple.Key, tuple.Value);
  public static implicit operator OBSRequestParameter((string Key, string Value, bool Condition) tuple) => new(tuple.Key, tuple.Value, tuple.Condition);

  public static implicit operator OBSRequestParameter((string Key, ID Value) tuple) => new(tuple.Key, tuple.Value);
  public static implicit operator OBSRequestParameter((string Key, ID Value, bool Condition) tuple) => new(tuple.Key, tuple.Value, tuple.Condition);
}