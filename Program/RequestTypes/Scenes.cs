using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Scenes
  {
    public static OBSRequest<OBSSceneList> GetSceneList()
      => new OBSRequest<OBSSceneList>
      {
        CastResult = (r, d) => new OBSSceneList(r, d),
        RequestType = "GetSceneList"
      };

    // "GetGroupList" will remain unsupported until I see an actual
    // example of it working.

    public static OBSRequest<CurrentScene> GetCurrentProgramScene()
      => new OBSRequest<CurrentScene>
      {
        CastResult = (r, d) => new CurrentScene(r, d),
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
        CastResult = (r, d) => new CurrentScene(r, d),
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

    public static OBSRequest<OBSSingleValueResult<string>> CreateScene(string name)
      => new OBSRequest<OBSSingleValueResult<string>>
      {
        CastResult = OBSSingleValueResult<string>.CastFunc(n => (string)n!),
        RequestType = "CreateScene",
        RequestData = new JsonObject
        {
          ["sceneName"] = name
        }
      };

    public static OBSVoidRequest RemoveScene(ID sceneID)
      => new OBSVoidRequest
      {
        RequestType = "RemoveScene",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

    public static OBSVoidRequest SetSceneName(ID sceneID, string newName)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneName",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["newSceneName"] = newName
        }
      };

    public static OBSRequest<OBSSceneTransitionOverride> GetSceneTransitionOverride(ID sceneID)
      => new OBSRequest<OBSSceneTransitionOverride>
      {
        CastResult = (r, d) => new OBSSceneTransitionOverride(r, d),
        RequestType = "GetSceneTransitionOverride",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

    public static OBSVoidRequest SetSceneTransitionOverride(ID sceneID, string? transitionName, int? transitionDuration)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneTransitionOverride",
        RequestData = (JsonObject)new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }.WithValueIfNotNull("transitionName", transitionName)
        .WithValueIfNotNull("transitionDuration", transitionDuration)
      };
  }
}

public class OBSSceneList : OBSRequestResult, IEnumerable<SceneInfo>
{
  public required string? CurrentProgramSceneName { get; init; }
  public required string? CurrentProgramSceneUuid { get; init; }
  public Guid? CurrentProgramSceneGuid => CurrentProgramSceneUuid != null ? Guid.Parse(CurrentProgramSceneUuid) : null;
  public required string? CurrentPreviewSceneName { get; init; }
  public required string? CurrentPreviewSceneUuid { get; init; }
  public Guid? CurrentPreviewSceneGuid => CurrentPreviewSceneUuid != null ? Guid.Parse(CurrentPreviewSceneUuid) : null;
  public required SceneInfo[] Scenes { get; init; }

  public OBSSceneList() { }

  [SetsRequiredMembers]
  public OBSSceneList(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    CurrentProgramSceneName = (string?)GetNode("currentProgramSceneName");
    CurrentProgramSceneUuid = (string?)GetNode("currentProgramSceneUuid");
    CurrentPreviewSceneName = (string?)GetNode("currentPreviewSceneName");
    CurrentPreviewSceneUuid = (string?)GetNode("currentPreviewSceneUuid");
    Scenes = [.. ((JsonArray)GetNode("scenes")).Select(n => new SceneInfo(n!))];
  }

  public IEnumerator<SceneInfo> GetEnumerator() => ((IEnumerable<SceneInfo>)Scenes).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => Scenes.GetEnumerator();
}

public readonly struct SceneInfo
{
  public required string Name { get; init; }
  public required string Uuid { get; init; }
  public Guid Guid => Guid.Parse(Uuid);
  public required int Index { get; init; }

  [SetsRequiredMembers]
  public SceneInfo(string name, string uuid, int index)
  {
    Name = name;
    Uuid = uuid;
    Index = index;
  }

  [SetsRequiredMembers]
  public SceneInfo(JsonNode node)
  {
    JsonObject obj = (JsonObject)node;
    Name = (string)obj["sceneName"]!;
    Uuid = (string)obj["sceneUuid"]!;
    Index = (int)obj["sceneIndex"]!;
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
  public CurrentScene(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Name = (string)GetNode("sceneName")!;
    Uuid = (string)GetNode("sceneUuid")!;
  }
}

public class OBSSceneTransitionOverride : OBSRequestResult
{
  public required string? TransitionName { get; init; }
  public required int? TransitionDuration { get; init; }

  public OBSSceneTransitionOverride() { }

  [SetsRequiredMembers]
  public OBSSceneTransitionOverride(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    TransitionName = (string?)GetNode("transitionName");
    TransitionDuration = (int?)GetNode("transitionDuration");
  }
}
