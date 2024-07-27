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

    public static OBSRequest<InputSettingsResponse> GetInputSettings(ID inputId)
      => new OBSRequest<InputSettingsResponse>
      {
        RequestType = "GetInputSettings",
        RequestData = new JsonObject
        {
          [$"input{inputId.Key}"] = inputId.Value
        }
      };

    public static OBSVoidRequest SetInputSettings(ID inputId, JsonObject inputSettings, bool overlay = true)
      => new OBSVoidRequest
      {
        RequestType = "SetInputSettings",
        RequestData = new JsonObject
        {
          [$"input{inputId.Key}"] = inputId.Value,
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
}
