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
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, "#########################################");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# Build Version: {TPlayerDatabase.Version}");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, $"# Build Date: {TPlayerDatabase.BuildDate}");
            TPlayerDatabase.Instance.SendPlainCommandReply(caller, "#########################################");
        }
    }
}
