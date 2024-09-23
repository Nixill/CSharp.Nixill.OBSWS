This is a library to create a websocket client to connect to OBS Studio.

# Connecting to OBS
To establish a connection to an OBS websocket server, create an `OBSClient` (`Nixill.OBSWS`) instance and then await `ConnectAndIdentifyAsync` on it. For example:

```cs
string[] serverDetails = File.ReadAllLines("data/obs.txt");
string serverIP = serverDetails[0];
int serverPort = int.Parse(serverDetails[1]);
string serverPassword = serverDetails[2];

OBSClient client = new(serverIP, serverPort, serverPassword);

await client.ConnectAndIdentifyAsync();
```

It is also possible to connect without waiting for identification using `ConnectAsync`, or to *only* wait for identification using `WaitForIdentify`.

The library will handle responding to the server's hello with identification automatically at this stage (opcodes 0, 1, and 2).

# Making and sending requests
To create a request that will be sent to OBS, use a member of the `OBSRequests` class. For example:

```cs
var getActiveScene = OBSRequests.Scenes.GetCurrentProgramScene();
```

... will change the "text" setting of the "HelloWorldText" input to say "Hello world!".

Note that this does not actually send the request! So far, you have only initialized it. To actually send it and get results, you will need to call the `SendRequest` method on the client. For example:

```cs
var currentScene = await client.SendRequest(getActiveScene);
```
