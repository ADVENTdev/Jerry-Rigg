/*
 * Jerry Rigg
 * Author: ADVENT#0216
 * Start date: 25 - 11 - 2020
 * MRU: 25 - 11 - 2020
 */
using System;
using System.Text;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JerryRigg2.Modules.Games
{
    class LobbyManager : ModuleBase
    {
        public static List<Lobby> Lobbies;
        public static ICommandContext Context;
        public LobbyManager()
        {
            Lobbies = new List<Lobby>();
        }

        public static async Task Create(string args) 
        {

        }

        public static async Task Settings(string args)
        {

        }

        public static async Task SetGame(string args)
        {

        }

        public static async Task Invite(string args) 
        {

        }

        public static async Task Decline()
        {

        }

        public static async Task Accept()
        {

        }

        public static async Task Kick(string args) 
        { 
        
        }

        public static async Task Leave() 
        { 
        
        }

        public static async Task Delete() 
        { 
        
        }

        public static Lobby GetLobby(ulong User = 0) 
        {
            if (User == 0)
                User = Context.User.Id;

            for (int i = 0; i < Lobbies.Count; i++)
            {
                var lobby = Lobbies[i];
                if (lobby.Context.Guild.Id == Context.Guild.Id)
                    for (int j = 0; j < lobby.Players.Count; j++)
                    {
                        if (lobby.Players[j].Id == User)
                            return lobby;
                    }
            }
            return null;
        }

        public static async Task StartGame()
        {

        }
    }
}
