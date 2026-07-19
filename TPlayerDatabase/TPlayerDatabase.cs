using System;
using SDG.Unturned;
using System.Collections.Generic;
using System.Text;
using Tavstal.TLibrary.Extensions;
using Tavstal.TLibrary.Models.Logging;
using Tavstal.TLibrary.Models.Plugin;
using Tavstal.TPlayerDatabase.Utils.Handlers;
using Tavstal.TPlayerDatabase.Utils.Managers;

namespace Tavstal.TPlayerDatabase
{
    /// <summary>
    /// Main entry point for the TPlayerDatabase plugin. Handles initialization,
    /// event registration, and lifecycle management for the player database system.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class TPlayerDatabase : PluginBase<PlayerDatabaseConfig>
    {
        /// <summary>
        /// Gets the singleton instance of the plugin.
        /// </summary>
        public static TPlayerDatabase Instance { get; private set; } = null!;

        /// <summary>
        /// Gets the database manager responsible for player data persistence.
        /// </summary>
        public static DatabaseManager DatabaseManager { get; private set; } = null!;
        
        /// <summary>
        /// Gets or sets a flag indicating whether the database connection authentication has failed.
        /// Used to prevent repeated error logging and trigger an automatic plugin unload.
        /// </summary>
        public static bool IsConnectionAuthFailed { get; set; }

        /// <summary>
        /// Called before the plugin is loaded. Prints the plugin banner and build information to the console.
        /// </summary>
        public override void OnPreLoad()
        {
            Instance = this;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("────────────────────────────────────────────────────────");
            sb.AppendLine();
            sb.AppendLine("████████╗██████╗ ██╗      █████╗ ██╗   ██╗███████╗██████╗ ██████╗ ██████╗");
            sb.AppendLine("╚══██╔══╝██╔══██╗██║     ██╔══██╗╚██╗ ██╔╝██╔════╝██╔══██╗██╔══██╗██╔══██╗");
            sb.AppendLine("   ██║   ██████╔╝██║     ███████║ ╚████╔╝ █████╗  ██████╔╝██║  ██║██████╔");
            sb.AppendLine("   ██║   ██╔═══╝ ██║     ██╔══██║  ╚██╔╝  ██╔══╝  ██╔══██╗██║  ██║██╔══██╗");
            sb.AppendLine("   ██║   ██║     ███████╗██║  ██║   ██║   ███████╗██║  ██║██████╔╝██████╔╝");
            sb.AppendLine("   ╚═╝   ╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═════╝ ╚═════╝");
            sb.AppendLine();
            sb.AppendLine("[ About ]");
            sb.AppendLine(" ▸ Developer : Tavstal");
            sb.AppendLine(" ▸ Discord   : @Tavstal");
            sb.AppendLine(" ▸ Website   : https://redstoneplugins.com");
            sb.AppendLine(" ▸ GitHub    : https://github.com/TavstalDev");
            sb.AppendLine();
            sb.AppendLine("[ Build ]");
            sb.AppendLine($" ▸ Version   : {Version}");
            sb.AppendLine($" ▸ Build Date: {BuildDate} UTC");
            sb.AppendLine($" ▸ TLibrary  : {LibraryVersion}");
            sb.AppendLine();
            sb.AppendLine("[ Support ]");
            sb.AppendLine(" ▸ Report issues or request features:");
            sb.AppendLine(" ▸ https://github.com/TavstalDev/TPlayerDatabase/issues");
            sb.AppendLine();
            sb.AppendLine("────────────────────────────────────────────────────────");
            Logger.Log(ELogLevel.COMMAND, sb.ToString(), includePrefixes: false, color:  ConsoleColor.Cyan);
        }
        
        /// <summary>
        /// Called when the plugin is loaded. Subscribes to level events, attaches player
        /// event handlers, and initializes the database manager.
        /// </summary>
        public override void OnLoad()
        {
            Level.onPostLevelLoaded += Event_OnPluginsLoaded;
            PlayerEventHandler.AttachEvents();
            
            DatabaseManager = new DatabaseManager(this, Config);
            if (IsConnectionAuthFailed)
                return;

            Logger.Info($"# {Name} has been successfully loaded.");
        }

        /// <summary>
        /// Called when the plugin is unloaded. Detaches all event handlers and cleans up resources.
        /// </summary>
        public override void OnUnLoad()
        {
            Level.onPostLevelLoaded -= Event_OnPluginsLoaded;
            PlayerEventHandler.DetachEvents();
            Logger.Info($"# {Name} has been successfully unloaded.");
        }

        /// <summary>
        /// Handles the post-level-loaded event. If database authentication has failed,
        /// unloads the plugin to prevent further errors.
        /// </summary>
        /// <param name="i">The level index that was loaded.</param>
        private void Event_OnPluginsLoaded(int i)
        {
            if (!IsConnectionAuthFailed)
                return;
            Logger.Warning($"# Unloading {GetPluginName()} due to database authentication error.");
            UnloadPlugin();

        }


        /// <summary>
        /// Gets the default localization dictionary for the plugin.
        /// </summary>
        public override Dictionary<string, string> DefaultLocalization => new Dictionary<string, string>();
    }
}