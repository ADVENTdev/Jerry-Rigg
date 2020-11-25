/*
 * Jerry Rigg - Game Lobbies
 * Author: ADVENT#0216
 *      (Reference from KurumiBot)
 * Start date: 22 - 11 - 2020
 * Last update: 25 - 11 - 2020
 */

// Import libraries and extensions
using System;
using Discord;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;

// Import foreign classes, modules, and namespaces
using JerryRigg2.Start;
using JerryRigg2.Modules;
using JerryRigg2.Services;
using JerryRigg2.Modules.Games;
using JerryRigg2.Modules.Games.Advanced;
using JerryRigg2.Modules.Games.All_Any;
using JerryRigg2.Modules.Games.Challenge;
using JerryRigg2.Modules.Games.Evils;
using JerryRigg2.Modules.Games.Custom;
using JerryRigg2.Modules.Games.Classic;
using System.Collections;

namespace JerryRigg2.Modules.Games
{
    public class Lobby
    {
        public ICommandContext Context { get; private set; }
        public List<IUser> Players { get; private set; }
        public Gamemode SelectedMode;
        public string Name { get; private set; }
        public IGame Game;
        public bool Ingame;

        public Lobby(ICommandContext context, string name, Gamemode? selectedmode = null)
        {
            Name = name;
            Context = context;

            Players = new List<IUser>
            {
                Context.User
            };

            if (selectedmode.HasValue)
                SetMode(selectedmode.Value);
            else
            {
                SelectedMode = Gamemode.None;
                Game = new NoneGame();
            }
        }

        public void SetMode(Gamemode game)
        {
            switch (game)
            {
                case Gamemode.classic:
                    Game = (IGame)new ClassicGame();
                    break;
                case Gamemode.advanced:
                    Game = (IGame)new AdvancedGame();
                    break;
                case Gamemode.challenge:
                    Game = (IGame)new ChallengeGame();
                    break;
                case Gamemode.evils:
                    Game = (IGame)new EvilsGame();
                    break;
                case Gamemode.custom:
                    Game = (IGame)new CustomGame();
                    break;
                case Gamemode.allany:
                    Game = (IGame)new AllAnyGame();
                    break;
            }
            SelectedMode = game;
        }

        public void QuitGame()
        {

        }

        public void RemovePlayer(ulong Player)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].Id == Player)
                {
                    Players.RemoveAt(i);
                    goto Check;
                }
            }

        Check:
            if (Players.Count == 0 || (Players.Count == 1 && Players[0].IsBot))
            {
                LobbyManager.Lobbies.Remove(this);
            }
        }
    }
}


