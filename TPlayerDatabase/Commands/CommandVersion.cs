using Rocket.API;
using System.Collections.Generic;
using System.Reflection;
using Tavstal.TLibrary.Helpers.Unturned;

namespace Tavstal.TPlayerDatabase.Commands
{
    public class CommandVersion : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => ("v" + Assembly.GetExecutingAssembly().GetName().Name);
        public string Help => "Gets the version of the plugin";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "tplayerdatabase.command.version" };


        public void Execute(IRocketPlayer caller, string[] command)
        {
            // Please do not remove this region and its code, because the license require credits to the author.
            #region Credits to Tavstal
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, "#########################################");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# This plugin uses TLibrary.");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# TLibrary Created By: Tavstal");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# Github: https://github.com/TavstalDev/TLibrary/tree/master");
            #endregion
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, "#########################################");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# Build Version: {TPlayerDatabase.Version}");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# Build Date: {TPlayerDatabase.BuildDate}");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, "#########################################");
        }
    }
}
