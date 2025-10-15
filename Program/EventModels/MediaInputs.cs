using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public partial class OBSEvents
{
  public readonly OBSMediaInputEvents MediaInputs;

  public class OBSMediaInputEvents
  {
    internal OBSClient Client;

    internal OBSMediaInputEvents(OBSClient client) => Client = client;

    internal void Handle(string eventName, JsonObject eventData)
    {
      switch (eventName)
      {
        case "MediaInputPlaybackStarted":
          MediaInputPlaybackStarted?.Invoke(Client, new InputEventArgs(eventData));
          break;
        case "MediaInputPlaybackEnded":
          MediaInputPlaybackEnded?.Invoke(Client, new InputEventArgs(eventData));
          break;
        case "MediaInputActionTriggered":
          MediaInputActionTriggered?.Invoke(Client, new MediaInputActionEvent(eventData));
          break;
      }
    }

    public event EventHandler<InputEventArgs>? MediaInputPlaybackStarted;
    public event EventHandler<InputEventArgs>? MediaInputPlaybackEnded;
    public event EventHandler<MediaInputActionEvent>? MediaInputActionTriggered;
  }
}

public class MediaInputActionEvent : InputEventArgs
{
  public required OBSMediaInputAction MediaAction { get; init; }

  public MediaInputActionEvent() { }

  [SetsRequiredMembers]
  public MediaInputActionEvent(JsonObject obj) : base(obj)
  {
    MediaAction = OBSMediaInputActions.ForIdentifierValue((string)GetRequiredNode("mediaAction")!);
  }
}