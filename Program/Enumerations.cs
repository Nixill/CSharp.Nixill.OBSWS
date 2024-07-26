public enum OpCode
{
  Hello = 0,
  Identify = 1,
  Identified = 2,
  Reidentify = 3,
  // there is no 4,
  Event = 5,
  Request = 6,
  RequestResponse = 7,
  RequestBatch = 8,
  RequestBatchResponse = 9
}

public static class OpCodeExtensions
{
  public static bool IsIncoming(this OpCode code) => code switch
  {
    OpCode.Hello or OpCode.Identified or OpCode.Event or OpCode.RequestResponse or OpCode.RequestBatchResponse => true,
    _ => false
  };

  public static bool IsOutgoing(this OpCode code) => code switch
  {
    OpCode.Identify or OpCode.Reidentify or OpCode.Request or OpCode.RequestBatch => true,
    _ => false
  };
}

public enum WebSocketCloseCode
{
  DontClose = 0,
  UnknownReason = 4000,
  MessageDecodeError = 4002,
  MissingDataField = 4003,
  InvalidDataFieldType = 4004,
  InvalidDataFieldValue = 4005,
  UnknownOpCode = 4006,
  NotIdentified = 4007,
  AlreadyIdentified = 4008,
  AuthenticationFailed = 4009,
  UnsupportedRpcVersion = 4010,
  SessionInvalidated = 4011,
  UnsupportedFeature = 4012
}

public enum RequestBatchExecutionType
{
  None = -1,
  SerialRealtime = 0,
  SerialFrame = 1,
  Parallel = 2
}

public enum RequestStatus
{
  Unknown = 0,
  NoError = 10,
  Success = 100,
  MissingRequestType = 203,
  UnknownRequestType = 204,
  GenericError = 205,
  UnsupportedRequestBatchExecutionType = 206,
  NotReady = 207,
  MissingRequestField = 300,
  MissingRequestData = 301,
  InvalidRequestField = 400,
  InvalidRequestFieldType = 401,
  RequestFieldOutOfRange = 402,
  RequestFieldEmpty = 403,
  TooManyRequestFields = 404,
  OutputRunning = 500,
  OutputNotRunning = 501,
  OutputPaused = 502,
  OutputNotPaused = 503,
  OutputDisabled = 504,
  StudioModeActive = 505,
  StudioModeNotActive = 506,
  ResourceNotFound = 600,
  ResourceAlreadyExists = 601,
  InvalidResourceType = 602,
  NotEnoughResources = 603,
  InvalidResourceState = 604,
  InvalidInputKind = 605,
  ResourceNotConfigurable = 606,
  InvalidFilterKind = 607,
  ResourceCreationFailed = 700,
  ResourceActionFailed = 701,
  RequestProcessingFailed = 702,
  CannotAct = 703
}

[Flags]
public enum EventSubscription
{
  None = 0,
  General = 1 << 0,
  Config = 1 << 1,
  Scenes = 1 << 2,
  Inputs = 1 << 3,
  Transitions = 1 << 4,
  Filters = 1 << 5,
  Outputs = 1 << 6,
  SceneItems = 1 << 7,
  MediaInputs = 1 << 8,
  Vendors = 1 << 9,
  Ui = 1 << 10,
  All = 0b111_1111_1111,
  InputVolumeMeters = 1 << 16,
  InputActiveStateChanged = 1 << 17,
  InputShowStateChanged = 1 << 18,
  SceneItemTransformChanged = 1 << 19
}

