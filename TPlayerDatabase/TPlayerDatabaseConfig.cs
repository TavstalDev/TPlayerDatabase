using Newtonsoft.Json;
using Tavstal.TPlayerDatabase.Models;
using Tavstal.TLibrary.Models.Plugin;

namespace Tavstal.TPlayerDatabase
{
    public class TPlayerDatabaseConfig : ConfigurationBase
    {
        [JsonProperty(Order = 3)]
        public DatabaseData Database { get; set; }

        public override void LoadDefaults()
        {
            Database = new DatabaseData("tpdb_players");
        }

        // Required because of the library
        public TPlayerDatabaseConfig() { }
        public TPlayerDatabaseConfig(string fileName, string path) : base(fileName, path) { }
    }
}
