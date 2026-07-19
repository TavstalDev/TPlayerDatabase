using System;
using System.Collections.Concurrent;
using System.Threading;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Tavstal.TLibrary.Threading;
using Tavstal.TPlayerDatabase.Models;

namespace Tavstal.TPlayerDatabase.Utils.Handlers
{
    /// <summary>
    /// Provides event handlers for player-related events in the game.
    /// </summary>
    public static class PlayerEventHandler
    {
        private static bool _isAttached;
        private static readonly ConcurrentDictionary<ulong, SemaphoreSlim> _playerLocks = new ConcurrentDictionary<ulong, SemaphoreSlim>();

        /// <summary>
        /// Attaches event handlers to player-related events.
        /// </summary>
        public static void AttachEvents()
        {
            if (_isAttached)
                return;

            _isAttached = true;
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
        }

        /// <summary>
        /// Detaches event handlers from player-related events.
        /// </summary>
        public static void DetachEvents()
        {
            if (!_isAttached)
                return;

            _isAttached = false;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }

        /// <summary>
        /// Handles the event when a player connects to the game.
        /// Inserts a new record if the player is unknown, or updates the character name if it has changed.
        /// </summary>
        /// <param name="player">The player who has connected.</param>
        private static void OnPlayerConnected(UnturnedPlayer player)
        {
            BackgroundThreadDispatcher.RunAsync(async () =>
            {
                var playerLock = _playerLocks.GetOrAdd(player.CSteamID.m_SteamID, new SemaphoreSlim(1, 1));
                await playerLock.WaitAsync();
                try
                {
                    PlayerData? data = await TPlayerDatabase.DatabaseManager.Players.GetAsync(player.CSteamID.m_SteamID);
                    if (data == null)
                    {
                        await TPlayerDatabase.DatabaseManager.Players.AddAsync(new PlayerData
                        {
                            SteamId = player.CSteamID.m_SteamID,
                            SteamName = player.SteamName,
                            LastCharacterName = player.CharacterName,
                            LastLogin = DateTime.Now
                        });
                        return;
                    }
                    
                    if (data.LastCharacterName.Equals(player.CharacterName, StringComparison.InvariantCulture))
                        return;
                    
                    data.LastCharacterName = player.CharacterName;
                    await TPlayerDatabase.DatabaseManager.Players.UpdateAsync(data.SteamId, data);
                }
                finally
                {
                    playerLock.Release();
                }
            });
        }

        /// <summary>
        /// Handles the event when a player disconnects from the game.
        /// Releases and removes the per-player lock to free resources.
        /// </summary>
        /// <param name="player">The player who has disconnected.</param>
        private static void OnPlayerDisconnected(UnturnedPlayer player)
        {
            BackgroundThreadDispatcher.RunAsync(async () =>
            {
                var playerLock = _playerLocks.GetOrAdd(player.CSteamID.m_SteamID, new SemaphoreSlim(1, 1));
                await playerLock.WaitAsync();
                try
                {
                    _playerLocks.TryRemove(player.CSteamID.m_SteamID, out _);
                }
                finally
                {
                    playerLock.Release();
                }
            });
        }
    }
}