[Flags]
public enum SubscriptionRequired
{
  ExitStarted = EventSubscription.General,
  VendorEvent = EventSubscription.General,
  CustomEvent = EventSubscription.General,
  CurrentSceneCollectionChanging = EventSubscription.Config,
  CurrentSceneCollectionChanged = EventSubscription.Config,
  SceneCollectionListChanged = EventSubscription.Config,
  CurrentProfileChanging = EventSubscription.Config,
  CurrentProfileChanged = EventSubscription.Config,
  ProfileListChanged = EventSubscription.Config,
  SceneCreated = EventSubscription.Scenes,
  SceneRemoved = EventSubscription.Scenes,
  SceneNameChanged = EventSubscription.Scenes,
  CurrentProgramSceneChanged = EventSubscription.Scenes,
  CurrentPreviewSceneChanged = EventSubscription.Scenes,
  SceneListChanged = EventSubscription.Scenes,
  InputCreated = EventSubscription.Inputs,
  InputRemoved = EventSubscription.Inputs,
  InputNameChanged = EventSubscription.Inputs,
  InputSettingsChanged = EventSubscription.Inputs,
  InputActiveStateChanged = EventSubscription.InputActiveStateChanged,
  InputShowStateChanged = EventSubscription.InputShowStateChanged,
  InputMuteStateChanged = EventSubscription.Inputs,
  InputVolumeChanged = EventSubscription.Inputs,
  InputAudioBalanceChanged = EventSubscription.Inputs,
  InputAudioSyncOffsetChanged = EventSubscription.Inputs,
  InputAudioTracksChanged = EventSubscription.Inputs,
  InputAudioMonitorTypeChanged = EventSubscription.Inputs,
  InputVolumeMeters = EventSubscription.InputVolumeMeters,
  CurrentSceneTransitionChanged = EventSubscription.Transitions,
  CurrentSceneTransitionDurationChanged = EventSubscription.Transitions,
  SceneTransitionStarted = EventSubscription.Transitions,
  SceneTransitionEnded = EventSubscription.Transitions,
  SceneTransitionVideoEnded = EventSubscription.Transitions,
  SourceFilterListReindexed = EventSubscription.Filters,
  SourceFilterCreated = EventSubscription.Filters,
  SourceFilterRemoved = EventSubscription.Filters,
  SourceFilterNameChanged = EventSubscription.Filters,
  SourceFilterSettingsChanged = EventSubscription.Filters,
  SourceFilterEnableStateChanged = EventSubscription.Filters,
  SceneItemCreated = EventSubscription.SceneItems,
  SceneItemRemoved = EventSubscription.SceneItems,
  SceneItemListReindexed = EventSubscription.SceneItems,
  SceneItemEnableStateChanged = EventSubscription.SceneItems,
  SceneItemLockStateChanged = EventSubscription.SceneItems,
  SceneItemSelected = EventSubscription.SceneItems,
  SceneItemTransformChanged = EventSubscription.SceneItemTransformChanged,
  StreamStateChanged = EventSubscription.Outputs,
  RecordStateChanged = EventSubscription.Outputs,
  RecordFileChanged = EventSubscription.Outputs,
  ReplayBufferStateChanged = EventSubscription.Outputs,
  VirtualcamStateChanged = EventSubscription.Outputs,
  ReplayBufferSaved = EventSubscription.Outputs,
  MediaInputPlaybackStarted = EventSubscription.MediaInputs,
  MediaInputPlaybackEnded = EventSubscription.MediaInputs,
  MediaInputActionTriggered = EventSubscription.MediaInputs,
  StudioModeStateChanged = EventSubscription.Ui,
  ScreenshotSaved = EventSubscription.Ui
}

public enum OBSMediaInputAction
{
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE")]
  None,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY")]
  Play,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE")]
  Pause,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP")]
  Stop,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_RESTART")]
  Restart,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NEXT")]
  Next,
  [StringValue("OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PREVIOUS")]
  Previous
}

public static class OBSMediaInputActions
{
  public static string GetIdentifierValue(this OBSMediaInputAction action)
    => StringValueAttribute.GetValue(action);

  public static OBSMediaInputAction ForIdentifierValue(string value)
    => StringValueAttribute.FindValue<OBSMediaInputAction>(value);
}

public enum OBSOutputState
{
  [StringValue("OBS_WEBSOCKET_OUTPUT_UNKNOWN")]
  Unknown,
  [StringValue("OBS_WEBSOCKET_OUTPUT_STARTING")]
  Starting,
  [StringValue("OBS_WEBSOCKET_OUTPUT_STARTED")]
  Started,
  [StringValue("OBS_WEBSOCKET_OUTPUT_STOPPING")]
  Stopping,
  [StringValue("OBS_WEBSOCKET_OUTPUT_STOPPED")]
  Stopped,
  [StringValue("OBS_WEBSOCKET_OUTPUT_RECONNECTING")]
  Reconnecting,
  [StringValue("OBS_WEBSOCKET_OUTPUT_RECONNECTED")]
  Reconnected,
  [StringValue("OBS_WEBSOCKET_OUTPUT_PAUSED")]
  Paused,
  [StringValue("OBS_WEBSOCKET_OUTPUT_RESUMED")]
  Resumed
}

public static class OBSOutputStates
{
  public static string GetIdentifierValue(this OBSOutputState action)
    => StringValueAttribute.GetValue(action);

  public static OBSOutputState ForIdentifierValue(string value)
    => StringValueAttribute.FindValue<OBSOutputState>(value);
}

public class StringValueAttribute : Attribute
{
  public string Value { get; init; }

  public StringValueAttribute(string val) => Value = val;

  public static string GetValue<T>(T input) where T : System.Enum
  {
    try
    {
      return (typeof(T)
        .GetField(input.ToString()) ?? throw new InvalidCastException($"No enum constant exists for ({typeof(T).Name}){input.ToString()}"))
        .GetCustomAttributes(typeof(StringValueAttribute), false)
        .Cast<StringValueAttribute>()
        .Single()
        .Value;
    }
    catch (InvalidOperationException ex)
    {
      if (ex.Message == "Sequence contains no elements") throw new InvalidOperationException($"No StringValueAttribute exists on {typeof(T).Name}.{input.ToString()}");
      else throw;
    }
  }

  public static T FindValue<T>(string input) where T : System.Enum
  {
    try
    {
      return (T)(typeof(T)
        .GetFields()
        .Where(f => f
          .GetCustomAttributes(typeof(StringValueAttribute), false)
          .Cast<StringValueAttribute>()
          .Where(v => v.Value == input)
          .Any()
        ).Single()
        .GetValue(null))!;
    }
    catch (InvalidOperationException ex)
    {
      if (ex.Message == "Sequence contains no elements") throw new InvalidOperationException($"No StringValueAttribute with a value of {input} exists on a(n) {typeof(T).Name} constant.");
      else if (ex.Message == "Sequence contains more than one element") throw new InvalidOperationException($"Multiple {typeof(T).Name} constants have a StringValueAttribute with a value of {input}.");
      else throw;
    }
  }
}
