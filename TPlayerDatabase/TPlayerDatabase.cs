using System;
using SDG.Unturned;
using System.Collections.Generic;
using Tavstal.TLibrary.Models.Plugin;
using Tavstal.TPlayerDatabase.Utils.Handlers;
using Tavstal.TPlayerDatabase.Utils.Managers;

namespace Tavstal.TPlayerDatabase
{
    /// <summary>
    /// The main plugin class.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class TPlayerDatabase : PluginBase<PlayerDatabaseConfig>
    {
        public static TPlayerDatabase Instance { get; private set; }
        public new static readonly TLogger Logger = new TLogger("TPlayerDatabase", false);
        public static DatabaseManager DatabaseManager { get; private set; }
        
        /// <summary>
        /// Used to prevent error spamming that is related to database configuration.
        /// </summary>
        public static bool IsConnectionAuthFailed { get; set; }

        /// <summary>
        /// Fired when the plugin is loaded.
        /// </summary>
        public override void OnLoad()
        {
            Instance = this;
            // Attach event, which will be fired when all plugins are loaded.
            Level.onPostLevelLoaded += Event_OnPluginsLoaded;
            // Attach player related events
            PlayerEventHandler.AttachEvents();

            Logger.Log("████████╗██████╗ ██╗      █████╗ ██╗   ██╗███████╗██████╗ ██████╗ ██████╗", ConsoleColor.Cyan, prefix: null);
            Logger.Log("╚══██╔══╝██╔══██╗██║     ██╔══██╗╚██╗ ██╔╝██╔════╝██╔══██╗██╔══██╗██╔══██╗", ConsoleColor.Cyan, prefix: null);
            Logger.Log("   ██║   ██████╔╝██║     ███████║ ╚████╔╝ █████╗  ██████╔╝██║  ██║██████╔", ConsoleColor.Cyan, prefix: null);
            Logger.Log("   ██║   ██╔═══╝ ██║     ██╔══██║  ╚██╔╝  ██╔══╝  ██╔══██╗██║  ██║██╔══██╗", ConsoleColor.Cyan, prefix: null);
            Logger.Log("   ██║   ██║     ███████╗██║  ██║   ██║   ███████╗██║  ██║██████╔╝██████╔╝", ConsoleColor.Cyan, prefix: null);
            Logger.Log("   ╚═╝   ╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═════╝ ╚═════╝ ", ConsoleColor.Cyan, prefix: null);
            Logger.Log("#########################################", prefix: null);
            Logger.Log("#       Thanks for using this plugin!   #", prefix: null);
            Logger.Log("#########################################", prefix: null);
            Logger.Log("# Developed By: Tavstal", prefix: null);
            Logger.Log("# Discord:      @Tavstal", prefix: null);
            Logger.Log("# Website:      https://redstoneplugins.com", prefix: null);
            Logger.Log("# My GitHub:    https://tavstaldev.github.io", prefix: null);
            Logger.Log("#########################################", prefix: null);
            Logger.Log($"# Plugin Version:    {Version}", prefix: null);
            Logger.Log($"# Build Date:        {BuildDate}", prefix: null);
            Logger.Log($"# TLibrary Version:  {LibraryVersion}", prefix: null);
            Logger.Log("#########################################", prefix: null);
            Logger.Log("# Found an issue or have a suggestion?", prefix: null);
            Logger.Log("# Report it here: https://github.com/TavstalDev/TPlayerDatabase/issues", prefix: null); 
            Logger.Log("#########################################", prefix: null);

            DatabaseManager = new DatabaseManager(this, Config);
            if (IsConnectionAuthFailed)
                return;

            Logger.Log($"# {Name} has been loaded.");
        }

        /// <summary>
        /// Fired when the plugin is unloaded.
        /// </summary>
        public override void OnUnLoad()
        {
            Level.onPostLevelLoaded -= Event_OnPluginsLoaded;
            PlayerEventHandler.DetachEvents();
            Logger.Log($"# {Name} has been successfully unloaded.");
        }

        private void Event_OnPluginsLoaded(int i)
        {
            if (IsConnectionAuthFailed)
            {
                Logger.Warning($"# Unloading {GetPluginName()} due to database authentication error.");
                UnloadPlugin();
                //return;
            }
        }


        public override Dictionary<string, string> DefaultLocalization => new Dictionary<string, string>();
    }
}