using System;
using Tavstal.TLibrary.Models.Database.Attributes;

namespace Tavstal.TPlayerDatabase.Models
{
    /// <summary>
    /// Represents a player record stored in the MySQL database.
    /// Maps to the player table with columns defined by the <see cref="SqlMember"/> attributes.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        /// <summary>
        /// Gets or sets the player's Steam ID. Used as the primary key.
        /// </summary>
        [SqlMember("SteamId", "varchar(17)", isPrimaryKey: true)]
        public ulong SteamId { get; set; }

        /// <summary>
        /// Gets or sets the player's Steam display name.
        /// </summary>
        [SqlMember("SteamName", "varchar(32)")]
        public string SteamName { get; set; }

        /// <summary>
        /// Gets or sets the player's last used in-game character name.
        /// </summary>
        [SqlMember("LastCharacterName", "varchar(50)")]
        public string LastCharacterName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the player's most recent login.
        /// </summary>
        [SqlMember("LastLogin")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerData"/> with default values.
        /// </summary>
        public PlayerData()
        {
            SteamName = string.Empty;
            LastCharacterName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerData"/> with the specified values.
        /// </summary>
        /// <param name="steamId">The player's Steam ID.</param>
        /// <param name="steamName">The player's Steam display name.</param>
        /// <param name="characterName">The player's in-game character name.</param>
        /// <param name="lastLogin">The date and time of the player's last login.</param>
        public PlayerData(ulong steamId, string steamName, string characterName, DateTime lastLogin)
        {
            SteamId = steamId;
            SteamName = steamName;
            LastCharacterName = characterName;
            LastLogin = lastLogin;
        }
    }
}
