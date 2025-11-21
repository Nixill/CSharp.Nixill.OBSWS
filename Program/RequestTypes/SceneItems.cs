using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class SceneItems
  {
    public static OBSRequest<OBSListResult<SceneItem>> GetSceneItemList(ID sceneID)
      => new OBSRequest<OBSListResult<SceneItem>>
      {
        CastResult = ResultCasts.List(o => new SceneItem((JsonObject)o)),
        RequestType = "GetSceneItemList",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value
        }
      };

    public static OBSRequest<OBSListResult<SceneItem>> GetGroupSceneItemList(ID groupID)
      => new OBSRequest<OBSListResult<SceneItem>>
      {
        CastResult = ResultCasts.List(o => new SceneItem((JsonObject)o)),
        RequestType = "GetGroupSceneItemList",
        RequestData = new JsonObject
        {
          [$"scene{groupID.Key}"] = groupID.Value
        }
      };

    public static OBSRequest<OBSSingleValueResult<int>> GetSceneItemId(ID sceneID, string sourceName, int searchOffset = 0)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = ResultCasts.Int,
        RequestType = "GetSceneItemId",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sourceName"] = sourceName,
          ["searchOffset"] = searchOffset
        }
      };

    public static OBSRequest<SceneItemSource> GetSceneItemSource(ID sceneID, int sceneItemID)
      => new OBSRequest<SceneItemSource>
      {
        CastResult = (r, o) => new SceneItemSource(r, o),
        RequestType = "GetSceneItemSource",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID
        }
      };

    public static OBSRequest<OBSSingleValueResult<int>> CreateSceneItem(ID sceneID, ID sourceID, bool sceneItemEnabled = true)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = ResultCasts.Int,
        RequestType = "CreateSceneItem",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          [$"source{sourceID.Key}"] = sourceID.Value,
          ["sceneItemEnabled"] = sceneItemEnabled
        }
      };

    public static OBSVoidRequest RemoveSceneItem(ID sceneID, int sceneItemID)
      => new OBSVoidRequest
      {
        RequestType = "RemoveSceneItem",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID
        }
      };

    public static OBSRequest<OBSSingleValueResult<int>> DuplicateSceneItem(ID sceneID, int sceneItemID,
      ID? destinationSceneID = null)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = ResultCasts.Int,
        RequestType = "DuplicateSceneItem",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID,
          [$"destinationScene{destinationSceneID?.Key ?? sceneID.Key}"] = destinationSceneID?.Value ?? sceneID.Value
        }
      };

    public static OBSRequest<OBSSingleValueResult<SceneItemTransform>> GetSceneItemTransform(ID sceneID, int sceneItemID)
      => new OBSRequest<OBSSingleValueResult<SceneItemTransform>>
      {
        CastResult = ResultCasts.SingleValue(n => new SceneItemTransform((JsonObject)n)),
        RequestType = "GetSceneItemTransform",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID
        }.AddID(sceneID, "scene")
      };

    public static OBSVoidRequest SetSceneItemTransform(ID sceneID, int sceneItemID,
      SceneItemTransformSetter sceneItemTransform)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneItemTransform",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID,
          ["sceneItemTransform"] = sceneItemTransform.ToJson()
        }.AddID(sceneID, "scene")
      };

    public static OBSRequest<OBSSingleValueResult<bool>> GetSceneItemEnabled(ID sceneID, int sceneItemID)
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = ResultCasts.Bool,
        RequestType = "GetSceneItemEnabled",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID
        }.AddID(sceneID, "scene")
      };

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

    public static OBSRequest<OBSSingleValueResult<bool>> GetSceneItemLocked(ID sceneID, int sceneItemID)
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = ResultCasts.Bool,
        RequestType = "GetSceneItemLocked",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID
        }.AddID(sceneID, "scene")
      };

    public static OBSVoidRequest SetSceneItemLocked(ID sceneID, int sceneItemID, bool sceneItemLocked)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneItemLocked",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID,
          ["sceneItemLocked"] = sceneItemLocked
        }
      };

    public static OBSRequest<OBSSingleValueResult<int>> GetSceneItemIndex(ID sceneID, int sceneItemID)
      => new OBSRequest<OBSSingleValueResult<int>>
      {
        CastResult = ResultCasts.Int,
        RequestType = "GetSceneItemIndex",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID
        }.AddID(sceneID, "scene")
      };

    public static OBSVoidRequest SetSceneItemIndex(ID sceneID, int sceneItemID, int sceneItemIndex)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneItemIndex",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID,
          ["sceneItemIndex"] = sceneItemIndex
        }
      };

    public static OBSRequest<OBSSingleValueResult<SceneItemBlendMode>> GetSceneItemBlendMode(ID sceneID, int sceneItemID)
      => new OBSRequest<OBSSingleValueResult<SceneItemBlendMode>>
      {
        CastResult = ResultCasts.Enum(SceneItemBlendModes.ForIdentifierValue),
        RequestType = "GetSceneItemBlendMode",
        RequestData = new JsonObject
        {
          ["sceneItemId"] = sceneItemID
        }.AddID(sceneID, "scene")
      };

    public static OBSVoidRequest SetSceneItemBlendMode(ID sceneID, int sceneItemID, SceneItemBlendMode sceneItemBlendMode)
      => new OBSVoidRequest
      {
        RequestType = "SetSceneItemBlendMode",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["sceneItemId"] = sceneItemID,
          ["sceneItemBlendMode"] = sceneItemBlendMode.GetIdentifierValue()
        }
      };
  }
}

