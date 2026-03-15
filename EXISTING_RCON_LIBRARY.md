# Existing Rcon.WebRcon.Rust Library - Feature Summary

## 🎯 What You Already Have (Work in Progress)

Your workspace contains a **WebSocket RCON client library for Rust** that provides a solid foundation for building your WebRCON system. It's a work in progress, but it handles the complex WebSocket connection management and basic message parsing, which is a huge time-saver!

**Important:** This library is **not feature-complete** and will need to be extended for advanced features like inventory management, teleportation, and other custom commands you want to add.

---

## Project Structure

```
Rcon/                          - Base RCON abstractions
├── RconClient.cs             - Abstract base class
├── RconMessage.cs            - Message base
├── GameType.cs               - Enum for game types
└── Sequence.cs               - Request ID sequencing

Rcon.WebRcon/                  - WebSocket RCON implementation
├── WebRconClient.cs          - WebSocket-based RCON client
├── ConnectionChangedEventArgs.cs
├── MessageReceivedEventArgs.cs
└── Messages/
    ├── WebRconMessageBase.cs
    ├── WebRconRequest.cs
    └── WebRconResponse.cs

Rcon.WebRcon.Rust/            - Rust-specific implementation
├── RustWebRconClient.cs      - Main client class
├── Entities/                  - Data models
│   ├── Player.cs             - Player information
│   ├── ServerInfo.cs         - Server details
│   ├── ConsoleMessage.cs     - Console output
│   ├── ChatMessage.cs        - Chat messages
│   ├── Ban.cs                - Ban information
│   ├── OxidePlugin.cs        - Oxide plugins
│   ├── CarbonPlugin.cs       - Carbon plugins
│   └── ModFrameworkVersion.cs
├── EventArgs/                 - Event arguments for all events
├── Parsers/                   - Message parsers for each type
└── Containers/                - Collection classes
```

---

## Key Features Already Implemented

### ✅ Connection Management
- **WebSocket persistent connection** using `Websocket.Client` library
- Automatic connection handling
- Disconnect detection
- Connection URL format: `ws://{hostname}:{port}/{password}`

### ✅ Message Handling
- **Event-driven architecture** with typed events
- Automatic message parsing and routing
- Request/response correlation
- Message logging

### ✅ Events Available
```csharp
public event EventHandler<BanEventArgs>? BansReceived;
public event EventHandler<ChatEventArgs>? ChatReceived;
public event EventHandler<ConsoleEventArgs>? ConsoleReceived;
public event EventHandler<OxidePluginsEventArgs>? OxidePluginsReceived;
public event EventHandler<CarbonPluginsEventArgs>? CarbonPluginsReceived;
public event EventHandler<PlayerListEventArgs>? PlayerListReceived;
public event EventHandler<ServerInfoEventArgs>? ServerInfoReceived;
public event EventHandler<UnprocessedEventArgs>? UnprocessedMessageReceived;
public event EventHandler<ModFrameworkVersionEventArgs>? ModFrameworkVersionReceived;
```

### ✅ Entity Models

#### Player Entity
```csharp
public class Player : EntityBase
{
    public string SteamId { get; set; }
    public string OwnerSteamId { get; set; }
    public string DisplayName { get; set; }
    public int Ping { get; set; }
    public string Address { get; set; }
    public int ConnectedSeconds { get; set; }
    public decimal ViolationLevel { get; set; }
    public decimal CurrentLevel { get; set; }
    public decimal UnspentXp { get; set; }
    public decimal Health { get; set; }
}
```

#### Other Entities
- **ServerInfo** - Server name, map, players, max players, queued, joining, etc.
- **ConsoleMessage** - Type, message, timestamp, stacktrace
- **ChatMessage** - Channel, message, user ID, username, timestamp, color
- **Ban** - Steam ID, username, reason
- **OxidePlugin** - Name, author, version, title, description
- **CarbonPlugin** - Similar to Oxide

---

## How to Use It

### Basic Connection Example
```csharp
// Create client
var client = new RustWebRconClient(
    name: "My Server",
    hostname: "127.0.0.1",
    port: 28016,
    password: "your-password",
    gameType: GameType.Rust
);

// Subscribe to events
client.PlayerListReceived += (sender, args) => 
{
    foreach (var player in args.Players)
    {
        Console.WriteLine($"{player.DisplayName} - {player.SteamId}");
    }
};

client.ConsoleReceived += (sender, args) =>
{
    foreach (var message in args.ConsoleMessages)
    {
        Console.WriteLine($"[{message.Type}] {message.Message}");
    }
};

// Connect
client.Connect();

// Send command
var request = new WebRconRequest
{
    Identifier = client._sequence.Next(),
    Message = "status",
    Type = "command"
};
client.SendRequest(request);

// Disconnect when done
client.Disconnect();
```

