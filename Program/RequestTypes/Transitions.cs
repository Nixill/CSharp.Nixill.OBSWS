using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Transitions
  {
    public static OBSRequest<OBSListResult<string>> GetTransitionKindList()
      => new OBSRequest<OBSListResult<string>>
      {
        CastResult = ResultCasts.StringList,
        RequestType = "GetTransitionKindList"
      };

    // SceneTransitionList GetSceneTransitionList()
    // CurrentSceneTransition GetCurrentSceneTransition()
    // Void SetCurrentSceneTransition(string)
    // Void SetCurrentSceneTransitionDuration(double)
    // Void SetCurrentSceneTransitionSettings(obj, bool = true)
    // GetCurrentSceneTransitionCursor
    // TriggerStudioModeTransition
    // SetTBarPosition
  }
}