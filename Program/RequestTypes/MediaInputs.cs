using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class MediaInputs
  {
    public static OBSRequest<MediaInputState> GetMediaInputStatus(ID inputID)
      => new OBSRequest<MediaInputState>
      {
        CastResult = (r, o) => new MediaInputState(r, o),
        RequestType = "GetMediaInputStatus",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value
        }
      };

    // public static OBSRequest<MediaInputState> GetMediaInputStatus(ID inputID)
    //   => NewRequest("GetMediaInputStatus", (r, o) => new MediaInputState(r, o), ("input", inputID));

    public static OBSVoidRequest SetMediaInputCursor(ID inputID, int mediaCursor)
      => NewRequest("SetMediaInputCursor", ("input", inputID), ("mediaCursor", mediaCursor));

    public static OBSVoidRequest OffsetMediaInputCursor(ID inputID, int mediaCursorOffset)
      => NewRequest("OffsetMediaInputCursor", ("input", inputID), ("mediaCursorOffset", mediaCursorOffset));

    public static OBSVoidRequest TriggerMediaInputAction(ID inputID, OBSMediaInputAction action)
      => NewRequest("TriggerMediaInputAction", ("input", inputID), ("mediaAction", action.GetIdentifierValue()));
  }
}

public class MediaInputState : OBSRequestResult
{
  public required OBSMediaState State { get; init; }
  public required int? Cursor { get; init; }
  public required int? Duration { get; init; }

  public MediaInputState() { }

  [SetsRequiredMembers]
  public MediaInputState(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    State = OBSMediaStates.ForIdentifierValue((string)obj.GetNode("mediaState")!);
    Cursor = (int?)obj["mediaCursor"];
    Duration = (int?)obj["mediaDuration"];
  }
}