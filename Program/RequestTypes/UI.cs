namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public class UI
  {
    public static OBSRequest<OBSSingleValueResult<bool>> GetStudioModeEnabled()
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = OBSSingleValueResult<bool>.CastFunc(n => (bool)n),
        RequestType = "GetStudioModeEnabled"
      };
    // SetStudioModeEnabled
    // OpenInputPropertiesDialog
    // OpenInputFiltersDialog
    // GetMonitorList
    // OpenVideoMixProjector
    // OpenSourceProjector
  }
}