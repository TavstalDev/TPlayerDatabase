using Tavstal.TLibrary.Models.Config;
using YamlDotNet.Serialization;

namespace Tavstal.TPlayerDatabase.Models
{
    /// <summary>
    /// Extends the base database settings with a table prefix specific to this plugin.
    /// </summary>
    public class DatabaseData : DatabaseConfigBase
    {
        /// <summary>
        /// Gets or sets the prefix applied to all table names created by this plugin.
        /// </summary>
        [YamlMember(Order = 7)]
        public string TablePrefix { get; set; } = "tpdb_";
    }
}
