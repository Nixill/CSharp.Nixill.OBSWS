using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Stream
  {
    public static OBSRequest<StreamStatus> GetStreamStatus()
      => new OBSRequest<StreamStatus>
      {
        CastResult = (r, o) => new StreamStatus(r, o),
        RequestType = "GetStreamStatus"
      };

    public static OBSRequest<OBSSingleValueResult<bool>> ToggleStream()
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = OBSSingleValueResult<bool>.CastFunc(n => (bool)n),
        RequestType = "ToggleStream"
      };

    public static OBSVoidRequest StartStream()
      => new OBSVoidRequest
      {
        RequestType = "StartStream"
      };

    public static OBSVoidRequest StopStream()
      => new OBSVoidRequest
      {
        RequestType = "StopStream"
      };

    public static OBSVoidRequest SendStreamCaption(string text)
      => new OBSVoidRequest
      {
        RequestType = "SendStreamCaption",
        RequestData = new JsonObject
        {
          ["captionText"] = text
        }
      };
  }
}

public class StreamStatus : OBSRequestResult
{
  public required bool Active { get; init; }
  public required bool Reconnecting { get; init; }
  public required string Timecode { get; init; }
  public required double Duration { get; init; }
  public required double Congestion { get; init; }
  public required int Bytes { get; init; }
  public required int SkippedFrames { get; init; }
  public required int TotalFrames { get; init; }

  public StreamStatus() { }

  [SetsRequiredMembers]
  public StreamStatus(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Active = (bool)obj.GetNode("outputActive");
    Reconnecting = (bool)obj.GetNode("outputReconnecting");
    Timecode = (string)obj.GetNode("outputTimecode")!;
    Duration = (double)obj.GetNode("outputDuration");
    Congestion = (double)obj.GetNode("outputCongestion");
    Bytes = (int)obj.GetNode("outputBytes");
    SkippedFrames = (int)obj.GetNode("outputSkippedFrames");
    TotalFrames = (int)obj.GetNode("outputTotalFrames");
  }
}