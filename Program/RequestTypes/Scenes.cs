using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Scenes
  {
    // GetSceneList
    // GetGroupList

    public static OBSRequest<CurrentScene> GetCurrentProgramScene()
      => new OBSRequest<CurrentScene>
      {
        CastResult = d => new CurrentScene(d),
        RequestType = "GetCurrentProgramScene"
      };

    // SetCurrentProgramScene

    public static OBSRequest<CurrentScene> GetCurrentPreviewScene()
      => new OBSRequest<CurrentScene>
      {
        CastResult = d => new CurrentScene(d),
        RequestType = "GetCurrentPreviewScene"
      };

    // SetCurrentPreviewScene
    // CreateScene
    // RemoveScene
    // SetSceneName
    // GetSceneSceneTransitionOverride
    // SetSceneSceneTransitionOverride
  }
}

// This works for both program and preview scenes. Note that neither will
// have their call-specific return fields in a future RPC version.
public class CurrentScene : OBSRequestResult
{
  public required string Name { get; init; }
  public required string Uuid { get; init; }
  public Guid Guid => Guid.Parse(Uuid);

  public CurrentScene() { }

  [SetsRequiredMembers]
  public CurrentScene(JsonObject obj) : base(obj)
  {
    Name = (string)GetRequiredNode("sceneName")!;
    Uuid = (string)GetRequiredNode("sceneUuid")!;
  }
}
