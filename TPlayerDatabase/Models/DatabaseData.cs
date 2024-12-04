using Newtonsoft.Json;
using Tavstal.TLibrary.Models.Database;

namespace Tavstal.TPlayerDatabase.Models
{
    public class DatabaseData : DatabaseSettingsBase
    {
        // Note: It starts from 7 because there are 6 defined property in the base class
        [JsonProperty(Order = 7)]
        public string PlayersTable { get; set; }
        public DatabaseData(string tableName) 
        {
            PlayersTable = tableName;
        }
    }
}
