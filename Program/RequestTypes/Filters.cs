using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Filters
  {
    public static OBSRequest<OBSListResult<string>> GetSourceFilterKindList()
      => new OBSRequest<OBSListResult<string>>
      {
        CastResult = ResultCasts.StringList,
        RequestType = "GetSourceFilterKindList"
      };

    public static OBSRequest<OBSListResult<OBSFilter>> GetSourceFilterList(ID sourceID)
      => new OBSRequest<OBSListResult<OBSFilter>>
      {
        CastResult = ResultCasts.List(n => new OBSFilter(n)),
        RequestType = "GetSourceFilterList",
        RequestData = new JsonObject().AddID(sourceID, "source")
      };

    public static OBSRequest<OBSSingleValueResult<JsonObject>> GetSourceFilterDefaultSettings(string filterKind)
      => new OBSRequest<OBSSingleValueResult<JsonObject>>
      {
        CastResult = ResultCasts.Object,
        RequestType = "GetSourceFilterDefaultSettings",
        RequestData = new JsonObject
        {
          ["filterKind"] = filterKind
        }
      };

    public static OBSVoidRequest CreateSourceFilter(ID sourceID, string filterName, string filterKind, JsonObject? filterSettings = null)
      => new OBSVoidRequest
      {
        RequestType = "CreateSourceFilter",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName,
          ["filterKind"] = filterKind
        }.WithValueIfNotNull("filterSettings", filterSettings)
        .AddID(sourceID, "source")
      };

    public static OBSVoidRequest RemoveSourceFilter(ID sourceID, string filterName)
      => new OBSVoidRequest
      {
        RequestType = "RemoveSourceFilter",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName
        }.AddID(sourceID, "source")
      };

    public static OBSVoidRequest SetSourceFilterName(ID sourceID, string filterName, string newFilterName)
      => new OBSVoidRequest
      {
        RequestType = "SetSourceFilterName",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName,
          ["newFilterName"] = newFilterName
        }.AddID(sourceID, "source")
      };

    public static OBSRequest<FilterResult> GetSourceFilter(ID sourceID, string filterName)
      => new OBSRequest<FilterResult>
      {
        CastResult = (r, o) => new FilterResult(r, o),
        RequestType = "GetSourceFilter",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName
        }.AddID(sourceID, "source")
      };

    public static OBSVoidRequest SetSourceFilterIndex(ID sourceID, string filterName, int filterIndex)
      => new OBSVoidRequest
      {
        RequestType = "SetSourceFilterIndex",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName,
          ["filterIndex"] = filterIndex
        }.AddID(sourceID, "source")
      };

    public static OBSVoidRequest SetSourceFilterEnabled(ID sourceID, string filterName, bool filterEnabled)
      => new OBSVoidRequest
      {
        RequestType = "SetSourceFilterEnabled",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName,
          ["filterEnabled"] = filterEnabled
        }.AddID(sourceID, "source")
      };

    public static OBSVoidRequest SetSourceFilterSettings(ID sourceID, string filterName, JsonObject filterSettings,
      bool overlay = true)
      => new OBSVoidRequest
      {
        RequestType = "SetSourceFilterSettings",
        RequestData = new JsonObject
        {
          ["filterName"] = filterName,
          ["filterSettings"] = filterSettings,
          ["overlay"] = overlay
        }.AddID(sourceID, "source")
      };
  }
}

public class FilterResult : OBSRequestResult
{
  public required bool Enabled { get; init; }
  public required int Index { get; init; }
  public required string Kind { get; init; }
  public required JsonObject Settings { get; init; }

  public FilterResult() { }

  [SetsRequiredMembers]
  public FilterResult(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Enabled = (bool)GetNode("filterEnabled");
    Index = (int)GetNode("filterIndex");
    Kind = (string)GetNode("filterKind")!;
    Settings = (JsonObject)GetNode("filterSettings");
  }
}

public class OBSFilter
{
  public required JsonObject RawData { get; init; }

  public required bool Enabled { get; init; }
  public required int Index { get; init; }
  public required string Kind { get; init; }
  public required string Name { get; init; }
  public required JsonObject Settings { get; init; }

  public OBSFilter() { }

  [SetsRequiredMembers]
  public OBSFilter(JsonNode n)
  {
    JsonObject o = (JsonObject)n;

    RawData = o;

    Kind = (string)o.GetNode("filterKind")!;
    Enabled = (bool)o.GetNode("filterEnabled");
    Index = (int)o.GetNode("filterIndex");
    Name = (string)o.GetNode("filterName")!;
    Settings = (JsonObject)o.GetNode("filterSettings");
  }
}