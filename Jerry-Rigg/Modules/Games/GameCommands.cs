/*
 * Jerry Rigg - Commands
 *      > Game Commands
 * Start date: 22 - 11 - 2020
 * Last update: 25 - 11 - 2020
 */

// Import libraries, extensions
using System;
using Discord;
using System.IO;
using System.Text;
using Discord.Net;
using Discord.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Import namespaces, modules, classes
using JerryRigg2.Start;
using JerryRigg2.Services;
using JerryRigg2.Modules;
using JerryRigg2.Modules.Games;


namespace JerryRigg2.Modules.Games { 
    
    public class GameModule : ModuleBase<SocketCommandContext>
    {

        [Command("game")]
        [RequireContext (ContextType.Guild)]
        [Alias ("lobby", "setup")]
        public async Task LobbyCommand(string op, [Optional, Remainder] string arg1)
        // FIXME:
        // make ASYNC
        // DM is staff-only
        {
            op = op?.ToLower();
            switch (op)
            {
                case "create": //lobby create [chess]
                    await LobbyManager.Create(arg1);
                    break;
                case "invite": //lobby invite Noel-chan
                    await LobbyManager.Invite(arg1);
                    break;
                case "delete": //lobby delete
                    await LobbyManager.Delete();
                    break;
                case "kick": //lobby kick user
                    await LobbyManager.Kick(arg1);
                    break;
                case "setgame":
                case "game": //lobby game uno
                    await LobbyManager.SetGame(arg1);
                    break;
                case "leave": //lobby leave
                    await LobbyManager.Leave();
                    break;
                case "start": //lobby start
                    await LobbyManager.StartGame();
                    break;
                case "join": //lobby join
                case "accept": //lobby accept
                    await LobbyManager.Accept();
                    break;
                case "decline":
                    await LobbyManager.Decline();
                    break;
                case "settings": //lobby settings round-time=180
                    await LobbyManager.Settings(arg1);
                    break;
                default:
                    Lobby lobby = LobbyManager.GetLobby();
                    if (lobby != null)
                    {
                        await Context.Channel.SendEmbedAsync(ToEmbedBuilder(lobby));
                    }
                    break;
            }            
        }

        [Command("quit")]
        // [RequireContext (ContextType.Guild)]
        [Alias("leave", "yeetmyself", "fuck-this-shit-im-out")]
        public async Task QuitLobby()
        {
            await LobbyManager.Leave();
        }

        private EmbedBuilder ToEmbedBuilder(Lobby lobby)
        {
            return new EmbedBuilder() 
                        .WithColor(Color.Red)
                        .WithTitle(lobby.Name)
                        .WithFooter(lobby.Context.User.ToString()); 
        }
    }
}
