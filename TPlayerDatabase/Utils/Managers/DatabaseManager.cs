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
        public async Task<bool> AddPlayerAsync(ulong steamId, string steamName, string characterName)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.AddTableRowAsync(tableName: _pluginConfig.Database.PlayersTable, value: new PlayerData(steamId, steamName, characterName, DateTime.Now));
        }

        public async Task<bool> RemovePlayerAsync(ulong steamId)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return  await mySqlConnection.RemoveTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: $"SteamId='{steamId}'", parameters: null);
        }

        public async Task<bool> UpdatePlayerAsync(ulong steamId, string characterName)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.UpdateTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, $"SteamId='{steamId}'", new List<SqlParameter>
            {
                SqlParameter.Get<PlayerData>(x => x.LastCharacterName, characterName),
                SqlParameter.Get<PlayerData>(x => x.LastLogin, DateTime.Now)
            });
        }

        public async Task<List<PlayerData>> GetPlayersAsync()
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.GetTableRowsAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: string.Empty, null);
        }

        public async Task<PlayerData> FindPlayerAsync(ulong steamId)
        {
            MySqlConnection mySqlConnection = CreateConnection();
            return await mySqlConnection.GetTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.PlayersTable, whereClause: $"SteamId='{steamId}'", null);
        }
        #endregion
    }
}
