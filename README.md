# Xray.Sdk

Xray.Sdk is a .NET SDK that provides typed models, mappers and high-level interfaces to interact with Xray components programmatically. It is organized into several core modules to keep configuration, runtime control and API integration separated and easy to use.

## Key Features

- Strongly-typed configuration models and JSON helpers
- High-level API wrappers for managing Xray instances and performing operations
- Core utilities and process wrappers for running and interacting with Xray runtime

## Installation and build

Restore and build the whole solution from the repository root (Windows / PowerShell):

```powershell
dotnet restore Xray.Sdk.sln
dotnet build Xray.Sdk.sln -c Release
```

Build a single project (for example `Xray.Api`) from the repository root:

```powershell
dotnet build Xray.Api/Xray.Api.csproj -c Release
```

Add project references in your application using the `dotnet` CLI (example):

```powershell
dotnet add <YourProject>.csproj reference Xray.Config/Xray.Config.csproj
dotnet add <YourProject>.csproj reference Xray.Api/Xray.Api.csproj
dotnet add <YourProject>.csproj reference Xray.Core/Xray.Core.csproj
```

Run unit tests:

```powershell
dotnet test Xray.Sdk.Tests/Xray.Sdk.Tests.csproj -c Release
```

## Modules

The SDK is structured into three primary modules. The descriptions below are in English.

### Config

- Purpose: Configuration models and utilities for building, serializing and validating Xray configuration objects.
- Contents: typed model classes under `Xray.Config.Models`, enum types under `Xray.Config.Enums`, and JSON converters in `Xray.Config.Utilities`.
- Responsibilities:
  - Represent Xray configuration entities (inbounds, outbounds, routing, transports, metrics, etc.) as C# classes.
  - Provide helpers for loading/saving configuration files and for converting between SDK models and raw JSON used by the Xray binary.
  - Include domain-specific enums and value objects to avoid adhoc string usage.
- Typical usage: construct a `Config` model, adjust properties, then serialize to JSON and write to the Xray process or config file.

#### Xray.Config vs xray-core JSON configuration

- Representation: `Xray.Config` exposes strongly-typed C# classes that correspond to the conceptual structure of xray-core's JSON configuration (inbounds, outbounds, routing, transports, metrics, etc.). Each JSON object maps to a model class instead of raw dictionaries.

- Enums & fixed values: where xray-core expects string literals (for example protocol names, transport types, cipher names), the SDK provides enum types in `Xray.Config.Enums`. This improves discoverability and prevents typos compared to free-form JSON strings.

- Polymorphism and accounts: polymorphic fields in JSON (for example an inbound `accounts` entry that may carry different account payloads for VLESS/Trojan/Shadowsocks) are represented by model families and helper converters. The SDK encodes the concrete account payload as typed objects and serializes them into the shape xray-core expects.

- Defaults & validation: SDK models may expose default values and basic validation helpers so that common mistakes are caught earlier in code. When serialized, defaults are either omitted or emitted according to the serializer policy to match xray-core expectations.

- Serialization: use the SDK serializer utilities (see `Xray.Config.Utilities`) to convert SDK models to JSON compatible with xray-core. The serializer handles enum/string mapping, special field naming, and any custom converters required by xray's schema.

- Round-trip fidelity: the SDK aims for one-to-one round-trip with xray-core JSON for supported fields — you can build a `Xray.Config.Models.Config` instance, serialize it, and pass JSON to xray-core. Some advanced or unknown custom extensions in xray-core JSON may require manual handling via raw JSON containers.

- Example (conceptual):

JSON (xray-core):

```json
{
  "inbounds": [
    { "tag": "api", "protocol": "vless", "settings": { "clients": [{"id":"...","level":0}] } }
  ]
}
```

SDK equivalent:

```csharp
var config0 = new Xray.Config.Models.XrayConfig {
  Inbounds = new[]{
    new Xray.Config.Models.VlessInbound {
      Tag = "api",
      Settings = new Xray.Config.Models.Inbound.VlessSettings {
        Clients = new[]{ new Xray.Config.Models.VlessClient { Id = "...", Level = 0 } }
      }
    }
  }
};

config0.toJson();

// or decode from json

var config1 = Xray.Config.Models.XrayConfig.FromJson("{ ... }");
```

Use the SDK serializer to emit JSON that is compatible with xray-core and to preserve type-specific encoding for account/transport payloads.

### Api

- Purpose: High-level API surface for controlling and querying Xray programmatically.
- Contents: `IXrayApi.cs` defines the public API contract; concrete implementations and mappers live in `Xray.Api` project.
- Responsibilities:
  - Expose methods to perform management actions (for example: add/remove users, fetch runtime stats, modify live config) in a typed manner.
  - Map between wire/proto objects (from `Xray-Protos`) and SDK models (`Xray.Api.Models`) using mapper classes in `Mappers/`.
  - Provide convenience wrappers for common operations and to simplify integration for client apps.
- Typical usage: obtain an implementation of `IXrayApi`, then call methods like `GetUsers`, `AddUser`, or `GetStats()` to interact with Xray.

### Core

- Purpose: Core runtime utilities, process wrappers and low-level primitives used by other modules.
- Contents: interfaces `IXrayCore`, `IXrayLibCore`, `IXrayProcessCore`, native wrapper helpers and integration logic in `Xray.Core`.
- Responsibilities:
  - Manage lifecycle of Xray runtime (starting/stopping the process, monitoring health, piping logs).
  - Provide native interop or lightweight bindings when the SDK needs to call into packaged libraries or manage subprocesses.
  - Include common utilities used across the SDK such as options models (e.g. `PingOptions`), exception types and shared helpers.
- Typical usage: when embedding or launching an Xray process from code, use the core wrappers to start the process, supervise it, and interact with its control interfaces.

