using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSRequests
{
  public static class Inputs
  {
    public static OBSRequest<OBSListResult<OBSInput>> GetInputList(string? inputKind = null)
      => new OBSRequest<OBSListResult<OBSInput>>
      {
        CastResult = ResultCasts.List(o => new OBSInput((JsonObject)o)),
        RequestType = "GetInputList",
        RequestData = new JsonObject().WithValueIfNotNull("inputKind", inputKind)
      };

    public static OBSRequest<OBSListResult<string>> GetInputKindList(bool unversioned = false)
      => new OBSRequest<OBSListResult<string>>
      {
        CastResult = ResultCasts.StringList,
        RequestType = "GetInputKindList",
        RequestData = new JsonObject { ["unversioned"] = unversioned }
      };

    public static OBSRequest<SpecialInputs> GetSpecialInputs()
      => new OBSRequest<SpecialInputs>
      {
        CastResult = (r, d) => new SpecialInputs(r, d),
        RequestType = "GetSpecialInputs"
      };

    public static OBSRequest<NewInput> CreateInput(ID sceneID, string inputName, string inputKind,
      JsonObject? inputSettings = null, bool sceneItemEnabled = true)
      => new OBSRequest<NewInput>
      {
        CastResult = (r, d) => new NewInput(r, d),
        RequestType = "CreateInput",
        RequestData = new JsonObject
        {
          [$"scene{sceneID.Key}"] = sceneID.Value,
          ["inputName"] = inputName,
          ["inputKind"] = inputKind,
          ["sceneItemEnabled"] = sceneItemEnabled
        }.WithValueIfNotNull("inputSettings", inputSettings)
      };

    public static OBSVoidRequest RemoveInput(ID inputID)
      => new OBSVoidRequest
      {
        RequestType = "RemoveInput",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value
        }
      };

    public static OBSVoidRequest SetInputName(ID inputID, string newInputName)
      => new OBSVoidRequest
      {
        RequestType = "SetInputName",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["newInputName"] = newInputName
        }
      };

    public static OBSRequest<OBSSingleValueResult<JsonObject>> GetInputDefaultSettings(string inputKind)
      => new OBSRequest<OBSSingleValueResult<JsonObject>>
      {
        CastResult = ResultCasts.Object,
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

    public static OBSVoidRequest SetInputSetting(ID inputID, string setting, JsonNode value, bool overlay = true)
      => SetInputSettings(inputID, new JsonObject { [setting] = value }, overlay);

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

    public static OBSRequest<OBSSingleValueResult<bool>> GetInputMute(ID inputID)
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = ResultCasts.Bool,
        RequestType = "GetInputMute",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value
        }
      };

    public static OBSVoidRequest SetInputMute(ID inputID, bool inputMuted)
      => new OBSVoidRequest
      {
        RequestType = "SetInputMute",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputMuted"] = inputMuted
        }
      };

    public static OBSRequest<OBSSingleValueResult<bool>> ToggleInputMute(ID inputID)
      => new OBSRequest<OBSSingleValueResult<bool>>
      {
        CastResult = ResultCasts.Bool,
        RequestType = "ToggleInputMute",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value
        }
      };

    public static OBSRequest<InputVolumes> GetInputVolume(ID inputID)
      => new OBSRequest<InputVolumes>
      {
        CastResult = (r, d) => new InputVolumes(r, d),
        RequestType = "GetInputVolume",
        RequestData = [inputID.KVPOf("input")]
      };

    public static OBSVoidRequest SetInputVolume(ID inputID, VolumeLevel level)
      => new OBSVoidRequest
      {
        RequestType = "SetInputVolume",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputVolumeDb"] = level.Decibels
        }
      };

    public static OBSRequest<OBSSingleValueResult<double>> GetInputAudioBalance(ID inputID)
      => new OBSRequest<OBSSingleValueResult<double>>
      {
        CastResult = ResultCasts.Double,
        RequestType = "GetInputAudioBalance",
        RequestData = [inputID.KVPOf("input")]
      };

    public static OBSVoidRequest SetInputAudioBalance(ID inputID, double balance)
      => new OBSVoidRequest
      {
        RequestType = "SetInputAudioBalance",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputAudioBalance"] = balance
        }
      };

    public static OBSRequest<OBSSingleValueResult<double>> GetInputAudioSyncOffset(ID inputID)
      => new OBSRequest<OBSSingleValueResult<double>>
      {
        CastResult = ResultCasts.Double,
        RequestType = "GetInputAudioSyncOffset",
        RequestData = [inputID.KVPOf("input")]
      };

    public static OBSVoidRequest SetInputAudioSyncOffset(ID inputID, double offset)
      => new OBSVoidRequest
      {
        RequestType = "SetInputAudioBalance",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputAudioSyncOffset"] = offset
        }
      };

    public static OBSRequest<OBSSingleValueResult<MonitoringType>> GetInputAudioMonitorType(ID inputID)
      => new OBSRequest<OBSSingleValueResult<MonitoringType>>
      {
        CastResult = ResultCasts.SingleValue(n => MonitoringTypes.ForIdentifierValue((string)n!)),
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
          [$"input{inputID.Key}"] = inputID.Value,
          ["monitorType"] = newType.GetIdentifierValue()
        }
      };

    public static OBSRequest<InputAudioTracks> GetInputAudioTracks(ID inputID)
      => new OBSRequest<InputAudioTracks>
      {
        CastResult = (r, o) => new InputAudioTracks(r, o),
        RequestType = "GetInputAudioTracks",
        RequestData = [inputID.KVPOf("input")]
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

    public static OBSRequest<OBSSingleValueResult<DeinterlaceMode>> GetInputDeinterlaceMode(ID inputID)
      => new OBSRequest<OBSSingleValueResult<DeinterlaceMode>>
      {
        CastResult = ResultCasts.SingleValue(n => DeinterlaceModes.ForIdentifierValue((string)n!)),
        RequestType = "GetInputDeinterlaceMode",
        RequestData = [inputID.KVPOf("input")]
      };

    public static OBSVoidRequest SetInputDeinterlaceMode(ID inputID, DeinterlaceMode mode)
      => new OBSVoidRequest
      {
        RequestType = "SetInputDeinterlaceMode",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputDeinterlaceMode"] = mode.GetIdentifierValue()
        }
      };

    public static OBSRequest<OBSSingleValueResult<DeinterlaceFieldOrder>> GetInputDeinterlaceFieldOrder(ID inputID)
      => new OBSRequest<OBSSingleValueResult<DeinterlaceFieldOrder>>
      {
        CastResult = ResultCasts.SingleValue(n => DeinterlaceFieldOrders.ForIdentifierValue((string)n!)),
        RequestType = "GetInputDeinterlaceFieldOrder",
        RequestData = [inputID.KVPOf("input")]
      };

    public static OBSVoidRequest SetInputDeinterlaceFieldOrder(ID inputID, DeinterlaceFieldOrder fieldOrder)
      => new OBSVoidRequest
      {
        RequestType = "SetInputDeinterlaceFieldOrder",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["inputDeinterlaceFieldOrder"] = fieldOrder.GetIdentifierValue()
        }
      };

    public static OBSRequest<OBSListResult<PropertyItem>> GetInputPropertiesListPropertyItems(ID inputID,
      string propertyName)
      => new OBSRequest<OBSListResult<PropertyItem>>
      {
        CastResult = ResultCasts.List(n => new PropertyItem(n)),
        RequestType = "GetInputPropertiesListPropertyItems",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["propertyName"] = propertyName
        }
      };

    public static OBSVoidRequest PressInputPropertiesButton(ID inputID, string propertyName)
      => new OBSVoidRequest
      {
        RequestType = "PressInputPropertiesButton",
        RequestData = new JsonObject
        {
          [$"input{inputID.Key}"] = inputID.Value,
          ["propertyName"] = propertyName
        }
      };
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

