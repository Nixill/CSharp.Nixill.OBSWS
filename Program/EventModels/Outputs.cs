using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public partial class OBSEvents
{
  public readonly OBSOutputEvents Outputs;

  public class OBSOutputEvents
  {
    internal OBSClient Client;

    internal OBSOutputEvents(OBSClient client) => Client = client;

    bool IsStreaming = false;
    // bool IsRecording = false;
    // bool IsReplayBuffering = false;
    bool IsVirtualCamming = false;

    internal void Handle(string eventName, JsonObject eventData)
    {
      OutputStateChanged osc;
      switch (eventName)
      {
        case "StreamStateChanged":
          osc = new OutputStateChanged(eventData);
          StreamStateChanged?.Invoke(Client, osc);
          if (osc.Active && !IsStreaming) StreamStarted?.Invoke(Client, osc);
          if (IsStreaming && !osc.Active) StreamStopped?.Invoke(Client, osc);
          IsStreaming = osc.Active;
          break;
        // case "RecordStateChanged":
        // case "RecordFileChanged":
        // case "ReplayBufferStateChanged":
        case "VirtualcamStateChanged":
          osc = new OutputStateChanged(eventData);
          VirtualcamStateChanged?.Invoke(Client, osc);
          if (osc.Active && !IsVirtualCamming) VirtualcamStarted?.Invoke(Client, osc);
          if (IsVirtualCamming && !osc.Active) VirtualcamStopped?.Invoke(Client, osc);
          IsVirtualCamming = osc.Active;
          break;
          // case "ReplayBufferSaved":
      }
    }

    public event EventHandler<OutputStateChanged>? StreamStateChanged;
    public event EventHandler<OutputStateChanged>? StreamStarted;
    public event EventHandler<OutputStateChanged>? StreamStopped;
    public event EventHandler<OutputStateChanged>? VirtualcamStateChanged;
    public event EventHandler<OutputStateChanged>? VirtualcamStarted;
    public event EventHandler<OutputStateChanged>? VirtualcamStopped;


  }
}

public class OutputStateChanged : OBSEventArgs
{
  public required bool Active { get; init; }
  public required OBSOutputState State { get; init; }
  public string? Path { get; init; }

  public OutputStateChanged() { }

  [SetsRequiredMembers]
  public OutputStateChanged(JsonObject obj) : base(obj)
  {
    Active = (bool)GetRequiredNode("outputActive");
    State = OBSOutputStates.ForIdentifierValue((string)GetRequiredNode("outputState")!);
    Path = (string?)obj["outputPath"];
  }
}