---

## What You Need to Build

### ✅ Already Have (Foundation - Don't Need to Build from Scratch)
- ✅ WebSocket connection management infrastructure
- ✅ Basic RCON protocol handling
- ✅ Message serialization/deserialization framework
- ✅ Core entities (Player, Console, Chat, Bans, Plugins)
- ✅ Event handling architecture
- ✅ Parser infrastructure

### ⚠️ May Need to Extend in Rcon.WebRcon.Rust
- ⚠️ **Inventory management** - Add parser + entities for inventory commands (if RCON supports it)
- ⚠️ **Teleportation** - Add parser + entities for teleport commands (likely plugin-dependent)
- ⚠️ **Advanced statistics** - Enhance server statistics parsing for detailed metrics
- ⚠️ **Custom RCON commands** - Add parsers for any additional commands you need
- ⚠️ **Plugin-specific commands** - Add support for Oxide/Carbon/other plugin commands

### ✏️ Need to Build in Your Application (Your Main Work)
1. **Service Layer** - Wrap RustWebRconClient with your own service
2. **Connection Pool** - Manage multiple server connections
3. **Database Logging** - Persist RCON requests/responses to your database
4. **Reconnection Logic** - Use Polly for retry policies
5. **Background Service** - HostedService that polls servers using your client
6. **API Endpoints** - Expose RCON functionality via REST API
7. **Blazor UI** - Display data from RCON client in your UI
8. **User Management** - Authentication, authorization, multi-user support
9. **Audit Trail** - Track user actions and RCON operations

---

## Integration Strategy

### Phase 1: Service Wrapper
Create a service that wraps RustWebRconClient:
```csharp
public class RconService
{
    private Dictionary<int, RustWebRconClient> _clients = new();
    private readonly IDbContext _db;

    public async Task ConnectToServer(int serverId)
    {
        var server = await _db.Servers.FindAsync(serverId);
        var client = new RustWebRconClient(
            server.Name,
            server.Hostname,
            server.Port,
            DecryptPassword(server.EncryptedPassword),
            GameType.Rust
        );

        client.ConsoleReceived += async (s, e) => 
        {
            await LogConsoleMessages(serverId, e.ConsoleMessages);
        };

        client.PlayerListReceived += async (s, e) =>
        {
            await UpdatePlayerList(serverId, e.Players);
        };

        client.Connect();
        _clients[serverId] = client;
    }

    // More methods...
}
```

### Phase 2: Background Worker
```csharp
public class RconPollingService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var server in await GetActiveServers())
            {
                await _rconService.PollServer(server.Id);
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
```

---

## Dependencies

The Rcon.WebRcon project uses:
- **Websocket.Client v4.4.43** - Excellent reactive WebSocket library
- **System.Text.Json** - Built-in JSON serialization

Already targeting **.NET 10**! 🎉

---

## Estimated Time Saved

By having this library as a foundation:
- **WebSocket connection infrastructure**: 1 week saved
- **Basic message parsing framework**: 3-4 days saved
- **Event architecture and routing**: 3-4 days saved
- **Core entity models**: 2-3 days saved

**Total: 2-3 weeks saved on foundation/boilerplate!** 🚀

**What you still need to invest time in:**
- Extending the library for missing features (inventory, teleport, etc.) - 1-2 weeks
- Building your application layer (services, API, UI, auth) - 6-8 weeks
- Testing and polish - 2-3 weeks

**Realistic total timeline: Still 2-3 months, but with a solid foundation to build on!**

---

## Next Steps

1. ✅ Update PLAN.md to reference this library (DONE)
2. ✅ Set realistic expectations about what needs extending (DONE)
3. Add project reference from RustWebConsole.Web to Rcon.WebRcon.Rust
4. **Research what RCON commands are available** via Rust server (vanilla + plugins)
5. Create RconService wrapper class
6. Implement database logging for requests/responses
7. **Extend Rcon.WebRcon.Rust as you discover missing features**
8. Create background polling service
9. Build API endpoints
10. Build Blazor UI

## Testing & Discovery Process

**Recommended Approach:**
1. Connect to a test Rust server with your existing library
2. Send common RCON commands and observe responses
3. Document command formats and response structures
4. Identify gaps (inventory, teleport, etc.)
5. Add parsers and entities incrementally as needed
6. Build your application layer in parallel

**You're starting with a solid foundation - about 20-25% of the RCON work done!** 🎉

The good news: The hard part (WebSocket connections, message routing) is done. The rest is implementing specific commands and parsers as you need them.
