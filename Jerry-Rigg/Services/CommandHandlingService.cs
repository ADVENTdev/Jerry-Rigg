/*
 * Jerry Rigg
 * Author: @ADVENT#0216
 * (Helper: @TheStachelFisch#0395)
 * Start date: 22 - 11 - 2020
 * Last update: 22 - 11 - 2020
 */

// Import libraries and extensions
using System;
using Discord;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

// Import foreign classes, modules, and namespaces
using JerryRigg2.Start;
using JerryRigg2.Modules;

namespace JerryRigg2.Services
{

    public class CommandHandler
    {
        internal static string DEFAULT_PREFIX;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync() 
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services); 
        }

        private async Task MessageReceivedAsync(SocketMessage arg)
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var DEFAULT_PREFIX = configJson.prefix;

            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(DEFAULT_PREFIX, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine("Error happened while executing Command: " + result.ErrorReason + " ServerId: " + context.Guild.Id);
                }
            }
        }
    }
}