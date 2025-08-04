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

    public static OBSRequest<OBSListResult<string>> GetInputKindList(bool unversioned = false)
      => new OBSRequest<OBSListResult<string>>
      {
        CastResult = OBSListResult<string>.CastFunc(n => (string)n!),
        RequestType = "GetInputKindList",
        RequestData = new JsonObject { ["unversioned"] = unversioned }
      };

    // GetSpecialInputs
    // CreateInput
    // RemoveInput
    // SetInputName

    public static OBSRequest<OBSSingleValueResult<JsonObject>> GetInputDefaultSettings(string inputKind)
      => new OBSRequest<OBSSingleValueResult<JsonObject>>
      {
        CastResult = OBSSingleValueResult<JsonObject>.CastFunc(n => (JsonObject)n),
        RequestType = "GetInputDefaultSettings",
        RequestData = new JsonObject
        {
          ["inputKind"] = inputKind
        }
      };

    public static OBSRequest<InputSettings> GetInputSettings(ID inputID)
      => new OBSRequest<InputSettings>
      {
        CastResult = (r, j) => new InputSettings(r, j),
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

    public static OBSRequest<InputAudioTracks> GetInputAudioTracks(ID inputID)
      => new OBSRequest<InputAudioTracks>
      {
        CastResult = (r, o) => new InputAudioTracks(r, o),
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

public class InputSettings : OBSRequestResult
{
  public required JsonObject Settings { get; init; }
  public required string Kind { get; init; }

  public InputSettings() : base() { }

  [SetsRequiredMembers]
  public InputSettings(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Settings = (JsonObject)GetNode("inputSettings");
    Kind = (string)GetNode("inputKind")!;
  }
}

public class InputAudioTracks : OBSRequestResult
{
  public required Dictionary<string, bool> AudioTracks { get; init; }

  public InputAudioTracks() { }

  [SetsRequiredMembers]
  public InputAudioTracks(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    AudioTracks = ((JsonObject)GetNode("inputAudioTracks"))
      .Select(kvp => (kvp.Key, (bool)kvp.Value!))
      .ToDictionary();
  }
}