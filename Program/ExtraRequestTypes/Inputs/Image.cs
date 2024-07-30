using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Inputs
  {
    public static class Image
    {
      public static OBSVoidRequest SetInputImage(ID id, string path)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["file"] = path
        });
    }
  }
}
