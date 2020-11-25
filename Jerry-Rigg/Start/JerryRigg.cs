/*
 * Jerry Rigg
 * Author: ADVENT#0216
 * (Helper: @TheStachelFisch#0395)
 * Original start date: 25 - 09 - 2020
 * C# start date: 21 - 11 - 2020
 * Most recent update: 22 - 11 - 2020
 */

// Import libraries
using System;
using Discord;
using System.IO;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Discord.Commands;
using System.Threading;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

// Import foreign classes and modules
using JerryRigg2.Services;

namespace JerryRigg2.Start
{
    public class JerryRigg
    {

        private DiscordSocketClient _client;
        public static ServiceProvider Provider;

        public JerryRigg()
        {
            _client = new DiscordSocketClient();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
        }

        public async Task RunAsync()
        {

            using (Provider = ConfigureServices())
            {
                _client = Provider.GetRequiredService<DiscordSocketClient>();

                _client.Log += LogAsync;
                Provider.GetRequiredService<CommandService>().Log += LogAsync;

                // Read the file to attain the token
                var json = string.Empty;

                using (var fs = File.OpenRead("config.json"))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);

                var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

                // Set the token, start the bot
                await _client.LoginAsync(TokenType.Bot, configJson.token);
                await _client.StartAsync();

                await Provider.GetRequiredService<CommandHandler>().InitializeAsync();

                // Block the program until it is closed.
                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}

/*
using System;
using Discord;
using System.IO;
using System.Text;
using System.Timers;
using System.Net.Http;
using Newtonsoft.Json;
using Discord.Commands;
using System.Threading;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;


// Import foreign classes and modules
using JerryRigg2.Common;
using JerryRigg2.Modules;
using JerryRigg2.Services;
using JerryRigg2.Services.Random;

namespace JerryRigg2.Start
{
    public class JerryRigg
    {

        private DiscordSocketClient _client;
        public static ServiceProvider Provider;
        public DiscordShardedClient DiscordClient;
        public IServiceProvider Services;
        public CommandHandler CommandHandler;
        public StartupState State;
        private int StatusIndex;
        private Random rng;
        private bool Freeze;

        public JerryRigg()
        {
            _client = new DiscordSocketClient();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
        }

        public async Task RunAsync()
        {

            State = StartupState.DiscordInitializing;
            int RetryCount = 0;
            try
            {
                //Create discord client
                DiscordClient = new DiscordShardedClient(new DiscordSocketConfig
                {
                    MessageCacheSize = 50,
                    AlwaysDownloadUsers = true,
                    TotalShards = Config.ShardCount
                });

                //Setup events
                DiscordClient.Log += (LogMessage lmsg) => Utilities.Log(lmsg, null);
                DiscordClient.UserJoined += WelcomeMessage.SendMessage;
                if (Config.BotlistApiKey != null) //Check if enabled
                {
                    DiscordClient.JoinedGuild += (_) => DiscordBotlist.UpdateServerCount();
                    DiscordClient.LeftGuild += (_) => DiscordBotlist.UpdateServerCount();
                }

                //Setup command handler
                using (Provider = ConfigureServices())
                {
                    _client = Provider.GetRequiredService<DiscordSocketClient>();

                    _client.Log += LogAsync;
                    Provider.GetRequiredService<CommandService>().Log += LogAsync;

                    // Read the file to attain the token
                    var json = string.Empty;

                    using (var fs = File.OpenRead("config.json"))
                    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                        json = await sr.ReadToEndAsync().ConfigureAwait(false);

                    var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

                    // Set the token, start the bot
                    await _client.LoginAsync(TokenType.Bot, configJson.token);
                    await _client.StartAsync();

                    await Provider.GetRequiredService<CommandHandler>().InitializeAsync();

                    // Block the program until it is closed.
                    await Task.Delay(Timeout.Infinite);
                }

                //Setup services
                Services = new ServiceCollection()
                   .AddSingleton(DiscordClient)
                   .AddSingleton<CommandHandler>()
                   .BuildServiceProvider();
            }
            catch (Exception)
            {
                await Utilities.Log(new LogMessage(LogSeverity.Critical, "Start", "Failed to create discord client / register commands & events!"));
                Console.ReadLine();
                Environment.Exit(0);
            }
        //Connect to discord
        Reconnect:
            State = StartupState.DiscordLogin;
            try
            {
                //Login
                await DiscordClient.LoginAsync(TokenType.Bot, Config.BotToken);
                await DiscordClient.StartAsync();
                await DiscordClient.SetStatusAsync(UserStatus.Online);
                State = StartupState.Ready;
                //Set it to 0 to reset reconnecting
                RetryCount = 0;
                while (true)
                {
                    while (State != StartupState.Ready) await Task.Delay(1000);
                    if (Console.ReadLine().ToLower() == "quit")
                    {
                        await Program.Quit();
                        Program.ExitHandled = true;
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception) //Internet disconnect?
            {
                if (RetryCount != 3)
                    RetryCount++;
                await Utilities.Log(new LogMessage(LogSeverity.Error, "Main", $"Failed to connect... Retring in {10 * RetryCount} seconds"));
                await Task.Delay(1000 * 10 * RetryCount);

                //Reset discord client
                try
                {
                    await DiscordClient.LogoutAsync();
                }
                catch (Exception) { }
                try
                {
                    await DiscordClient.StopAsync();
                }
                catch (Exception) { }

                goto Reconnect;
            }
        }
        public async Task Shutdown()
        {
            await DiscordClient.LogoutAsync();
            while (DiscordClient.LoginState != LoginState.LoggedOut) await Task.Delay(10);
            await DiscordClient.StopAsync();
            DiscordClient.Dispose();
        }


        public List<(string msg, ActivityType type)> StatusMessages = new List<(string, ActivityType)>
        {
            ( "with @GUILDS@ server's life | jr!help", ActivityType.Playing ),
            ( "jr!help for help", ActivityType.Playing ),
            ( "with @OWNER@ | jr!help", ActivityType.Playing ),
            ( "support me on Patreon | jr!donate", ActivityType.Playing ),
            ( "you | jr!help", ActivityType.Watching ),
            ( "Vote for Jerry Rigg on discordbots.org | jr!vote", ActivityType.Playing ),
        };

        public void NextPlayingStatus()
        {
            Task.Run(async () =>
            {
                while (DiscordClient == null || DiscordClient.LoginState != LoginState.LoggedIn) { }

                if (rng == null)
                    rng = new Random();

                Get:
                if (!Freeze)
                {
                    var i = rng.Next(0, StatusMessages.Count);
                    if (i == StatusIndex)
                        goto Get;
                    StatusIndex = i;
                }
                var (msg, type) = StatusMessages[StatusIndex];
                var application = await DiscordClient.GetApplicationInfoAsync();
                await DiscordClient.SetGameAsync(msg.Replace("@GUILDS@", $"{DiscordClient.Guilds.Count}").Replace("@OWNER@", $"{application.Owner}"), null, type);
                return Task.CompletedTask;
            });
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}*/