namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Config
  {
    // GetPersistentData
    // SetPersistentData
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