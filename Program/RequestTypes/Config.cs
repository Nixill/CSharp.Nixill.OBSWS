using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Config
  {
    public static OBSRequest<OBSSingleValueResult<JsonNode>> GetPersistentData(OBSWebsocketDataRealm realm, string slotName)
      => new OBSRequest<OBSSingleValueResult<JsonNode>>
      {
        CastResult = OBSSingleValueResult<JsonNode>.CastFunc(n => n),
        RequestType = "GetPersistentData",
        RequestData = new JsonObject
        {
          ["realm"] = realm.GetIdentifierValue(),
          ["slotName"] = slotName
        }
      };

    public static OBSVoidRequest SetPersistentData(OBSWebsocketDataRealm realm, string slotName, JsonNode value)
      => new OBSVoidRequest
      {
        RequestType = "SetPersistentData",
        RequestData = new JsonObject
        {
          ["realm"] = realm.GetIdentifierValue(),
          ["slotName"] = slotName,
          ["value"] = value
        }
      };

    public static OBSRequest<OBSSceneCollectionList> GetSceneCollectionList()
      => new OBSRequest<OBSSceneCollectionList>
      {
        CastResult = (r, d) => new OBSSceneCollectionList(r, d),
        RequestType = "GetSceneCollectionList"
      };

    public static OBSVoidRequest SetCurrentSceneCollection(string name)
      => new OBSVoidRequest
      {
        RequestType = "SetCurrentSceneCollection",
        RequestData = new JsonObject
        {
          ["sceneCollectionName"] = name
        }
      };

    public static OBSVoidRequest CreateSceneCollection(string name)
      => new OBSVoidRequest
      {
        RequestType = "CreateSceneCollection",
        RequestData = new JsonObject
        {
          ["sceneCollectionName"] = name
        }
      };

    public static OBSRequest<OBSProfileList> GetProfileList()
      => new OBSRequest<OBSProfileList>
      {
        CastResult = (r, d) => new OBSProfileList(r, d),
        RequestType = "GetProfileList"
      };

    public static OBSVoidRequest SetCurrentProfile(string name)
      => new OBSVoidRequest
      {
        RequestType = "SetCurrentProfile",
        RequestData = new JsonObject
        {
          ["profileName"] = name
        }
      };

    public static OBSVoidRequest CreateProfile(string name)
      => new OBSVoidRequest
      {
        RequestType = "CreateProfile",
        RequestData = new JsonObject
        {
          ["profileName"] = name
        }
      };

    public static OBSVoidRequest RemoveProfile(string name)
      => new OBSVoidRequest
      {
        RequestType = "RemoveProfile",
        RequestData = new JsonObject
        {
          ["profileName"] = name
        }
      };

    public static OBSRequest<OBSProfileParameter> GetProfileParameter(string category, string name)
      => new OBSRequest<OBSProfileParameter>
      {
        CastResult = (r, d) => new OBSProfileParameter(r, d),
        RequestType = "GetProfileParameter",
        RequestData = new JsonObject
        {
          ["parameterCategory"] = category,
          ["parameterName"] = name
        }
      };

    public static OBSVoidRequest SetProfileParameter(string category, string name, string? value)
      => new OBSVoidRequest
      {
        RequestType = "SetProfileParameter",
        RequestData = new JsonObject
        {
          ["parameterCategory"] = category,
          ["parameterName"] = name,
          ["parameterValue"] = value
        }
      };

    public static OBSRequest<OBSVideoSettings> GetVideoSettings()
      => new OBSRequest<OBSVideoSettings>
      {
        CastResult = (r, d) => new OBSVideoSettings(r, d),
        RequestType = "GetVideoSettings"
      };

    public static OBSVoidRequest SetVideoSettings((int Numerator, int Denominator)? fps = null,
      (int Width, int Height)? baseSize = null, (int Width, int Height)? outputSize = null)
    {
      JsonObject data = [];
      if (fps.HasValue) (data["fpsNumerator"], data["fpsDenominator"]) = fps.Value;
      if (baseSize.HasValue) (data["baseWidth"], data["baseHeight"]) = baseSize.Value;
      if (outputSize.HasValue) (data["outputWidth"], data["outputHeight"]) = outputSize.Value;

      return new OBSVoidRequest
      {
        RequestType = "SetVideoSettings",
        RequestData = data
      };
    }

    public static OBSRequest<OBSStreamServiceSettings> GetStreamServiceSettings()
      => new OBSRequest<OBSStreamServiceSettings>
      {
        CastResult = (r, d) => new OBSStreamServiceSettings(r, d),
        RequestType = "GetStreamServiceSettings"
      };

    public static OBSVoidRequest SetStreamServiceSettings(string type, JsonObject settings)
      => new OBSVoidRequest
      {
        RequestType = "SetStreamServiceSettings",
        RequestData = new JsonObject
        {
          ["streamServiceType"] = type,
          ["streamServiceSettings"] = settings
        }
      };

    public static OBSRequest<OBSSingleValueResult<string>> GetRecordDirectory()
      => new OBSRequest<OBSSingleValueResult<string>>
      {
        CastResult = OBSSingleValueResult<string>.CastFunc(x => (string)x!),
        RequestType = "GetRecordDirectory"
      };

    public static OBSVoidRequest SetRecordDirectory(string directory)
      => new OBSVoidRequest
      {
        RequestType = "SetRecordDirectory",
        RequestData = new JsonObject
        {
          ["recordDirectory"] = directory
        }
      };
  }

  public class OBSSceneCollectionList : OBSRequestResult
  {
    public required string CurrentSceneCollectionName { get; init; }
    public required string[] SceneCollections { get; init; }

    public OBSSceneCollectionList() { }

    [SetsRequiredMembers]
    public OBSSceneCollectionList(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      CurrentSceneCollectionName = (string)GetRequiredNode("currentSceneCollectionName")!;
      SceneCollections = GetRequiredNode("sceneCollections").ToStringArray();
    }
  }

  public class OBSProfileList : OBSRequestResult
  {
    public required string CurrentProfileName { get; init; }
    public required string[] Profiles { get; init; }

    public OBSProfileList() { }

    [SetsRequiredMembers]
    public OBSProfileList(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      CurrentProfileName = (string)GetRequiredNode("currentProfileName")!;
      Profiles = GetRequiredNode("profiles").ToStringArray();
    }
  }

  public class OBSProfileParameter : OBSRequestResult
  {
    public required string? Value { get; init; }
    public required string? DefaultValue { get; init; }

    public OBSProfileParameter() { }

    [SetsRequiredMembers]
    public OBSProfileParameter(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      Value = (string?)GetRequiredNode("parameterValue");
      DefaultValue = (string?)GetRequiredNode("defaultParameterValue");
    }
  }

  public class OBSVideoSettings : OBSRequestResult
  {
    public required int FPSNumerator { get; init; }
    public required int FPSDenominator { get; init; }
    public double EffectiveFPS => (double)FPSNumerator / FPSDenominator;
    public required int BaseWidth { get; init; }
    public required int BaseHeight { get; init; }
    public required int OutputWidth { get; init; }
    public required int OutputHeight { get; init; }

    public OBSVideoSettings() { }

    [SetsRequiredMembers]
    public OBSVideoSettings(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      FPSNumerator = (int)GetRequiredNode("fpsNumerator");
      FPSDenominator = (int)GetRequiredNode("fpsDenominator");
      BaseWidth = (int)GetRequiredNode("baseWidth");
      BaseHeight = (int)GetRequiredNode("baseHeight");
      OutputWidth = (int)GetRequiredNode("outputWidth");
      OutputHeight = (int)GetRequiredNode("outputHeight");
    }
  }

  public class OBSStreamServiceSettings : OBSRequestResult
  {
    public required string StreamServiceType { get; init; }
    public required JsonObject StreamServiceSettings { get; init; }

    public OBSStreamServiceSettings() { }

    [SetsRequiredMembers]
    public OBSStreamServiceSettings(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      StreamServiceType = (string)GetRequiredNode("streamServiceType")!;
      StreamServiceSettings = (JsonObject)GetRequiredNode("streamServiceSettings");
    }
  }
}