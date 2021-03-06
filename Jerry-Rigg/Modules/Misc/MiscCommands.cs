﻿/*
 * Jerry Rigg - Commands
 *      > Miscellaneous Commands
 * Start date: 22 - 11 - 2020
 * Last update: 22 - 11 - 2020
 */

// Import libraries and extensions
using System;
using Discord;
using System.IO;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

// Import foreign classes, modules, and namespaces
using JerryRigg2.Start;
using JerryRigg2.Services;
using JerryRigg2.Modules;

namespace JerryRigg2.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {

        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
        {
            var pingtime = PingTest.ComplexPing();
            ReplyAsync("pong! " + pingtime + "ms");
            return null;
        }
    }
}

class PingTest
{
    public static string ComplexPing()
    {
        Ping pingSender = new Ping();

        // Create a buffer of 32 bytes of data to be transmitted.
        string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] buffer = Encoding.ASCII.GetBytes(data);

        // Wait 10 seconds for a reply.
        int timeout = 10000;

        // Set options for transmission:
        // The data can go through 64 gateways or routers
        // before it is destroyed, and the data packet
        // cannot be fragmented.
        PingOptions options = new PingOptions(64, true);

        // Send the request.
        PingReply reply = pingSender.Send("www.discord.com", timeout, buffer, options);

        if (reply.Status == IPStatus.Success)
        {
            var x = reply.RoundtripTime;

            Console.WriteLine("Address: {0}", reply.Address.ToString());
            Console.WriteLine("RoundTrip time: {0}", x);
            Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
            Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
            Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);

            return x.ToString();
        }
        else
        {
            Console.WriteLine(reply.Status);
            return "(Outward ping unsuccessful.)";
        }
    }
}