public class SceneItem
{
  public JsonObject? RawData;

  public string? InputKind { get; init; } = null;
  public bool? IsGroup { get; init; } = null;
  public required SceneItemBlendMode SceneItemBlendMode { get; init; }
  public required bool SceneItemEnabled { get; init; }
  public required int SceneItemID { get; init; }
  public required int SceneItemIndex { get; init; }
  public required bool SceneItemLocked { get; init; }
  public required SceneItemTransform SceneItemTransform { get; init; }
  public required string SourceName { get; init; }
  public required SourceType SourceType { get; init; }
  public required string SourceUuid { get; init; }
  public Guid SourceGuid => new Guid(SourceUuid);

  public SceneItem() { }

  [SetsRequiredMembers]
  public SceneItem(JsonObject o)
  {
    RawData = o;

    InputKind = (string?)o["inputKind"];
    IsGroup = (bool?)o["isGroup"];
    SceneItemBlendMode = SceneItemBlendModes.ForIdentifierValue((string)o.GetNode("sceneItemBlendMode")!);
    SceneItemEnabled = (bool)o.GetNode("sceneItemEnabled");
    SceneItemID = (int)o.GetNode("sceneItemId");
    SceneItemIndex = (int)o.GetNode("sceneItemIndex");
    SceneItemLocked = (bool)o.GetNode("sceneItemLocked");
    SceneItemTransform = new SceneItemTransform((JsonObject)o.GetNode("sceneItemTransform"));
    SourceName = (string)o.GetNode("sourceName")!;
    SourceType = SourceTypes.ForIdentifierValue((string)o.GetNode("sourceType")!);
    SourceUuid = (string)o.GetNode("sourceUuid")!;
  }
}

public class SceneItemTransformSetter
{
  public Alignment? Alignment { get; init; } = null;
  public Alignment? BoundsAlignment { get; init; } = null;
  public double? BoundsHeight { get; init; } = null;
  public BoundingBoxType? BoundsType { get; init; } = null;
  public double? BoundsWidth { get; init; } = null;
  public int? CropBottom { get; init; } = null;
  public int? CropLeft { get; init; } = null;
  public int? CropRight { get; init; } = null;
  public bool? CropToBounds { get; init; } = null;
  public int? CropTop { get; init; } = null;
  public double? Height { get; init; } = null;
  public double? PositionX { get; init; } = null;
  public double? PositionY { get; init; } = null;
  public double? Rotation { get; init; } = null;
  public double? ScaleX { get; init; } = null;
  public double? ScaleY { get; init; } = null;
  public double? Width { get; init; } = null;

  public SceneItemTransformSetter() { }

  public SceneItemTransformSetter(JsonObject o)
  {
    Alignment = (Alignment)(int)o.GetNode("alignment");
    BoundsAlignment = (Alignment)(int)o.GetNode("boundsAlignment");
    BoundsHeight = (double)o.GetNode("boundsHeight");
    BoundsType = BoundingBoxTypes.ForIdentifierValue((string)o.GetNode("boundsType")!);
    BoundsWidth = (double)o.GetNode("boundsWidth");
    CropBottom = (int)o.GetNode("cropBottom");
    CropLeft = (int)o.GetNode("cropLeft");
    CropRight = (int)o.GetNode("cropRight");
    CropToBounds = (bool)o.GetNode("cropToBounds");
    CropTop = (int)o.GetNode("cropTop");
    Height = (double)o.GetNode("height");
    PositionX = (double)o.GetNode("positionX");
    PositionY = (double)o.GetNode("positionY");
    Rotation = (double)o.GetNode("rotation");
    ScaleX = (double)o.GetNode("scaleX");
    ScaleY = (double)o.GetNode("scaleY");
    Width = (double)o.GetNode("width");
  }

