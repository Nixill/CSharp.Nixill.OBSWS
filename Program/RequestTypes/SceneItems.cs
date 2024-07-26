using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class SceneItems
  {
    // GetSceneItemList
    // GetGroupSceneItemList

    public static OBSRequest<OBSSingleValueResult<int>> GetSceneItemId(ID sceneID, string sourceName, int searchOffset = 0)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = j => new OBSSingleValueResult<int>(j, n => (int)n),
        RequestType = "GetSceneItemId",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sourceName"] = sourceName,
          ["searchOffset"] = searchOffset
        }
      };

    // GetSceneItemSource
    // CreateSceneItem
    // RemoveSceneItem
    // DuplicateSceneItem
    // GetSceneItemTransform
    // SetSceneItemTransform
    // GetSceneItemEnabled

    public static OBSVoidRequest SetSceneItemEnabled(ID sceneID, int sceneItemID, bool sceneItemEnabled)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneItemEnabled",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID,
          ["sceneItemEnabled"] = sceneItemEnabled
        }
      };

    // GetSceneItemLocked
    // SetSceneItemLocked
    // GetSceneItemIndex
    // SetSceneItemIndex
    // GetSceneItemBlendMode
    // SetSceneItemBlendMode
  }
}