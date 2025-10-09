using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

internal static class UtilExtensions
{
  internal static JsonNode GetNode(this JsonObject obj, string key)
    => obj[key] ?? throw new MissingFieldException(key);

  internal static JsonObject WithValueIf(this JsonObject input, string key, JsonNode value, bool condition)
  {
    if (condition) input[key] = value;
    return input;
  }

  internal static JsonObject WithValueIfNotNull(this JsonObject input, string key, JsonNode? value)
  {
    if (value != null) input[key] = value;
    return input;
  }
}