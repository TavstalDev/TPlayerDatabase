using System;
using System.Threading.Tasks;
using Tavstal.TLibrary.Extensions;
using Tavstal.TLibrary.Managers;
using Tavstal.TLibrary.Models.Database;
using Tavstal.TLibrary.Models.Plugin;
using Tavstal.TPlayerDatabase.Models;

namespace Tavstal.TPlayerDatabase.Utils.Managers
{
    /// <summary>
    /// Manages the MySQL database connection and provides access to the player data repository.
    /// </summary>
    public class DatabaseManager : DatabaseManagerBase
    {
        /// <summary>
        /// Gets the repository for performing CRUD operations on <see cref="PlayerData"/> records.
        /// </summary>
        public MySqlRepository<ulong, PlayerData> Players { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseManager"/> and creates the player repository.
        /// </summary>
        /// <param name="plugin">The owning plugin instance.</param>
        /// <param name="config">The plugin configuration containing database settings.</param>
        public DatabaseManager(IPlugin plugin, PlayerDatabaseConfig config) : base(plugin, config.Database)
        {
            Players = new MySqlRepository<ulong, PlayerData>(this, config.Database.TablePrefix);
        }

        /// <summary>
        /// Verifies that the database schema exists and creates it if necessary.
        /// </summary>
        public override async Task CheckSchemaAsync()
        {
            try
            {
                await using var connection = CreateConnection();
                await Players.CheckSchemaAsync(connection);
            }
            catch (Exception ex)
            {
                TPlayerDatabase.Logger.Error("Error in checkSchema:", ex);
            }
        }
    }
}
