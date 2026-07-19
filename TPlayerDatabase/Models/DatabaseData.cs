using Tavstal.TLibrary.Models.Database;
using YamlDotNet.Serialization;

namespace Tavstal.TPlayerDatabase.Models
{
    /// <summary>
    /// Extends the base database settings with a table prefix specific to this plugin.
    /// </summary>
    public class DatabaseData : DatabaseSettingsBase
    {
        /// <summary>
        /// Gets or sets the prefix applied to all table names created by this plugin.
        /// </summary>
        [YamlMember(Order = 7)]
        public string TablePrefix { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseData"/> with the specified table prefix.
        /// </summary>
        /// <param name="tablePrefixName">The prefix to prepend to all table names.</param>
        public DatabaseData(string tablePrefixName) 
        {
            TablePrefix = tablePrefixName;
        }
    }
}
