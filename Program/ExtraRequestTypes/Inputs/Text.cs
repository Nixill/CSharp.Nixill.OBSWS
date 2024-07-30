using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Inputs
  {
    public static class Text
    {
      public static OBSVoidRequest SetInputText(ID id, string text)
        => OBSRequests.Inputs.SetInputSettings(id, new JsonObject
        {
          ["text"] = text
        });
    }
  }
}
