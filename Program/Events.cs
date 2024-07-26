using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Nodes;
using Websocket.Client;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  public readonly OBSEvents Events;
}

public partial class OBSEvents
{
  internal OBSClient Client;

  internal OBSEvents(OBSClient client)
  {
    Client = client;

    // General = new(client);
    // Config = new(client);
    // Scenes = new(client);
    // Inputs = new(client);
    // Transitions = new(client);
    // Filters = new(client);
    // SceneItems = new(client);
    Outputs = new(client);
    // MediaInputs = new(client);
    // UI = new(client);
    // HighVolume = new(client);
  }

  internal async Task Handle(JsonObject data)
  {
    await Task.Delay(0);

    string eventName = (string)data["eventType"]!;
    JsonObject eventData = (JsonObject)data["eventData"]!;

    switch (eventName)
    {
      // case "ExitStarted":
      // case "VendorEvent":
      // case "CustomEvent":
      //   General.Handle(eventName, eventData);
      //   break;
      // case "CurrentSceneCollectionChanging":
      // case "CurrentSceneCollectionChanged":
      // case "SceneCollectionListChanged":
      // case "CurrentProfileChanging":
      // case "CurrentProfileChanged":
      // case "ProfileListChanged":
      //   Config.Handle(eventName, eventData);
      //   break;
      // case "SceneCreated":
      // case "SceneRemoved":
      // case "SceneNameChanged":
      // case "CurrentProgramSceneChanged":
      // case "CurrentPreviewSceneChanged":
      // case "SceneListChanged":
      //   Scenes.Handle(eventName, eventData);
      //   break;
      // case "InputCreated":
      // case "InputRemoved":
      // case "InputNameChanged":
      // case "InputSettingsChanged":
      // case "InputMuteStateChanged":
      // case "InputVolumeChanged":
      // case "InputAudioBalanceChanged":
      // case "InputAudioSyncOffsetChanged":
      // case "InputAudioTracksChanged":
      // case "InputAudioMonitorTypeChanged":
      //   Inputs.Handle(eventName, eventData);
      //   break;
      // case "CurrentSceneTransitionChanged":
      // case "CurrentSceneTransitionDurationChanged":
      // case "SceneTransitionStarted":
      // case "SceneTransitionEnded":
      // case "SceneTransitionVideoEnded":
      //   Transitions.Handle(eventName, eventData);
      //   break;
      // case "SourceFilterListReindexed":
      // case "SourceFilterCreated":
      // case "SourceFilterRemoved":
      // case "SourceFilterNameChanged":
      // case "SourceFilterSettingsChanged":
      // case "SourceFilterEnableStateChanged":
      //   Filters.Handle(eventName, eventData);
      //   break;
      // case "SceneItemCreated":
      // case "SceneItemRemoved":
      // case "SceneItemListReindexed":
      // case "SceneItemEnableStateChanged":
      // case "SceneItemLockStateChanged":
      // case "SceneItemSelected":
      //   SceneItems.Handle(eventName, eventData);
      //   break;
      case "StreamStateChanged":
      // case "RecordStateChanged":
      // case "RecordFileChanged":
      // case "ReplayBufferStateChanged":
      case "VirtualcamStateChanged":
        // case "ReplayBufferSaved":
        Outputs.Handle(eventName, eventData);
        break;
      // case "MediaInputPlaybackStarted":
      // case "MediaInputPlaybackEnded":
      // case "MediaInputActionTriggered":
      //   MediaInputs.Handle(eventName, eventData);
      //   break;
      // case "StudioModeStateChanged":
      // case "ScreenshotSaved":
      //   UI.Handle(eventName, eventData);
      //   break;
      // case "InputActiveStateChanged":
      // case "InputShowStateChanged":
      // case "InputVolumeMeters":
      // case "SceneItemTransformChanged":
      //   HighVolume.Handle(eventName, eventData);
      //   break;
      default:
        UnknownEvent?.Invoke(this, data);
        break;
    }
  }

  public event EventHandler<JsonObject>? UnknownEvent;
}

public class OBSEventArgs : EventArgs
{
  public required JsonObject EventJsonData { get; init; }

  public OBSEventArgs() { }

  [SetsRequiredMembers]
  public OBSEventArgs(JsonObject data)
  {
    EventJsonData = data;
  }

  [return: NotNull]
  protected JsonNode GetRequiredNode(string node)
  {
    return EventJsonData![node] ?? throw new MissingFieldException(node);
  }
}