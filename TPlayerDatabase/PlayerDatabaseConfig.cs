using Tavstal.TLibrary.Models.Config;
using Tavstal.TPlayerDatabase.Models;
using YamlDotNet.Serialization;
// ReSharper disable ClassNeverInstantiated.Global

namespace Tavstal.TPlayerDatabase
{
    /// <summary>
    /// Represents the YAML-serialized configuration for the TPlayerDatabase plugin.
    /// Contains database connection settings and general plugin options.
    /// </summary>
    public class PlayerDatabaseConfig : YamlConfiguration
    {
        /// <summary>
        /// Gets or sets the database connection and table configuration.
        /// </summary>
        [YamlMember(Order = 3)]
        public DatabaseData Database { get; set; }

        /// <summary>
        /// Populates the configuration with default values, including locale, log level,
        /// and a default database configuration with the <c>tpdb_</c> table prefix.
        /// </summary>
        public override void LoadDefaults()
        {
            General = new GeneralConfig
            {
                MessageIcon = "https://raw.githubusercontent.com/TavstalDev/TPlayerDatabase/refs/heads/master/assets/icon.png"
            };
            Database = new DatabaseData();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerDatabaseConfig"/> with default database settings.
        /// Required by the serialization library.
        /// </summary>
        public PlayerDatabaseConfig()
        {
            Database = new DatabaseData();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerDatabaseConfig"/> with the specified file name and path.
        /// </summary>
        /// <param name="fileName">The name of the configuration file.</param>
        /// <param name="path">The directory path where the configuration file is located.</param>
        public PlayerDatabaseConfig(string fileName, string path) : base(fileName, path)
        {
            Database = new DatabaseData();
        }
    }
}
