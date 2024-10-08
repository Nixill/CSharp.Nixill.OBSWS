﻿using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Nixill.OBSWS;

public partial class OBSClient
{
  internal WebsocketClient Client;
  internal ILogger? Logger;
  internal string Password;
  public EventSubscription EventSubscriptions { get; private set; }

  TaskCompletionSource<bool> IdentifyWaitTask = new TaskCompletionSource<bool>();

  public bool IsConnected { get; private set; } = false;
  public bool IsIdentified { get; private set; } = false;

  public OBSClient(string ip, int port = 4455, string password = "", EventSubscription subs = EventSubscription.All,
  ILoggerFactory? loggerFactory = null) : this(new Uri($"ws://{ip}:{port}/"), password, subs, loggerFactory) { }

  public OBSClient(Uri uri, string password = "", EventSubscription subs = EventSubscription.All,
  ILoggerFactory? loggerFactory = null)
  {
    Client = new WebsocketClient(uri);
    Client.ReconnectTimeout = TimeSpan.FromSeconds(15);
    Client.MessageReceived.Subscribe(msg => Task.Run(() => Dispatch(msg)));
    Client.DisconnectionHappened.Subscribe(InvokeDisconnect);

    Password = password;
    Logger = loggerFactory?.CreateLogger(typeof(OBSClient));
    EventSubscriptions = subs;

    Events = new(this);
  }

  public async Task ConnectAsync()
  {
    if (IdentifyWaitTask.Task.IsCompleted) IdentifyWaitTask = new();
    await Client.StartOrFail();
    IsConnected = true;
  }

  public async Task<bool> WaitForIdentify()
  {
    return await IdentifyWaitTask.Task;
  }

  public async Task<bool> ConnectAndIdentifyAsync()
  {
    await ConnectAsync();
    return await IdentifyWaitTask.Task;
  }

  public Task<bool> DisconnectAsync() => DisconnectAsync(WebSocketCloseStatus.Empty, "");
  public async Task<bool> DisconnectAsync(WebSocketCloseStatus code, string description)
  {
    bool success = await Client.StopOrFail(code, description);
    IdentifyWaitTask.TrySetException(new OBSDisconnectedException());
    IsConnected = false;
    IsIdentified = false;
    return success;
  }

  public Task Reidentify(EventSubscription newSubs)
  {
    EventSubscriptions = newSubs;

    JsonObject reidentify = new JsonObject
    {
      ["op"] = (int)OpCode.Reidentify,
      ["d"] = new JsonObject
      {
        ["eventSubscriptions"] = (int)newSubs
      }
    };

    Client.Send(reidentify.ToString());

    return Task.CompletedTask;
  }

  public event EventHandler<OBSDisconnectedArgs>? Disconnected;

  private void InvokeDisconnect(DisconnectionInfo info)
  {
    OBSDisconnectedArgs args = new(info);
    Logger?.LogError($"Disconnected with code {args.Code} - {args.Comment ?? "(no comment)"}");

    Disconnected?.Invoke(this, new OBSDisconnectedArgs(info));
    IsConnected = false;
    IsIdentified = false;
  }

  private async Task Dispatch(ResponseMessage msg)
  {
    JsonObject response = (JsonObject)JsonNode.Parse(msg.Text!)!;
    OpCode opcode = (OpCode)(int)response["op"]!;
    JsonObject data = (JsonObject)response["d"]!;

    switch (opcode)
    {
      case OpCode.Hello:
        Logger?.LogInformation("Server said hello.");
        HandleHello(data); break;
      case OpCode.Identified:
        Logger?.LogInformation("Successfully identified.");
        IsIdentified = true;
        IdentifyWaitTask.SetResult(true);
        break;
      case OpCode.Event:
        await Events.Handle(data); break;
      case OpCode.RequestResponse:
        HandleResponse(data); break;
      case OpCode.RequestBatchResponse:
        HandleBatchResponse(data); break;
    }
  }

  void HandleHello(JsonObject data)
  {
    JsonObject identify = new JsonObject
    {
      ["op"] = (int)OpCode.Identify,
      ["d"] = new JsonObject
      {
        ["rpcVersion"] = 1,
        ["eventSubscriptions"] = (int)EventSubscriptions
      }
    };

    // Get authentication info
    if (data.ContainsKey("authentication"))
    {
      string password = Password;
      string salt = (string)data["authentication"]!["salt"]!;
      string salted_pass = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)));
      string challenge = (string)data["authentication"]!["challenge"]!;
      string authKey = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(salted_pass + challenge)));

      identify["d"]!["authentication"] = authKey;
    }

    Client.SendInstant(identify.ToString());

    Logger?.LogInformation("Sent identify.");
  }
}

public class OBSDisconnectedArgs : EventArgs
{
  public readonly WebSocketCloseCode Code;
  public readonly string? Comment;

  public OBSDisconnectedArgs(DisconnectionInfo info)
  {
    Code = (WebSocketCloseCode?)info.CloseStatus ?? WebSocketCloseCode.UnknownReason;
    Comment = info.CloseStatusDescription;
  }
}