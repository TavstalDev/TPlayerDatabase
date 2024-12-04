using Newtonsoft.Json;
using Tavstal.TPlayerDatabase.Models;
using Tavstal.TLibrary.Models.Plugin;

namespace Tavstal.TPlayerDatabase
{
    public class PlayerDatabaseConfig : ConfigurationBase
    {
        [JsonProperty(Order = 3)]
        public DatabaseData Database { get; set; }

        public override void LoadDefaults()
        {
            DebugMode = false;
            Locale = "en";
            DownloadLocalePacks = true;
            Database = new DatabaseData("tpdb_players");
        }

        // Required because of the library
        public PlayerDatabaseConfig() { }
        public PlayerDatabaseConfig(string fileName, string path) : base(fileName, path) { }
    }
}
