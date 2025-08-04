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

    public static OBSRequest<SceneCollectionList> GetSceneCollectionList()
      => new OBSRequest<SceneCollectionList>
      {
        CastResult = (r, d) => new SceneCollectionList(r, d),
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

    public static OBSRequest<ProfileList> GetProfileList()
      => new OBSRequest<ProfileList>
      {
        CastResult = (r, d) => new ProfileList(r, d),
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

    public static OBSRequest<ProfileParameter> GetProfileParameter(string category, string name)
      => new OBSRequest<ProfileParameter>
      {
        CastResult = (r, d) => new ProfileParameter(r, d),
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

    public static OBSRequest<VideoSettings> GetVideoSettings()
      => new OBSRequest<VideoSettings>
      {
        CastResult = (r, d) => new VideoSettings(r, d),
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

    public static OBSRequest<StreamServiceSettings> GetStreamServiceSettings()
      => new OBSRequest<StreamServiceSettings>
      {
        CastResult = (r, d) => new StreamServiceSettings(r, d),
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

  public class SceneCollectionList : OBSRequestResult
  {
    public required string CurrentSceneCollectionName { get; init; }
    public required string[] SceneCollections { get; init; }

    public SceneCollectionList() { }

    [SetsRequiredMembers]
    public SceneCollectionList(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      CurrentSceneCollectionName = (string)GetNode("currentSceneCollectionName")!;
      SceneCollections = GetNode("sceneCollections").ToStringArray();
    }
  }

  public class ProfileList : OBSRequestResult
  {
    public required string CurrentProfileName { get; init; }
    public required string[] Profiles { get; init; }

    public ProfileList() { }

    [SetsRequiredMembers]
    public ProfileList(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      CurrentProfileName = (string)GetNode("currentProfileName")!;
      Profiles = GetNode("profiles").ToStringArray();
    }
  }

  public class ProfileParameter : OBSRequestResult
  {
    public required string? Value { get; init; }
    public required string? DefaultValue { get; init; }

    public ProfileParameter() { }

    [SetsRequiredMembers]
    public ProfileParameter(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      Value = (string?)GetNode("parameterValue");
      DefaultValue = (string?)GetNode("defaultParameterValue");
    }
  }

  public class VideoSettings : OBSRequestResult
  {
    public required int FPSNumerator { get; init; }
    public required int FPSDenominator { get; init; }
    public double EffectiveFPS => (double)FPSNumerator / FPSDenominator;
    public required int BaseWidth { get; init; }
    public required int BaseHeight { get; init; }
    public required int OutputWidth { get; init; }
    public required int OutputHeight { get; init; }

    public VideoSettings() { }

    [SetsRequiredMembers]
    public VideoSettings(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      FPSNumerator = (int)GetNode("fpsNumerator");
      FPSDenominator = (int)GetNode("fpsDenominator");
      BaseWidth = (int)GetNode("baseWidth");
      BaseHeight = (int)GetNode("baseHeight");
      OutputWidth = (int)GetNode("outputWidth");
      OutputHeight = (int)GetNode("outputHeight");
    }
  }

  public class StreamServiceSettings : OBSRequestResult
  {
    public required string Type { get; init; }
    public required JsonObject Settings { get; init; }

    public StreamServiceSettings() { }

    [SetsRequiredMembers]
    public StreamServiceSettings(OBSRequest req, JsonObject obj) : base(req, obj)
    {
      Type = (string)GetNode("streamServiceType")!;
      Settings = (JsonObject)GetNode("streamServiceSettings");
    }
  }
}