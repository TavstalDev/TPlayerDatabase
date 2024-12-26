using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Tavstal.TLibrary.Extensions;
using Tavstal.TLibrary.Helpers.General;
using Tavstal.TLibrary.Managers;
using Tavstal.TLibrary.Models.Database;
using Tavstal.TLibrary.Models.Plugin;
using Tavstal.TPlayerDatabase.Models;

namespace Tavstal.TPlayerDatabase.Utils.Managers
{
    /// <summary>
    /// Manages database operations related to the application, inheriting from <see cref="DatabaseManagerBase"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="DatabaseManager"/> class provides methods for interacting with the database, such as adding, removing, and updating records.
    /// It inherits from <see cref="DatabaseManagerBase"/>, which contains common database functionality. This class can be extended with custom methods specific to the application.
    /// </remarks>
    public class DatabaseManager : DatabaseManagerBase
    {
        // ReSharper disable once InconsistentNaming
        private static PlayerDatabaseConfig _pluginConfig => TPlayerDatabase.Instance.Config;

        public DatabaseManager(IPlugin plugin, IConfigurationBase config) : base(plugin, config)
        {

        }

        /// <summary>
        /// Checks the schema of the database, creates or modifies the tables if needed
        /// <br/>PS. If you change the Primary Key then you must delete the table.
        /// </summary>
        public override async Task CheckSchemaAsync()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    if (!await connection.OpenSafeAsync())
                        TPlayerDatabase.IsConnectionAuthFailed = true;
                    if (connection.State != System.Data.ConnectionState.Open)
                        throw new Exception("# Failed to connect to the database. Please check the plugin's config file.");

                    // Player Table
                    if (await connection.DoesTableExistAsync<PlayerData>(_pluginConfig.Database.PlayersTable))
                        await connection.CheckTableAsync<PlayerData>(_pluginConfig.Database.PlayersTable);
                    else
                        await connection.CreateTableAsync<PlayerData>(_pluginConfig.Database.PlayersTable);

                    if (connection.State != System.Data.ConnectionState.Closed)
                        await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                TPlayerDatabase.Logger.LogException("Error in checkSchema:");
                TPlayerDatabase.Logger.LogError(ex);
            }
        }

        #region Player Table
        /// <summary>
        /// Adds a new player to the database asynchronously.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player.</param>
        /// <param name="steamName">The Steam name of the player.</param>
        /// <param name="characterName">The in-game character name of the player.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a boolean indicating whether the player was added successfully.
        /// </returns>
        /// <remarks>
        /// This method asynchronously adds a player to the database, storing their Steam ID, Steam name, and character name.
        /// It can be used when a new player joins the game or registers for the first time.
        /// </remarks>
        public async Task<bool> AddPlayerAsync(ulong steamId, string steamName, string characterName)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.AddTableRowAsync(tableName: _pluginConfig.Database.PlayersTable, value: new PlayerData(steamId, steamName, characterName, DateTime.Now));
        }

        /// <summary>
        /// Removes a player from the database asynchronously.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player to be removed.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a boolean indicating whether the player was successfully removed.
        /// </returns>
        /// <remarks>
        /// This method asynchronously removes a player from the database based on their Steam ID.
        /// It can be used when a player leaves the game or is otherwise no longer part of the system.
        /// </remarks>
        public async Task<bool> RemovePlayerAsync(ulong steamId)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return  await mySqlConnection.RemoveTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: $"SteamId='{steamId}'", parameters: null);
        }

        /// <summary>
        /// Updates the character name of a player in the database asynchronously.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player whose information is to be updated.</param>
        /// <param name="characterName">The new character name to be set for the player.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a boolean indicating whether the player's information was successfully updated.
        /// </returns>
        /// <remarks>
        /// This method updates the character name of a player in the database, using the player's Steam ID as the unique identifier.
        /// It can be used to modify a player's character name when they change it within the game.
        /// </remarks>
        public async Task<bool> UpdatePlayerAsync(ulong steamId, string characterName)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.UpdateTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, $"SteamId='{steamId}'", new List<SqlParameter>
            {
                SqlParameter.Get<PlayerData>(x => x.LastCharacterName, characterName),
                SqlParameter.Get<PlayerData>(x => x.LastLogin, DateTime.Now)
            });
        }

        /// <summary>
        /// Retrieves a list of all players from the database asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a list of <see cref="PlayerData"/> representing all players in the database.
        /// </returns>
        /// <remarks>
        /// This method fetches all player records from the database asynchronously and returns them as a list of <see cref="PlayerData"/> objects.
        /// It can be used to gather information on all players, such as their Steam IDs and character names.
        /// </remarks>
        public async Task<List<PlayerData>> GetPlayersAsync()
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.GetTableRowsAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: string.Empty, null);
        }

        /// <summary>
        /// Retrieves the player data for a specific player by their Steam ID asynchronously.
        /// </summary>
        /// <param name="steamId">The Steam ID of the player whose data is to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a <see cref="PlayerData"/> object containing the player's data, or null if the player is not found.
        /// </returns>
        /// <remarks>
        /// This method fetches the player data for a specific player based on their Steam ID. It returns a <see cref="PlayerData"/> object with the player's information, or null if no player with the given Steam ID exists.
        /// </remarks>
        public async Task<PlayerData> FindPlayerAsync(ulong steamId)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.GetTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: $"SteamId='{steamId}'", null);
        }
        #endregion
    }
}
