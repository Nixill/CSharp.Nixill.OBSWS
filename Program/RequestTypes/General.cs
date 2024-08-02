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
        CastResult = (r, d) => new OBSVersionInfo(r, d),
        RequestType = "GetVersion"
      };

    public static OBSRequest<OBSStatsInfo> GetStats()
      => new OBSRequest<OBSStatsInfo>
      {
        CastResult = (r, d) => new OBSStatsInfo(r, d),
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

    public static OBSRequest<OBSVendorResponse> CallVendorRequest(string vendorName, string requestType, JsonObject? requestData = null)
      => new OBSRequest<OBSVendorResponse>
      {
        CastResult = (r, d) => new OBSVendorResponse(r, d),
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
        CastResult = OBSListResult<string>.CastFunc(n => (string)n!),
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
  public OBSVersionInfo(OBSRequest req, JsonObject obj) : base(req, obj)
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

public class OBSStatsInfo : OBSRequestResult
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

  public OBSStatsInfo() { }

  [SetsRequiredMembers]
  public OBSStatsInfo(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    CPUUsage = (double)GetRequiredNode("cpuUsage");
    MemoryUsage = (double)GetRequiredNode("memoryUsage");
    AvailableDiskSpace = (double)GetRequiredNode("availableDiskSpace");
    ActiveFPS = (double)GetRequiredNode("activeFps");
    AverageFrameRenderTime = (double)GetRequiredNode("averageFrameRenderTime");
    RenderSkippedFrames = (int)GetRequiredNode("renderSkippedFrames");
    RenderTotalFrames = (int)GetRequiredNode("renderTotalFrames");
    OutputSkippedFrames = (int)GetRequiredNode("outputSkippedFrames");
    OutputTotalFrames = (int)GetRequiredNode("outputTotalFrames");
    WebSocketSessionIncomingMessages = (int)GetRequiredNode("webSocketSessionIncomingMessages");
    WebSocketSessionOutgoingMessages = (int)GetRequiredNode("webSocketSessionOutgoingMessages");
  }
}

public class OBSVendorResponse : OBSRequestResult
{
  public required string VendorName { get; init; }
  public required string RequestTypeFromVendor { get; init; }
  public required JsonObject ResponseDataFromVendor { get; init; }

  public OBSVendorResponse() { }

  [SetsRequiredMembers]
  public OBSVendorResponse(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    VendorName = (string)GetRequiredNode("vendorName")!;
    RequestTypeFromVendor = (string)GetRequiredNode("requestType")!;
    ResponseDataFromVendor = (JsonObject)GetRequiredNode("responseData");
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