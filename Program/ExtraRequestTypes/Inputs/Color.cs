using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Inputs
  {
    public static class Color
    {
      public static OBSVoidRequest SetSize(ID id, int width, int height)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["height"] = height,
          ["width"] = width
        });

      public static OBSVoidRequest SetColor(ID id, uint abgr)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["color"] = abgr
        });
    }
  }
}