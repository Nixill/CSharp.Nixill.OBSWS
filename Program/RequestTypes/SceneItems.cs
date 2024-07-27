using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class SceneItems
  {
    public static OBSRequest<OBSListResult<SceneItem>> GetSceneItemList(ID sceneID)
      => new OBSRequest<OBSListResult<SceneItem>>
      {
        CastResult = OBSListResult<SceneItem>.DeserializeFunction(),
        RequestType = "GetSceneItemList",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

    // GetGroupSceneItemList

    public static OBSRequest<OBSSingleValueResult<int>> GetSceneItemId(ID sceneID, string sourceName, int searchOffset = 0)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = OBSSingleValueResult<int>.DeserializeFunction(),
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

public class SceneItem
{
  [JsonExtensionData]
  public required Dictionary<string, JsonElement>? _ExtraData;

  public string? InputKind { get; init; } = null;
  public bool? IsGroup { get; init; } = null;
  public required SceneItemBlendMode SceneItemBlendMode { get; init; }
  public required bool SceneItemEnabled { get; init; }
  public required int SceneItemId { get; init; }
  public required int SceneItemIndex { get; init; }
  public required bool SceneItemLocked { get; init; }
  public required SceneItemTransform SceneItemTransform { get; init; }
  public required string SourceName { get; init; }
  public required SourceType SourceType { get; init; }
  public required string SourceUuid { get; init; }
  public required Guid SourceGuid { get; init; }

  public SceneItem() { }
}

public class SceneItemTransform
{
  [JsonExtensionData]
  public required Dictionary<string, JsonElement>? _ExtraData;

  public required Alignment Alignment { get; init; }
  public required Alignment BoundsAlignment { get; init; }
  public required int BoundsHeight { get; init; }
  public required BoundingBoxType BoundsType { get; init; }
  public required int BoundsWidth { get; init; }
  public required int CropBottom { get; init; }
  public required int CropLeft { get; init; }
  public required int CropRight { get; init; }
  public required bool CropToBounds { get; init; }
  public required int CropTop { get; init; }
  public required double Height { get; init; }
  public required double PositionX { get; init; }
  public required double PositionY { get; init; }
  public required int Rotation { get; init; }
  public required double ScaleX { get; init; }
  public required double ScaleY { get; init; }
  public required int SourceHeight { get; init; }
  public required int SourceWidth { get; init; }
  public required double Width { get; init; }


}

public enum Alignment
{
  Center = 0,
  Left = 1,
  Right = 2,
  Top = 4,
  TopLeft = 5,
  TopRight = 6,
  Bottom = 8,
  BottomLeft = 9,
  BottomRight = 10
}

public static class AlignmentExtensions
{
  public static Alignment Horizontal(this Alignment a) => (Alignment)((int)a & 3);
  public static Alignment Vertical(this Alignment a) => (Alignment)((int)a & 12);

  public static bool IsCenter(this Alignment a) => a.Horizontal() == Alignment.Center;
  public static bool IsHorizontalCenter(this Alignment a) => a.IsCenter();
  public static bool IsLeft(this Alignment a) => a.Horizontal() == Alignment.Left;
  public static bool IsRight(this Alignment a) => a.Horizontal() == Alignment.Right;

  public static bool IsMiddle(this Alignment a) => a.Vertical() == Alignment.Center;
  public static bool IsVerticalCenter(this Alignment a) => a.IsCenter();
  public static bool IsTop(this Alignment a) => a.Vertical() == Alignment.Top;
  public static bool IsBottom(this Alignment a) => a.Vertical() == Alignment.Bottom;
}