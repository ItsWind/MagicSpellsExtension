using System.Collections.Generic;
using TaleWorlds.Library;

namespace MagicSpells
{
    internal class ConsoleCommands
    {
        [CommandLineFunctionality.CommandLineArgumentFunction("reloadconfig", "magicspells")]
        private static string CommandReloadConfig(List<string> args)
        {
            SubModule.Config.LoadConfig();
            return "Config reloaded!";
        }
    }
}