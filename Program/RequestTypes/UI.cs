using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public class UI
  {
    public static OBSRequest<OBSSingleValueResult<bool>> GetStudioModeEnabled()
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = ResultCasts.Bool,
        RequestType = "GetStudioModeEnabled"
      };

    public static OBSVoidRequest SetStudioModeEnabled(bool enabled)
      => new OBSVoidRequest
      {
        RequestType = "SetStudioModeEnabled",
        RequestData = new JsonObject
        {
          ["studioModeEnabled"] = enabled
        }
      };

    public static OBSVoidRequest OpenInputPropertiesDialog(ID inputID)
      => new OBSVoidRequest
      {
        RequestType = "OpenInputPropertiesDialog",
        RequestData = new JsonObject().AddID(inputID, "input")
      };

    public static OBSVoidRequest OpenInputFiltersDialog(ID inputID)
      => new OBSVoidRequest
      {
        RequestType = "OpenInputFiltersDialog",
        RequestData = new JsonObject().AddID(inputID, "input")
      };

    public static OBSVoidRequest OpenInputInteractDialog(ID inputID)
      => new OBSVoidRequest
      {
        RequestType = "OpenInputInteractDialog",
        RequestData = new JsonObject().AddID(inputID, "input")
      };

    public static OBSRequest<OBSListResult<OBSMonitor>> GetMonitorList()
      => new OBSRequest<OBSListResult<OBSMonitor>>
      {
        CastResult = ResultCasts.List(n => new OBSMonitor(n)),
        RequestType = "GetMonitorList"
      };

    [Obsolete("Likely to be removed from a future websocket release.")]
    public static OBSVoidRequest OpenVideoMixProjector(VideoMixType type, int monitorIndex)
      => new OBSVoidRequest
      {
        RequestType = "OpenVideoMixProjector",
        RequestData = new JsonObject
        {
          ["videoMixType"] = VideoMixTypes.GetIdentifierValue(type),
          ["monitorIndex"] = monitorIndex
        }
      };

    [Obsolete("Likely to be removed from a future websocket release.")]
    public static OBSVoidRequest OpenVideoMixProjector(VideoMixType type, string projectorGeometry)
      => new OBSVoidRequest
      {
        RequestType = "OpenVideoMixProjector",
        RequestData = new JsonObject
        {
          ["videoMixType"] = VideoMixTypes.GetIdentifierValue(type),
          ["projectorGeometry"] = projectorGeometry
        }
      };

    [Obsolete("Likely to be removed from a future websocket release.")]
    public static OBSVoidRequest OpenSourceProjector(ID sourceID, int monitorIndex)
      => new OBSVoidRequest
      {
        RequestType = "OpenSourceProjector",
        RequestData = new JsonObject
        {
          ["monitorIndex"] = monitorIndex
        }.AddID(sourceID, "source")
      };

    [Obsolete("Likely to be removed from a future websocket release.")]
    public static OBSVoidRequest OpenSourceProjector(ID sourceID, string projectorGeometry)
      => new OBSVoidRequest
      {
        RequestType = "OpenSourceProjector",
        RequestData = new JsonObject
        {
          ["projectorGeometry"] = projectorGeometry
        }.AddID(sourceID, "source")
      };
  }
}

public class OBSMonitor
{
  public required JsonObject RawData { get; init; }

  public required int Index { get; init; }
  public required string Name { get; init; }
  public required int Width { get; init; }
  public required int Height { get; init; }
  public required int PositionX { get; init; }
  public required int PositionY { get; init; }

  public OBSMonitor() { }

  [SetsRequiredMembers]
  public OBSMonitor(JsonNode n)
  {
    JsonObject o = (JsonObject)n;

    RawData = o;

    Index = (int)o.GetNode("monitorIndex");
    Name = (string)o.GetNode("monitorName")!;
    Width = (int)o.GetNode("monitorWidth");
    Height = (int)o.GetNode("monitorHeight");
    PositionX = (int)o.GetNode("monitorPositionX");
    PositionY = (int)o.GetNode("monitorPositionY");
  }
}