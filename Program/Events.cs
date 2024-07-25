using System.Reflection;
using System.Text.Json.Nodes;
using Websocket.Client;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  void HandleEvent(JsonObject data)
  {
    string eventName = (string)data["eventType"]!;
    JsonObject eventData = (JsonObject)data["eventData"]!;

    switch (eventName)
    {
      case "ExitStarted":
        ExitStartedJson?.Invoke(this, eventData);
        break;
      case "VendorEvent":
        VendorEventJson?.Invoke(this, eventData);
        break;
      case "CustomEvent":
        CustomEventJson?.Invoke(this, eventData);
        break;
      case "CurrentSceneCollectionChanging":
        CurrentSceneCollectionChangingJson?.Invoke(this, eventData);
        break;
      case "CurrentSceneCollectionChanged":
        CurrentSceneCollectionChangedJson?.Invoke(this, eventData);
        break;
      case "SceneCollectionListChanged":
        SceneCollectionListChangedJson?.Invoke(this, eventData);
        break;
      case "CurrentProfileChanging":
        CurrentProfileChangingJson?.Invoke(this, eventData);
        break;
      case "CurrentProfileChanged":
        CurrentProfileChangedJson?.Invoke(this, eventData);
        break;
      case "ProfileListChanged":
        ProfileListChangedJson?.Invoke(this, eventData);
        break;
      case "SceneCreated":
        SceneCreatedJson?.Invoke(this, eventData);
        break;
      case "SceneRemoved":
        SceneRemovedJson?.Invoke(this, eventData);
        break;
      case "SceneNameChanged":
        SceneNameChangedJson?.Invoke(this, eventData);
        break;
      case "CurrentProgramSceneChanged":
        CurrentProgramSceneChangedJson?.Invoke(this, eventData);
        break;
      case "CurrentPreviewSceneChanged":
        CurrentPreviewSceneChangedJson?.Invoke(this, eventData);
        break;
      case "SceneListChanged":
        SceneListChangedJson?.Invoke(this, eventData);
        break;
      case "InputCreated":
        InputCreatedJson?.Invoke(this, eventData);
        break;
      case "InputRemoved":
        InputRemovedJson?.Invoke(this, eventData);
        break;
      case "InputNameChanged":
        InputNameChangedJson?.Invoke(this, eventData);
        break;
      case "InputSettingsChanged":
        InputSettingsChangedJson?.Invoke(this, eventData);
        break;
      case "InputActiveStateChanged":
        InputActiveStateChangedJson?.Invoke(this, eventData);
        break;
      case "InputShowStateChanged":
        InputShowStateChangedJson?.Invoke(this, eventData);
        break;
      case "InputMuteStateChanged":
        InputMuteStateChangedJson?.Invoke(this, eventData);
        break;
      case "InputVolumeChanged":
        InputVolumeChangedJson?.Invoke(this, eventData);
        break;
      case "InputAudioBalanceChanged":
        InputAudioBalanceChangedJson?.Invoke(this, eventData);
        break;
      case "InputAudioSyncOffsetChanged":
        InputAudioSyncOffsetChangedJson?.Invoke(this, eventData);
        break;
      case "InputAudioTracksChanged":
        InputAudioTracksChangedJson?.Invoke(this, eventData);
        break;
      case "InputAudioMonitorTypeChanged":
        InputAudioMonitorTypeChangedJson?.Invoke(this, eventData);
        break;
      case "InputVolumeMeters":
        InputVolumeMetersJson?.Invoke(this, eventData);
        break;
      case "CurrentSceneTransitionChanged":
        CurrentSceneTransitionChangedJson?.Invoke(this, eventData);
        break;
      case "CurrentSceneTransitionDurationChanged":
        CurrentSceneTransitionDurationChangedJson?.Invoke(this, eventData);
        break;
      case "SceneTransitionStarted":
        SceneTransitionStartedJson?.Invoke(this, eventData);
        break;
      case "SceneTransitionEnded":
        SceneTransitionEndedJson?.Invoke(this, eventData);
        break;
      case "SceneTransitionVideoEnded":
        SceneTransitionVideoEndedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterListReindexed":
        SourceFilterListReindexedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterCreated":
        SourceFilterCreatedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterRemoved":
        SourceFilterRemovedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterNameChanged":
        SourceFilterNameChangedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterSettingsChanged":
        SourceFilterSettingsChangedJson?.Invoke(this, eventData);
        break;
      case "SourceFilterEnableStateChanged":
        SourceFilterEnableStateChangedJson?.Invoke(this, eventData);
        break;
      case "SceneItemCreated":
        SceneItemCreatedJson?.Invoke(this, eventData);
        break;
      case "SceneItemRemoved":
        SceneItemRemovedJson?.Invoke(this, eventData);
        break;
      case "SceneItemListReindexed":
        SceneItemListReindexedJson?.Invoke(this, eventData);
        break;
      case "SceneItemEnableStateChanged":
        SceneItemEnableStateChangedJson?.Invoke(this, eventData);
        break;
      case "SceneItemLockStateChanged":
        SceneItemLockStateChangedJson?.Invoke(this, eventData);
        break;
      case "SceneItemSelected":
        SceneItemSelectedJson?.Invoke(this, eventData);
        break;
      case "SceneItemTransformChanged":
        SceneItemTransformChangedJson?.Invoke(this, eventData);
        break;
      case "StreamStateChanged":
        StreamStateChangedJson?.Invoke(this, eventData);
        break;
      case "RecordStateChanged":
        RecordStateChangedJson?.Invoke(this, eventData);
        break;
      case "RecordFileChanged":
        RecordFileChangedJson?.Invoke(this, eventData);
        break;
      case "ReplayBufferStateChanged":
        ReplayBufferStateChangedJson?.Invoke(this, eventData);
        break;
      case "VirtualcamStateChanged":
        VirtualcamStateChangedJson?.Invoke(this, eventData);
        break;
      case "ReplayBufferSaved":
        ReplayBufferSavedJson?.Invoke(this, eventData);
        break;
      case "MediaInputPlaybackStarted":
        MediaInputPlaybackStartedJson?.Invoke(this, eventData);
        break;
      case "MediaInputPlaybackEnded":
        MediaInputPlaybackEndedJson?.Invoke(this, eventData);
        break;
      case "MediaInputActionTriggered":
        MediaInputActionTriggeredJson?.Invoke(this, eventData);
        break;
      case "StudioModeStateChanged":
        StudioModeStateChangedJson?.Invoke(this, eventData);
        break;
      case "ScreenshotSaved":
        ScreenshotSavedJson?.Invoke(this, eventData);
        break;
      default:
        UnknownEvent?.Invoke(this, data);
        break;
    }
  }

  public event EventHandler<JsonObject>? ExitStartedJson;
  public event EventHandler<JsonObject>? VendorEventJson;
  public event EventHandler<JsonObject>? CustomEventJson;
  public event EventHandler<JsonObject>? CurrentSceneCollectionChangingJson;
  public event EventHandler<JsonObject>? CurrentSceneCollectionChangedJson;
  public event EventHandler<JsonObject>? SceneCollectionListChangedJson;
  public event EventHandler<JsonObject>? CurrentProfileChangingJson;
  public event EventHandler<JsonObject>? CurrentProfileChangedJson;
  public event EventHandler<JsonObject>? ProfileListChangedJson;
  public event EventHandler<JsonObject>? SceneCreatedJson;
  public event EventHandler<JsonObject>? SceneRemovedJson;
  public event EventHandler<JsonObject>? SceneNameChangedJson;
  public event EventHandler<JsonObject>? CurrentProgramSceneChangedJson;
  public event EventHandler<JsonObject>? CurrentPreviewSceneChangedJson;
  public event EventHandler<JsonObject>? SceneListChangedJson;
  public event EventHandler<JsonObject>? InputCreatedJson;
  public event EventHandler<JsonObject>? InputRemovedJson;
  public event EventHandler<JsonObject>? InputNameChangedJson;
  public event EventHandler<JsonObject>? InputSettingsChangedJson;
  public event EventHandler<JsonObject>? InputActiveStateChangedJson;
  public event EventHandler<JsonObject>? InputShowStateChangedJson;
  public event EventHandler<JsonObject>? InputMuteStateChangedJson;
  public event EventHandler<JsonObject>? InputVolumeChangedJson;
  public event EventHandler<JsonObject>? InputAudioBalanceChangedJson;
  public event EventHandler<JsonObject>? InputAudioSyncOffsetChangedJson;
  public event EventHandler<JsonObject>? InputAudioTracksChangedJson;
  public event EventHandler<JsonObject>? InputAudioMonitorTypeChangedJson;
  public event EventHandler<JsonObject>? InputVolumeMetersJson;
  public event EventHandler<JsonObject>? CurrentSceneTransitionChangedJson;
  public event EventHandler<JsonObject>? CurrentSceneTransitionDurationChangedJson;
  public event EventHandler<JsonObject>? SceneTransitionStartedJson;
  public event EventHandler<JsonObject>? SceneTransitionEndedJson;
  public event EventHandler<JsonObject>? SceneTransitionVideoEndedJson;
  public event EventHandler<JsonObject>? SourceFilterListReindexedJson;
  public event EventHandler<JsonObject>? SourceFilterCreatedJson;
  public event EventHandler<JsonObject>? SourceFilterRemovedJson;
  public event EventHandler<JsonObject>? SourceFilterNameChangedJson;
  public event EventHandler<JsonObject>? SourceFilterSettingsChangedJson;
  public event EventHandler<JsonObject>? SourceFilterEnableStateChangedJson;
  public event EventHandler<JsonObject>? SceneItemCreatedJson;
  public event EventHandler<JsonObject>? SceneItemRemovedJson;
  public event EventHandler<JsonObject>? SceneItemListReindexedJson;
  public event EventHandler<JsonObject>? SceneItemEnableStateChangedJson;
  public event EventHandler<JsonObject>? SceneItemLockStateChangedJson;
  public event EventHandler<JsonObject>? SceneItemSelectedJson;
  public event EventHandler<JsonObject>? SceneItemTransformChangedJson;
  public event EventHandler<JsonObject>? StreamStateChangedJson;
  public event EventHandler<JsonObject>? RecordStateChangedJson;
  public event EventHandler<JsonObject>? RecordFileChangedJson;
  public event EventHandler<JsonObject>? ReplayBufferStateChangedJson;
  public event EventHandler<JsonObject>? VirtualcamStateChangedJson;
  public event EventHandler<JsonObject>? ReplayBufferSavedJson;
  public event EventHandler<JsonObject>? MediaInputPlaybackStartedJson;
  public event EventHandler<JsonObject>? MediaInputPlaybackEndedJson;
  public event EventHandler<JsonObject>? MediaInputActionTriggeredJson;
  public event EventHandler<JsonObject>? StudioModeStateChangedJson;
  public event EventHandler<JsonObject>? ScreenshotSavedJson;
  public event EventHandler<JsonObject>? UnknownEvent;
}