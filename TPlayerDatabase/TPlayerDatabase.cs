﻿using SDG.Unturned;
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

            Logger.Log("#########################################");
            Logger.Log("# Thanks for using my plugin");
            Logger.Log($"# Plugin Created By Tavstal");
            Logger.Log("# Discord: @TavstalDev");
            Logger.Log("# Website: https://your.website.example");
            Logger.Log("# Discord Guild: https://discord.gg/your_invite");
            // Please do not remove this region and its code, because the license require credits to the author.
            #region Credits to Tavstal
            Logger.Log("#########################################");
            Logger.Log($"# This plugin uses TLibrary.");
            Logger.Log($"# TLibrary Created By: Tavstal"); 
            Logger.Log($"# Github: https://github.com/TavstalDev/TLibrary/tree/master");
            #endregion
            Logger.Log("#########################################");
            Logger.Log($"# Build Version: {Version}");
            Logger.Log($"# Build Date: {BuildDate}");
            Logger.Log("#########################################");

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
                Logger.LogWarning($"# Unloading {GetPluginName()} due to database authentication error.");
                UnloadPlugin();
                //return;
            }
        }


        public override Dictionary<string, string> DefaultLocalization => new Dictionary<string, string>();
    }
}