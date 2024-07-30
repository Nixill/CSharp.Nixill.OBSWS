using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Inputs
  {
    public static class Browser
    {
      public static OBSVoidRequest SetBrowserURL(ID id, string url)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["url"] = url
        });
    }
  }
}