using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static partial class Inputs
  {
    public static class Types
    {
      public static class Text
      {
        public static OBSVoidRequest SetInputText(ID id, string text)
          => SetInputSettings(id, new JsonObject
          {
            ["text"] = text
          });
      }
    }
  }
}