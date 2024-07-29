using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Sources
  {
    public static OBSRequest<SourceActive> GetSourceActive(ID sourceID)
      => new OBSRequest<SourceActive>
      {
        CastResult = (r, o) => new SourceActive(r, o),
        RequestType = "GetSourceActive",
        RequestData = new JsonObject
        {
          [$"source{sourceID.Key}"] = sourceID.Value
        }
      };

    public static OBSRequest<OBSSingleValueResult<byte[]>> GetSourceScreenshot(ID sourceID, string imageFormat,
      int? imageWidth = null, int? imageHeight = null, int imageCompressionQuality = -1)
      => new OBSRequest<OBSSingleValueResult<byte[]>>
      {
        CastResult = OBSSingleValueResult<byte[]>.CastFunc(n => Convert.FromBase64String((string)n!)),
        RequestType = "GetSourceScreenshot",
        RequestData = (JsonObject)new JsonObject
        {
          [$"source{sourceID.Key}"] = sourceID.Value,
          ["imageFormat"] = imageFormat,
          ["imageCompressionQuality"] = imageCompressionQuality
        }.WithValueIf("imageWidth", imageWidth, imageWidth != null)
        .WithValueIf("imageHeight", imageHeight, imageHeight != null)
      };

    public static OBSVoidRequest SaveSourceScreenshot(ID sourceID, string imageFormat, string imageFilePath,
      int? imageWidth = null, int? imageHeight = null, int imageCompressionQuality = -1)
      => new OBSVoidRequest
      {
        RequestType = "SaveSourceScreenshot",
        RequestData = (JsonObject)new JsonObject
        {
          [$"source{sourceID.Key}"] = sourceID.Value,
          ["imageFormat"] = imageFormat,
          ["imageFilePath"] = imageFilePath,
          ["imageCompressionQuality"] = imageCompressionQuality
        }.WithValueIf("imageWidth", imageWidth, imageWidth != null)
        .WithValueIf("imageHeight", imageHeight, imageHeight != null)
      };
  }
}

public class SourceActive : OBSRequestResult
{
  public required bool VideoActive { get; init; }
  public required bool VideoShowing { get; init; }

  public SourceActive() { }

  [SetsRequiredMembers]
  public SourceActive(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    VideoActive = (bool)obj.GetNode("videoActive");
    VideoShowing = (bool)obj.GetNode("videoShowing");
  }
}