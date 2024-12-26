using System.Collections.Generic;
using System.Threading.Tasks;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Tavstal.TPlayerDatabase.Models;

namespace Tavstal.TPlayerDatabase.Utils.Handlers
{
    /// <summary>
    /// Provides event handlers for player-related events in the game.
    /// </summary>
    /// <remarks>
    /// This class contains static methods that handle various player actions, such as player connection, disconnection,
    /// item interactions, and other events triggered by player activity.
    /// </remarks>
    public static class PlayerEventHandler
    {
        private static bool _isAttached;
        // ReSharper disable once InconsistentNaming
        private static readonly List<UnturnedPlayer> _players = new List<UnturnedPlayer>();
        public static List<UnturnedPlayer> Players => _players;

        /// <summary>
        /// Attaches event handlers to player-related events.
        /// </summary>
        /// <remarks>
        /// This method subscribes to the events that trigger player actions such as connection, disconnection, item interactions,
        /// and other player-related activities. It ensures that the appropriate event handlers are invoked when these actions occur.
        /// </remarks>
        public static void AttachEvents()
        {
            if (_isAttached)
                return;

            _isAttached = true;

            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
        }

        /// <summary>
        /// Detaches event handlers from player-related events.
        /// </summary>
        /// <remarks>
        /// This method unsubscribes from the events related to player actions such as connection, disconnection, and item interactions.
        /// It ensures that the event handlers are no longer invoked for these events when the player actions occur.
        /// </remarks>
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
        /// </summary>
        /// <param name="player">The player who has connected.</param>
        /// <remarks>
        /// This method is triggered when a player connects to the game.
        /// It can be used to subscribe the player to necessary events, initialize player data, or perform any actions required when a player joins.
        /// </remarks>
        private static void OnPlayerConnected(UnturnedPlayer player)
        {
            if (!_players.Contains(player)) 
                _players.Add(player);

            Task.Run(async () =>
            {
                PlayerData data = await TPlayerDatabase.DatabaseManager.FindPlayerAsync(player.CSteamID.m_SteamID);
                if (data == null)
                {
                    await TPlayerDatabase.DatabaseManager.AddPlayerAsync(player.CSteamID.m_SteamID, player.SteamName, player.CharacterName);
                }
                else
                {
                    if (data.LastCharacterName != player.CharacterName)
                        await TPlayerDatabase.DatabaseManager.UpdatePlayerAsync(player.CSteamID.m_SteamID, player.CharacterName);
                }
            });
        }

        /// <summary>
        /// Handles the event when a player disconnects from the game.
        /// </summary>
        /// <param name="player">The player who has disconnected.</param>
        /// <remarks>
        /// This method is triggered when a player disconnects from the game.
        /// It can be used to unsubscribe the player from events, save player data, or perform any necessary cleanup tasks when a player leaves.
        /// </remarks>
        private static void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (_players.Contains(player))
                _players.Remove(player);
        }
    }
}