## Usage example

Below is a minimal conceptual example showing how the projects map together. Names are illustrative and reflect SDK interfaces.

```csharp
// Build config
var config = new Xray.Config.Models.Config { /* populate inbounds/outbounds */ };
var json = Xray.Config.Utilities.ConfigSerializer.Serialize(config);

// Start process via Core
IXrayProcessCore processCore = new Xray.Core.XrayProcessCore();
processCore.Start(json);

// Use API to interact
IXrayApi api = new Xray.Api.XrayApiClient(processCore.ControlEndpoint);
var users = await api.GetUsers(new Xray.Api.Models.GetUsersOptions());

// Modify runtime config or query stats
var stats = await api.GetStats();
```

Refer to the example project `Xray.Sdk.Example` for a small runnable sample that demonstrates initialization and simple API calls.

## API & Core method reference

The following section documents main interfaces and the primary API implementation. Source files:

- [Xray.Api/IXrayApi.cs](Xray.Api/IXrayApi.cs)
- [Xray.Api/XrayApi.cs](Xray.Api/XrayApi.cs)
- [Xray.Core/IXrayCore.cs](Xray.Core/IXrayCore.cs)
- [Xray.Core/IXrayProcessCore.cs](Xray.Core/IXrayProcessCore.cs)
- [Xray.Core/IXrayLibCore.cs](Xray.Core/IXrayLibCore.cs)

### IXrayApi (overview)

Interface: [Xray.Api/IXrayApi.cs](Xray.Api/IXrayApi.cs)

- `Task<List<BaseUser>> GetInboundUsers(GetUsersOptions options)` — returns the list of users for an inbound. Implementation currently queries the Handler service and maps protobuf user objects to SDK `BaseUser` models. The `options` parameter is intended to select tag/filters; the implementation defaults to tag "default".

- `Task RemoveUser(string tag, string email)` — removes a user identified by `email` from the inbound identified by `tag`. Implementation issues an `AlterInbound` request with a `RemoveUserOperation` message.

- `Task<long> GetInboundUsersCount(string tag)` — returns the user count for the inbound with the given `tag`. Calls the handler's `GetInboundUsersCount` RPC and returns `response.Count`.

- `Task AddVlessUser(AddVlessUser values)` — adds a VLESS user using provided `values` (email, uuid, level, flow, etc.). Implementation builds a typed `AddUserOperation` with a VLESS account payload and calls `AlterInbound`.

- `Task AddTrojanUser(AddTrojanUser values)` — adds a Trojan user. Builds `AddUserOperation` with Trojan account payload (password) and calls `AlterInbound`.

- `Task AddShadowsocksUser(AddShadowsocksUser values)` — adds a Shadowsocks user with cipher, password and optional IV-check. Translates SDK `CipherType` to proto `CipherType` and sends `AlterInbound`.

- `Task AddShadowsocks2022User(AddShadowsocks2022User values)` — adds a Shadowsocks2022 user using the provided `Key`.

- `Task AddSocksUser(AddSocksUser values)` — adds a SOCKS user (username/password) via `AlterInbound`.

- `Task AddHttpUser(AddHttpUser values)` — adds an HTTP proxy user (username/password) via `AlterInbound`.

- `Task<SysStats> GetSysStats()` — retrieves system-level statistics through the Stats service and maps the response to the SDK `SysStats` model using `StatsMapper`.

- `Task<List<UserStats>> GetAllUsersStats(bool reset = false)` — queries stats with pattern `"user>>>"` to collect per-user stats; `reset` will clear counters if true. Response is mapped to `UserStats` models.

- `Task<UserStats> GetUserStats(string email, bool reset = false)` — queries stats for a single user by email (pattern `"user>>>{email}>>>"`) and returns the first matching `UserStats` object.

- `Task<bool> GetUserOnlineStatus(string email)` — checks if a user's online stat exists by calling `GetStatsOnline`. If stat is present returns `true`; if RPC fails with message containing `"online not found."` returns `false`; other exceptions are rethrown.

Notes: implementation lives in [Xray.Api/XrayApi.cs](Xray.Api/XrayApi.cs). A helper `CreateAddUserOperation(BaseAddUser model, TypedMessage account)` constructs the `AlterInboundRequest` carrying an `AddUserOperation` protobuf message.

### IXrayCore and process/lib variants

Interfaces: [Xray.Core/IXrayCore.cs](Xray.Core/IXrayCore.cs), [Xray.Core/IXrayProcessCore.cs](Xray.Core/IXrayProcessCore.cs), [Xray.Core/IXrayLibCore.cs](Xray.Core/IXrayLibCore.cs)

`IXrayCore` defines the minimal runtime lifecycle surface used across the SDK:

- `bool TryStart(XrayConfig config)` — attempts to start Xray with the provided `XrayConfig` model and returns `true` on success, `false` on failure. Use when caller prefers a non-throwing start.

- `void Start(XrayConfig config)` — starts Xray using the provided config. A failing start is expected to throw (implementation-defined exception) so callers can observe error details.

- `void Stop()` — stops the running Xray instance (process or library) and releases any resources.

- `bool IsStarted()` — returns whether the runtime is currently started and healthy.

- `string Version()` — returns a short version string for the embedded/managed Xray binary or library.

`IXrayProcessCore` and `IXrayLibCore` are specialized markers that inherit from `IXrayCore`. `IXrayProcessCore` is intended for process-based hosting (spawned executable), while `IXrayLibCore` is intended for in-process/native bindings. Concrete implementations should document additional behavior such as logging, control endpoints, and health checks.

## Contributing

Contributions are welcome. Open issues or pull requests against the solution. Follow existing code style and add unit tests to `Xray.Sdk.Tests` when applicable.

## License

Check the repository root or project files for license details.
