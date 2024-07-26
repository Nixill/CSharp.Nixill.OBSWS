using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static partial class Inputs
  {
    // GetInputList
    // GetInputKindList
    // GetSpecialInputs
    // CreateInput
    // RemoveInput
    // SetInputName
    // GetInputDefaultSettings

    public static OBSRequest<InputSettingsResponse> GetInputSettings(ID inputID)
      => new OBSRequest<InputSettingsResponse>
      {
        CastResult = j => new InputSettingsResponse(j),
        RequestType = "GetInputSettings",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value
        }
      };

    public static OBSVoidRequest SetInputSettings(ID inputID, JsonObject inputSettings, bool overlay = true)
      => new OBSVoidRequest
      {
        RequestType = "SetInputSettings",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputSettings"] = inputSettings,
          ["overlay"] = overlay
        }
      };

    // GetInputMute
    // SetInputMute
    // ToggleInputMute
    // GetInputVolume
    // SetInputVolume
    // GetInputAudioBalance
    // SetInputAudioBalance
    // GetInputAudioSyncOffset
    // SetInputAudioSyncOffset
    // GetInputAudioMonitorType
    // SetInputAudioMonitorType
    // GetInputAudioTracks
    // SetInputAudioTracks
    // GetInputPropertiesListPropertyItems
    // PressInputPropertiesButton
  }
}

public class InputSettingsResponse : OBSRequestResult
{
  public required JsonObject InputSettings { get; init; }
  public required string InputKind { get; init; }

  public InputSettingsResponse() : base() { }

  [SetsRequiredMembers]
  public InputSettingsResponse(JsonObject obj) : base(obj)
  {
    InputSettings = (JsonObject)GetRequiredNode("inputSettings");
    InputKind = (string)GetRequiredNode("inputKind")!;
  }
}