  public SceneItemTransformSetter(SceneItemTransform sit)
  {
    Alignment = sit.Alignment;
    BoundsAlignment = sit.BoundsAlignment;
    BoundsHeight = sit.BoundsHeight;
    BoundsType = sit.BoundsType;
    BoundsWidth = sit.BoundsWidth;
    CropBottom = sit.CropBottom;
    CropLeft = sit.CropLeft;
    CropRight = sit.CropRight;
    CropToBounds = sit.CropToBounds;
    CropTop = sit.CropTop;
    Height = sit.Height;
    PositionX = sit.PositionX;
    PositionY = sit.PositionY;
    Rotation = sit.Rotation;
    ScaleX = sit.ScaleX;
    ScaleY = sit.ScaleY;
    Width = sit.Width;
  }

  public JsonObject ToJson()
  {
    JsonObject obj = [];
    if (Alignment != null) obj["alignment"] = (int)Alignment;
    if (BoundsAlignment != null) obj["boundsAlignment"] = (int)BoundsAlignment;
    if (BoundsHeight != null) obj["boundsHeight"] = BoundsHeight;
    if (BoundsType != null) obj["boundsType"] = BoundingBoxTypes.GetIdentifierValue(BoundsType.Value);
    if (BoundsWidth != null) obj["boundsWidth"] = BoundsWidth;
    if (CropBottom != null) obj["cropBottom"] = CropBottom;
    if (CropLeft != null) obj["cropLeft"] = CropLeft;
    if (CropRight != null) obj["cropRight"] = CropRight;
    if (CropToBounds != null) obj["cropToBounds"] = CropToBounds;
    if (CropTop != null) obj["cropTop"] = CropTop;
    if (Height != null) obj["height"] = Height;
    if (PositionX != null) obj["positionX"] = PositionX;
    if (PositionY != null) obj["positionY"] = PositionY;
    if (Rotation != null) obj["rotation"] = Rotation;
    if (ScaleX != null) obj["scaleX"] = ScaleX;
    if (ScaleY != null) obj["scaleY"] = ScaleY;
    if (Width != null) obj["width"] = Width;
    return obj;
  }
}

public class SceneItemTransform
{
  public JsonObject? RawData;

  public required Alignment Alignment { get; init; }
  public required Alignment BoundsAlignment { get; init; }
  public required double BoundsHeight { get; init; }
  public required BoundingBoxType BoundsType { get; init; }
  public required double BoundsWidth { get; init; }
  public required int CropBottom { get; init; }
  public required int CropLeft { get; init; }
  public required int CropRight { get; init; }
  public required bool CropToBounds { get; init; }
  public required int CropTop { get; init; }
  public required double Height { get; init; }
  public required double PositionX { get; init; }
  public required double PositionY { get; init; }
  public required double Rotation { get; init; }
  public required double ScaleX { get; init; }
  public required double ScaleY { get; init; }
  public required double SourceHeight { get; init; }
  public required double SourceWidth { get; init; }
  public required double Width { get; init; }

  public SceneItemTransform() { }

  [SetsRequiredMembers]
  public SceneItemTransform(JsonObject o)
  {
    RawData = o;

    Alignment = (Alignment)(int)o.GetNode("alignment");
    BoundsAlignment = (Alignment)(int)o.GetNode("boundsAlignment");
    BoundsHeight = (double)o.GetNode("boundsHeight");
    BoundsType = BoundingBoxTypes.ForIdentifierValue((string)o.GetNode("boundsType")!);
    BoundsWidth = (double)o.GetNode("boundsWidth");
    CropBottom = (int)o.GetNode("cropBottom");
    CropLeft = (int)o.GetNode("cropLeft");
    CropRight = (int)o.GetNode("cropRight");
    CropToBounds = (bool)o.GetNode("cropToBounds");
    CropTop = (int)o.GetNode("cropTop");
    Height = (double)o.GetNode("height");
    PositionX = (double)o.GetNode("positionX");
    PositionY = (double)o.GetNode("positionY");
    Rotation = (double)o.GetNode("rotation");
    ScaleX = (double)o.GetNode("scaleX");
    ScaleY = (double)o.GetNode("scaleY");
    SourceHeight = (double)o.GetNode("sourceHeight");
    SourceWidth = (double)o.GetNode("sourceWidth");
    Width = (double)o.GetNode("width");
  }
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

public class SceneItemSource : OBSRequestResult
{
  public required string Name { get; init; }
  public required string Uuid { get; init; }
  public Guid Guid => Guid.Parse(Uuid);

  public SceneItemSource() { }

  [SetsRequiredMembers]
  public SceneItemSource(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Name = (string)obj["sourceName"]!;
    Uuid = (string)obj["sourceUuid"]!;
  }
}