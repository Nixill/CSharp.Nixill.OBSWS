using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

internal static class Utils
{
  internal static JsonNode GetNode(this JsonObject obj, string key)
    => obj[key] ?? throw new MissingFieldException(key);

  internal static IDictionary<K, V> WithValueIf<K, V>(this IDictionary<K, V> input, K key, V value, bool condition)
  {
    if (condition) input[key] = value;
    return input;
  }
}