using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public partial class OBSEvents
{

}

public class InputEventArgs : OBSEventArgs
{
  public required string InputName { get; init; }
  public required Guid InputGuid { get; init; }
  public string InputUuid => InputGuid.ToString();

  public InputEventArgs() { }

  [SetsRequiredMembers]
  public InputEventArgs(JsonObject obj) : base(obj)
  {
    InputName = (string)GetRequiredNode("inputName")!;
    InputGuid = Guid.Parse((string)GetRequiredNode("inputUuid")!);
  }
}