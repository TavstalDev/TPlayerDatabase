# TPlayerDatabase

![Release (latest by date)](https://img.shields.io/github/v/release/TavstalDev/TPlayerDatabase?style=plastic-square)
![Workflow Status](https://img.shields.io/github/actions/workflow/status/TavstalDev/TPlayerDatabase/release.yml?branch=stable&label=build&style=plastic-square)
![License](https://img.shields.io/github/license/TavstalDev/TPlayerDatabase?style=plastic-square)
![Downloads](https://img.shields.io/github/downloads/TavstalDev/TPlayerDatabase/total?style=plastic-square)
![Issues](https://img.shields.io/github/issues/TavstalDev/TPlayerDatabase?style=plastic-square)

### What is this?
This is the source code of a .NET Framework library written in C#. It is a RocketMod plugin for Unturned 3.24.x+ servers that persists basic player information in a MySQL database.

### Description
TPlayerDatabase is a lightweight data-collection plugin that tracks player identity across sessions.
Whenever a player joins, the plugin records their Steam ID, Steam display name, in-game character name, and the timestamp of their last login. 

### Features
- **Player Tracking** -- Records each player's Steam ID, Steam display name, character name, and last login time.
- **Automatic Updates** -- Detects character name changes on reconnect and updates the database record accordingly.
- **MySQL Storage** -- Persists all data in a MySQL database for reliable, server-side storage.
- **Async & Thread-Safe** -- All database operations run asynchronously on background threads with per-player locking to prevent race conditions.
- **Graceful Error Handling** -- Automatically unloads the plugin if the database connection fails, preventing log spam and server instability.

### Requirements
- Unturned 3.24.x or later
- [RocketMod](https://rocketmod.net/) installed on the server

### Installation

1. Download the latest release and its libraries from the [Releases](https://github.com/TavstalDev/TPlayerDatabase/releases) page.
2. Place `TPlayerDatabase.dll` into your server's `Rocket/Plugins/` directory.
3. Extract the libraries archive into `Rocket/Libraries` directory.
4. Start or restart the server. The plugin will generate a default YAML configuration file on first load.
5. Edit the configuration file to your liking, then reload the plugin or restart the server.

## Building from Source

### Prerequisites

- .NET Framework 4.8 SDK / targeting pack

### Steps

1. Clone the repository:
   ```
   git clone https://github.com/TavstalDev/TPlayerDatabase.git
   ```
2. Open `TPlayerDatabase.sln` in your IDE.
3. Build the project:
   ```
   dotnet build -c Release
   ```
4. The output DLL will be at `TPlayerDatabase/bin/Release/net48/TPlayerDatabase.dll`.

## License

This project is licensed under the GNU General Public License v3.0. See the `LICENSE` file for more details.

## Contact

For issues or feature requests, please use the [GitHub issue tracker](https://github.com/TavstalDev/TPlayerDatabase/issues).