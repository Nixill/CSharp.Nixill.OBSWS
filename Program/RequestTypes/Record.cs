using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Record
  {
    public static OBSRequest<RecordStatus> GetRecordStatus()
      => new OBSRequest<RecordStatus>
      {
        CastResult = (req, obj) => new RecordStatus(req, obj),
        RequestType = "GetRecordStatus"
      };
    // ToggleRecord
    // StartRecord
    // StopRecord
    // ToggleRecordPause
    // PauseRecord
    // ResumeRecord
    // SplitRecordFile
    // CreateRecordChapter
  }
}

public class RecordStatus : OBSRequestResult
{
  public required bool Active { get; init; }
  public required bool Paused { get; init; }
  public required string Timecode { get; init; }
  public required double Duration { get; init; }
  public required int Bytes { get; init; }

  public RecordStatus() { }

  [SetsRequiredMembers]
  public RecordStatus(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Active = (bool)obj.GetNode("outputActive");
    Paused = (bool)obj.GetNode("outputPaused");
    Timecode = (string)obj.GetNode("outputTimecode")!;
    Duration = (double)obj.GetNode("outputDuration");
    Bytes = (int)obj.GetNode("outputBytes");
  }
}