public class SpecialInputs : OBSRequestResult
{
  public required string? Desktop1 { get; init; }
  public required string? Desktop2 { get; init; }
  public required string? Mic1 { get; init; }
  public required string? Mic2 { get; init; }
  public required string? Mic3 { get; init; }
  public required string? Mic4 { get; init; }

  public SpecialInputs() { }

  [SetsRequiredMembers]
  public SpecialInputs(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Desktop1 = (string?)GetNode("desktop1");
    Desktop2 = (string?)GetNode("desktop2");
    Mic1 = (string?)GetNode("mic1");
    Mic2 = (string?)GetNode("mic2");
    Mic3 = (string?)GetNode("mic3");
    Mic4 = (string?)GetNode("mic4");
  }
}

public class NewInput : OBSRequestResult
{
  public required string InputUuid { get; init; }
  public Guid InputGuid => Guid.Parse(InputUuid);
  public required int SceneItemID { get; init; }

  public NewInput() { }

  [SetsRequiredMembers]
  public NewInput(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    InputUuid = (string)GetNode("inputUuid")!;
    SceneItemID = (int)GetNode("sceneItemId");
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

public class InputVolumes : OBSRequestResult
{
  public required VolumeLevel Level { get; init; }

  public InputVolumes() { }

  [SetsRequiredMembers]
  public InputVolumes(OBSRequest req, JsonObject obj) : base(req, obj)
  {
    Level = VolumeLevel.FromDecibels((double)GetNode("inputVolumeDb"));
  }
}

public readonly struct VolumeLevel
{
  private readonly double _db;

  public double Decibels
  {
    get => _db;
    init => _db = Math.Clamp(value, -100, 26);
  }
  public static VolumeLevel FromDecibels(double db) => new VolumeLevel { Decibels = db };

  public double Multiplier
  {
    get => (_db == -100) ? 0 : Math.Pow(10, _db / 20);
    init => _db = (value <= 0) ? -100
      : Math.Clamp(20 * Math.Log10(value), -100, 26);
  }
  public static VolumeLevel FromMultiplier(double mul) => new VolumeLevel { Multiplier = mul };

  public double Percent
  {
    get => (_db == -100) ? 0 : Math.Pow(10, _db / 20 + 2);
    init => _db = (value <= 0) ? -100
      : Math.Clamp(20 * (Math.Log10(value) - 2), -100, 26);
  }
  public static VolumeLevel FromPercent(double pct) => new VolumeLevel { Percent = pct };
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

public class PropertyItem
{
  public required bool Enabled { get; init; }
  public required string Name { get; init; }
  public required JsonNode? Value { get; init; }

  public PropertyItem() { }

  [SetsRequiredMembers]
  public PropertyItem(JsonNode n)
  {
    Enabled = (bool)n["itemEnabled"]!;
    Name = (string)n["itemName"]!;
    Value = n["itemValue"];
  }
}