using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class General
  {
    public static OBSRequest<OBSVersionInfo> GetVersion()
      => new OBSRequest<OBSVersionInfo>
      {
        CastResult = d => new OBSVersionInfo(d),
        RequestType = "GetVersion"
      };

    // GetStats
    // BroadcastCustomEvent
    // CallVendorRequest
    // GetHotkeyList
    // TriggerHotkeyByName
    // TriggerHotkeyByKeySequence
    // Sleep
  }
}

public class OBSVersionInfo : OBSRequestResult
{
  public required string OBSVersion { get; init; }
  public required string OBSWebSocketVersion { get; init; }
  public required int RPCVersion { get; init; }
  public required string[] AvailableRequests { get; init; }
  public required string[] SupportedImageFormats { get; init; }
  public required string Platform { get; init; }
  public required string PlatformDescription { get; init; }

  public OBSVersionInfo() { }

  [SetsRequiredMembers]
  public OBSVersionInfo(JsonObject obj) : base(obj)
  {
    OBSVersion = (string)GetRequiredNode("obsVersion")!;
    OBSWebSocketVersion = (string)GetRequiredNode("obsWebSocketVersion")!;
    RPCVersion = (int)GetRequiredNode("rpcVersion");
    AvailableRequests = ((JsonArray)GetRequiredNode("availableRequests")).Select(x => (string)x!).ToArray();
    SupportedImageFormats = ((JsonArray)GetRequiredNode("supportedImageFormats")).Select(x => (string)x!).ToArray();
    Platform = (string)GetRequiredNode("platform")!;
    PlatformDescription = (string)GetRequiredNode("platformDescription")!;
  }
}

public struct ID
{
  public required string Key { get; init; }
  public required string Value { get; init; }

  [SetsRequiredMembers]
  public ID(string key, string value)
  {
    Key = key;
    Value = value;
  }

  public static ID FromName(string name) => new ID("Name", name);
  public static ID FromUuid(string uuid) => new ID("Uuid", uuid);
  public static ID FromGuid(Guid guid) => new ID("Uuid", guid.ToString());

  public static implicit operator ID(string name) => new ID("Name", name);
  public static implicit operator ID(Guid guid) => ID.FromGuid(guid);

  public static implicit operator KeyValuePair<string, JsonNode?>(ID id)
    => new KeyValuePair<string, JsonNode?>(id.Key, id.Value);
}