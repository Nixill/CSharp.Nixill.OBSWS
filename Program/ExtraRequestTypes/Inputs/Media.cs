using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Inputs
  {
    public static class Media
    {
      public static OBSVoidRequest SetMediaFile(ID id, string localFile)
        => OBSRequests.Inputs.SetInputSetting(id, "local_file", localFile);
    }
  }
}