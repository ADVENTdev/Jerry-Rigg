/*
 * Jerry Rigg - Commands
 *      > General Commands
 * Start date: 22 - 11 - 2020
 * Last update: 22 - 11 - 2020
 */

// Import libraries and extensions
using Discord;
using System.IO;
using Discord.Commands;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


// Import foreign classes, modules, and namespaces
using JerryRigg2.Start;
using JerryRigg2.Services;
using JerryRigg2.Modules;

namespace JerryRigg2.Modules.General
{
    public class CommonModule : ModuleBase<SocketCommandContext>
    {

        [Command("help")]
        [Alias("cmds", "commands")]
        public Task HelpCommand() 
        {
            ReplyAsync( "Prefix: jr!\n" + 
                        "- `help`\n" + 
                        "- `userinfo`\n" + 
                        "- `ping`");
            return null;
        }

        [Command ("manual")]
        [Alias ("man", "help")]
        public Task ManualCommand([Remainder]string command)
        {
            return null;
        }

        [Command("categories")]
        [Alias("cat", "catlist")]
        public Task CategoriesAsync([Remainder] string category) 
        {
            return null;
        }
    }
}

