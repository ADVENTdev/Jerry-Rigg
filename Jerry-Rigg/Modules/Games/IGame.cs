using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JerryRigg2.Modules.Games
{
    public interface IGame
    {
        int MinPlayers { get; }
        int MaxPlayers { get; }
        Dictionary<string, object> Settings { get; }
        
        Task Start(Lobby lobby, ICommandContext context);
        bool ValidPlayer(IUser user);
        string ToString();
    }

    public class NoneGame : IGame
    {
        public int MinPlayers { get; private set; }
        public int MaxPlayers { get; private set; }
        public Dictionary<string, object> Settings { get; private set; }
        public NoneGame()
        {
            MinPlayers = 1;
            MaxPlayers = 20;
            Settings = new Dictionary<string, object>();
        }
        public bool ValidPlayer(IUser user) => true;

        public Task Start(Lobby lobby, ICommandContext context) => throw new Exception("Somehow 'None' got started.");

        public override string ToString() => "lobby_no_game";
    }


    public enum Gamemode : byte
    {
        None,
        classic,
        advanced,
        challenge,
        custom,
        evils,
        allany
    }
}