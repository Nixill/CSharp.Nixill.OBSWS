using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class General
  {
    public static OBSRequest<VersionInfo> GetVersion()
      => new OBSRequest<VersionInfo>
      {
        CastResult = (r, d) => new VersionInfo(r, d),
        RequestType = "GetVersion"
      };

    public static OBSRequest<StatsInfo> GetStats()
      => new OBSRequest<StatsInfo>
      {
        CastResult = (r, d) => new StatsInfo(r, d),
        RequestType = "GetStats"
      };

    public static OBSVoidRequest BroadcastCustomEvent(JsonObject eventData)
      => new OBSVoidRequest
      {
        RequestType = "BroadcastCustomEvent",
        RequestData = new JsonObject
        {
          ["eventData"] = eventData
        }
      };

    public static OBSRequest<VendorResponse> CallVendorRequest(string vendorName, string requestType, JsonObject? requestData = null)
      => new OBSRequest<VendorResponse>
      {
        CastResult = (r, d) => new VendorResponse(r, d),
        RequestType = "CallVendorRequest",
        RequestData = (JsonObject)new JsonObject
        {
          ["vendorName"] = vendorName,
          ["requestType"] = requestType
        }.WithValueIf("requestData", requestData, requestData != null)
      };

    public static OBSRequest<OBSListResult<string>> GetHotkeyList()
      => new OBSRequest<OBSListResult<string>>
      {
        CastResult = ResultCasts.StringList,
        RequestType = "GetHotkeyList"
      };

    public static OBSVoidRequest TriggerHotkeyByName(string hotkeyName, string? contextName = null)
      => new OBSVoidRequest
      {
        RequestType = "TriggerHotkeyByName",
        RequestData = (JsonObject)new JsonObject
        {
          ["hotkeyName"] = hotkeyName
        }.WithValueIfNotNull("contextName", contextName)
      };

    public static OBSVoidRequest TriggerHotkeyByKeySequence(string? keyID = null, KeyModifiers? keyModifiers = null)
      => new OBSVoidRequest
      {
        RequestType = "TriggerHotkeyByKeySequence",
        RequestData = (JsonObject)new JsonObject()
          .WithValueIfNotNull("keyId", keyID)
          .WithValueIfNotNull("keyModifiers", keyModifiers?.ToJson())
      };

    public static OBSVoidRequest Sleep(int millis = 0, int frames = 0)
      => new OBSVoidRequest
      {
        RequestType = "Sleep",
        RequestData = (JsonObject)new JsonObject()
          .WithValueIf("sleepMillis", millis, millis != 0)
          .WithValueIf("sleepFrames", frames, frames != 0 && millis == 0)
      };
  }
}

public class VersionInfo : OBSRequestResult
{
  public required string OBSVersion { get; init; }
  public required string OBSWebSocketVersion { get; init; }
  public required int RPCVersion { get; init; }
  public required string[] AvailableRequests { get; init; }
  public required string[] SupportedImageFormats { get; init; }
  public required string Platform { get; init; }
  public required string PlatformDescription { get; init; }

  public VersionInfo() { }

  [SetsRequiredMembers]
  public VersionInfo(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    OBSVersion = (string)GetNode("obsVersion")!;
    OBSWebSocketVersion = (string)GetNode("obsWebSocketVersion")!;
    RPCVersion = (int)GetNode("rpcVersion");
    AvailableRequests = ((JsonArray)GetNode("availableRequests")).Select(x => (string)x!).ToArray();
    SupportedImageFormats = ((JsonArray)GetNode("supportedImageFormats")).Select(x => (string)x!).ToArray();
    Platform = (string)GetNode("platform")!;
    PlatformDescription = (string)GetNode("platformDescription")!;
  }
}

public class StatsInfo : OBSRequestResult
{
  public required double CPUUsage { get; init; }
  public required double MemoryUsage { get; init; }
  public required double AvailableDiskSpace { get; init; }
  public required double ActiveFPS { get; init; }
  public required double AverageFrameRenderTime { get; init; }
  public required int RenderSkippedFrames { get; init; }
  public required int RenderTotalFrames { get; init; }
  public required int OutputSkippedFrames { get; init; }
  public required int OutputTotalFrames { get; init; }
  public required int WebSocketSessionIncomingMessages { get; init; }
  public required int WebSocketSessionOutgoingMessages { get; init; }

  public StatsInfo() { }

  [SetsRequiredMembers]
  public StatsInfo(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    CPUUsage = (double)GetNode("cpuUsage");
    MemoryUsage = (double)GetNode("memoryUsage");
    AvailableDiskSpace = (double)GetNode("availableDiskSpace");
    ActiveFPS = (double)GetNode("activeFps");
    AverageFrameRenderTime = (double)GetNode("averageFrameRenderTime");
    RenderSkippedFrames = (int)GetNode("renderSkippedFrames");
    RenderTotalFrames = (int)GetNode("renderTotalFrames");
    OutputSkippedFrames = (int)GetNode("outputSkippedFrames");
    OutputTotalFrames = (int)GetNode("outputTotalFrames");
    WebSocketSessionIncomingMessages = (int)GetNode("webSocketSessionIncomingMessages");
    WebSocketSessionOutgoingMessages = (int)GetNode("webSocketSessionOutgoingMessages");
  }
}

public class VendorResponse : OBSRequestResult
{
  public required string VendorName { get; init; }
  public required string RequestTypeFromVendor { get; init; }
  public required JsonObject ResponseDataFromVendor { get; init; }

  public VendorResponse() { }

  [SetsRequiredMembers]
  public VendorResponse(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    VendorName = (string)GetNode("vendorName")!;
    RequestTypeFromVendor = (string)GetNode("requestType")!;
    ResponseDataFromVendor = (JsonObject)GetNode("responseData");
  }
}

public struct KeyModifiers
{
  public KeyModifiers() { }

  public bool Shift { get; init; } = false;
  public bool Control { get; init; } = false;
  public bool Alt { get; init; } = false;
  public bool Command { get; init; } = false;

  public JsonObject ToJson()
    => new JsonObject
    {
      ["shift"] = Shift,
      ["control"] = Control,
      ["alt"] = Alt,
      ["command"] = Command
    };
}

[method: SetsRequiredMembers]
public readonly struct ID(string key, string value)
{
  public required string Key { get; init; } = key;
  public required string Value { get; init; } = value;

  public bool Matches(string type, JsonObject o)
    => o[type + Key] != null && (string)o[type + Key]! == Value;

  public static ID FromName(string name) => new ID("Name", name);
  public static ID FromUuid(string uuid) => new ID("Uuid", uuid);
  public static ID FromGuid(Guid guid) => new ID("Uuid", guid.ToString());

  public static implicit operator ID(string name) => new ID("Name", name);
  public static implicit operator ID(Guid guid) => ID.FromGuid(guid);

  public static implicit operator KeyValuePair<string, JsonNode?>(ID id)
    => new KeyValuePair<string, JsonNode?>(id.Key, id.Value);
}

internal static class IDExtensions
{
  internal static JsonObject AddID(this JsonObject input, ID id, string idType = "")
  {
    input.Add($"{idType}{id.Key}", id.Value);
    return input;
  }

  internal static KeyValuePair<string, JsonNode?> KVPOf(this ID id, string idType)
    => new KeyValuePair<string, JsonNode?>($"{idType}{id.Key}", id.Value);
}