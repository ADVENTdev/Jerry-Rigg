/*
 * Jerry Rigg
 * Author: ADVENT#0216
 * Original start date: 25 - 09 - 2020
 * C# start date: 21 - 11 - 2020
 * Most recent update: 22 - 11 - 2020
 */

// Import foreign libraries
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

// Import foreign namespaces, modules, and classes
using JerryRigg2.Start;
using JerryRigg2.Services;
using JerryRigg2.Modules;

namespace JerryRigg2.Start
{
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string token { get; private set; }
        [JsonProperty("prefix")]
        public string prefix { get; private set; }
    }
}