﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tavstal.TPlayerDatabase.Models;
using Tavstal.TLibrary.Compatibility;
using Tavstal.TLibrary.Compatibility.Database;
using Tavstal.TLibrary.Compatibility.Interfaces;
using Tavstal.TLibrary.Extensions;
using Tavstal.TLibrary.Helpers.General;
using Tavstal.TLibrary.Managers;

namespace Tavstal.TPlayerDatabase.Managers
{
    public class DatabaseManager : DatabaseManagerBase
    {
#pragma warning disable IDE1006 //
        private static TPlayerDatabaseConfig _pluginConfig => TPlayerDatabase.Instance.Config;
#pragma warning restore IDE1006 //

        public DatabaseManager(IPlugin plugin, IConfigurationBase config) : base(plugin, config)
        {

        }

        /// <summary>
        /// Checks the schema of the database, creates or modifies the tables if needed
        /// <br/>PS. If you change the Primary Key then you must delete the table.
        /// </summary>
        protected override async void CheckSchema()
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
                    if (await connection.DoesTableExistAsync<PlayerData>(_pluginConfig.Database.DatabaseTable_Players))
                        await connection.CheckTableAsync<PlayerData>(_pluginConfig.Database.DatabaseTable_Players);
                    else
                        await connection.CreateTableAsync<PlayerData>(_pluginConfig.Database.DatabaseTable_Players);

                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
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
            MySqlConnection MySQLConnection = CreateConnection();
            return await MySQLConnection.AddTableRowAsync(tableName: _pluginConfig.Database.DatabaseTable_Players, value: new PlayerData(steamId, steamName, characterName, DateTime.Now));
        }

        public async Task<bool> RemovePlayerAsync(ulong steamId)
        {
            MySqlConnection MySQLConnection = CreateConnection();
            return  await MySQLConnection.RemoveTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.DatabaseTable_Players, whereClause: $"SteamId='{steamId}'", parameters: null);
        }

        public async Task<bool> UpdatePlayerAsync(ulong steamId, string characterName)
        {
            MySqlConnection MySQLConnection = CreateConnection();
            return await MySQLConnection.UpdateTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.DatabaseTable_Players, $"SteamId='{steamId}'", new List<SqlParameter>
            {
                SqlParameter.Get<PlayerData>(x => x.LastCharacterName, characterName),
                SqlParameter.Get<PlayerData>(x => x.LastLogin, DateTime.Now)
            });
        }

        public async Task<List<PlayerData>> GetPlayersAsync()
        {
            MySqlConnection MySQLConnection = CreateConnection();
            return await MySQLConnection.GetTableRowsAsync<PlayerData>(tableName: _pluginConfig.Database.DatabaseTable_Players, whereClause: string.Empty, null);
        }

        public async Task<PlayerData> FindPlayerAsync(ulong steamId)
        {
            MySqlConnection MySQLConnection = CreateConnection();
            return await MySQLConnection.GetTableRowAsync<PlayerData>(tableName: _pluginConfig.Database.DatabaseTable_Players, whereClause: $"SteamId='{steamId}'", null);
        }
        #endregion
    }
}