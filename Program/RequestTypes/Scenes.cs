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

    public static OBSVoidRequest SetCurrentProgramScene(ID sceneID)
      => new OBSVoidRequest
      {
        RequestType = "SetCurrentProgramScene",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

    public static OBSRequest<CurrentScene> GetCurrentPreviewScene()
      => new OBSRequest<CurrentScene>
      {
        CastResult = d => new CurrentScene(d),
        RequestType = "GetCurrentPreviewScene"
      };

    public static OBSVoidRequest SetCurrentPreviewScene(ID sceneID)
      => new OBSVoidRequest
      {
        RequestType = "SetCurrentPreviewScene",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

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
