using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static class Utils
{
  public static JsonNode GetNode(this JsonObject obj, string key)
    => obj[key] ?? throw new MissingFieldException(key);
}