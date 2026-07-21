using Rocket.API;
using System.Collections.Generic;
using System.Reflection;
using Tavstal.TLibrary.Helpers.Unturned;
// ReSharper disable UnusedType.Global

namespace Tavstal.TPlayerDatabase.Commands
{
    /// <summary>
    /// RocketMod command that reports the plugin's build version and date to the caller.
    /// </summary>
    public class CommandVersion : IRocketCommand
    {
        /// <inheritdoc />
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        /// <inheritdoc />
        public string Name => ("v" + Assembly.GetExecutingAssembly().GetName().Name);

        /// <inheritdoc />
        public string Help => "Gets the version of the plugin";

        /// <inheritdoc />
        public string Syntax => "";

        /// <inheritdoc />
        public List<string> Aliases => new List<string>();

        /// <inheritdoc />
        public List<string> Permissions => new List<string> { "tplayerdatabase.commands.version" };

        /// <summary>
        /// Executes the version command, sending the plugin version and build date to the caller.
        /// </summary>
        /// <param name="caller">The player or console that invoked the command.</param>
        /// <param name="command">The command arguments (unused).</param>
        public void Execute(IRocketPlayer caller, string[] command)
        {
            var instance = TPlayerDatabase.Instance;
            var config = instance.Config.General;
            var icon = config.MessageIcon;
            string message = string.Join(System.Environment.NewLine, 
                $"&b&l[{instance.GetPluginName()}]&r System Info:",
                $"&b • Version: &r{TPlayerDatabase.Version}",
                $"&b • Build Date: &r{TPlayerDatabase.BuildDate}",
                "&b • Developer: &rTavstal");
            
            instance.SendPlainCommandReply(caller, message, icon);
        }
    }
}
