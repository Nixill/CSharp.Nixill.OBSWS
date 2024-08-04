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

    // GetSceneCollectionList
    // SetCurrentSceneCollection
    // CreateSceneCollection
    // GetProfileList
    // SetCurrentProfile
    // CreateProfile
    // RemoveProfile
    // GetProfileParameter
    // SetProfileParameter
    // GetVideoSettings
    // SetVideoSettings
    // GetStreamServiceSettings
    // SetStreamServiceSettings

    public static OBSRequest<OBSSingleValueResult<string>> GetRecordDirectory()
      => new OBSRequest<OBSSingleValueResult<string>>
      {
        CastResult = OBSSingleValueResult<string>.CastFunc(x => (string)x!),
        RequestType = "GetRecordDirectory"
      };

    // SetRecordDirectory
  }
}