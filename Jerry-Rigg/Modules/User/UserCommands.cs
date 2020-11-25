/*
 * Jerry Rigg - Commands
 *      > User Commands
 * Start date: 22 - 11 - 2020
 * Last update: 22 - 11 - 2020
 */

// Import libraries and extensions
using Discord;
using System.IO;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.Rest;

// Import foreign classes, modules, and namespaces
using JerryRigg2.Start;
using JerryRigg2.Services;
using JerryRigg2.Modules;

namespace JerryRigg2.Modules.User
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        
        [Command("userinfo")]
        [Alias("user", "uinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {

            user ??= Context.User;

            await ReplyAsync(user.ToString());
        }

    }
}