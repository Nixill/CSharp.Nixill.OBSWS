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

      public static OBSVoidRequest SetColor(ID id, System.Drawing.Color color)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["color"] = ((uint)color.R) << 0 | ((uint)color.G) << 8 | ((uint)color.B) << 16 | ((uint)color.A) << 24
        });

      public static OBSVoidRequest SetColorABGR(ID id, uint abgr)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["color"] = abgr
        });

      public static OBSVoidRequest SetColorARGB(ID id, uint argb)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["color"] = (argb & 0xFF00FF00) | ((argb & 0x00FF0000) >> 16) | ((argb & 0x000000FF) << 16)
        });

      public static OBSVoidRequest SetColorRGBA(ID id, uint rgba)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["color"] = ((rgba & 0xFF000000) >> 24) | ((rgba & 0x00FF0000) >> 8) | ((rgba & 0x0000FF00) << 8) | ((rgba & 0x000000FF) << 24)
        });
    }
  }
}