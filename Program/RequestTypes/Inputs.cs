using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static partial class Inputs
  {
    public static OBSRequest<OBSListResult<OBSInput>> GetInputList(string? inputKind = null)
      => new OBSRequest<OBSListResult<OBSInput>>
      {
        CastResult = OBSListResult<OBSInput>.CastFunc(o => new OBSInput((JsonObject)o)),
        RequestType = "GetInputList",
        RequestData = (JsonObject)new JsonObject().WithValueIf("inputKind", inputKind, inputKind != null)
      };

    // GetInputKindList
    // GetSpecialInputs
    // CreateInput
    // RemoveInput
    // SetInputName
    // GetInputDefaultSettings

    public static OBSRequest<InputSettingsResult> GetInputSettings(ID inputID)
      => new OBSRequest<InputSettingsResult>
      {
        CastResult = (r, j) => new InputSettingsResult(r, j),
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

    public static OBSRequest<OBSSingleValueResult<MonitoringType>> GetInputAudioMonitorType(ID inputID)
      => new OBSRequest<OBSSingleValueResult<MonitoringType>>
      {
        CastResult = OBSSingleValueResult<MonitoringType>.CastFunc(n => MonitoringTypes.ForIdentifierValue((string)n!)),
        RequestType = "GetInputAudioMonitorType",
        RequestData = new JsonObject {
          inputID.KVPOf("input")
        }
      };

    public static OBSVoidRequest SetInputAudioMonitorType(ID inputID, MonitoringType newType)
      => new OBSVoidRequest
      {
        RequestType = "SetInputAudioMonitorType",
        RequestData = new JsonObject
        {
          ["monitorType"] = newType.GetIdentifierValue()
        }.AddID(inputID, "input")
      };

    public static OBSRequest<InputAudioTracksResult> GetInputAudioTracks(ID inputID)
      => new OBSRequest<InputAudioTracksResult>
      {
        CastResult = (r, o) => new InputAudioTracksResult(r, o),
        RequestType = "GetInputAudioTracks",
        RequestData = new JsonObject {
          inputID.KVPOf("input")
        }
      };

    public static OBSVoidRequest SetInputAudioTracks(ID inputID, IDictionary<string, bool> tracks)
      => new OBSVoidRequest
      {
        RequestType = "SetInputAudioTracks",
        RequestData = new JsonObject
        {
          ["inputAudioTracks"] = new JsonObject(tracks
            .Select(kvp => new KeyValuePair<string, JsonNode?>(kvp.Key, kvp.Value)))
        }.AddID(inputID, "input")
      };

    // GetInputPropertiesListPropertyItems
    // PressInputPropertiesButton
  }
}

public class OBSInput
{
  public required JsonObject RawData { get; init; }

  public required string Kind { get; init; }
  public required string Name { get; init; }
  public required string Uuid { get; init; }
  public required string UnversionedKind { get; init; }
  public Guid Guid => new Guid(Uuid);

  public OBSInput() { }

  [SetsRequiredMembers]
  public OBSInput(JsonObject o)
  {
    RawData = o;

    Kind = (string)o.GetNode("inputKind")!;
    Name = (string)o.GetNode("inputName")!;
    Uuid = (string)o.GetNode("inputUuid")!;
    UnversionedKind = (string)o.GetNode("unversionedInputKind")!;
  }
}

public class InputSettingsResult : OBSRequestResult
{
  public required JsonObject InputSettings { get; init; }
  public required string InputKind { get; init; }

  public InputSettingsResult() : base() { }

  [SetsRequiredMembers]
  public InputSettingsResult(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    InputSettings = (JsonObject)GetRequiredNode("inputSettings");
    InputKind = (string)GetRequiredNode("inputKind")!;
  }
}

public class InputAudioTracksResult : OBSRequestResult
{
  public required Dictionary<string, bool> AudioTracks { get; init; }

  public InputAudioTracksResult() { }

  [SetsRequiredMembers]
  public InputAudioTracksResult(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    AudioTracks = ((JsonObject)obj.GetNode("inputAudioTracks"))
      .Select(kvp => (kvp.Key, (bool)kvp.Value!))
      .ToDictionary();
  }
}