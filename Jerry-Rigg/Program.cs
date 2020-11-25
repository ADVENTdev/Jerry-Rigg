/*
 * Jerry Rigg
 * Author: ADVENT#0216
 * Original start date: 25 - 09 - 2020
 * C# start date: 21 - 11 - 2020
 * Most recent update: 22 - 11 - 2020
 */

// Import foreign modules, classes, and namespaces
/*
 * using System;
 * using Discord;
 * using System.IO;
 * using System.Text;
 * using System.Threading;
 * using System.Diagnostics;
 * using System.Threading.Tasks;
 * using System.Collections.Generic;
 * using System.Runtime.InteropServices;
 */

// Import namespaces, modules, and classes
using JerryRigg2.Start;
/*
 * using JerryRigg2.Common;
 * using JerryRigg2.Modules;
 * using JerryRigg2.Services;
 * using JerryRigg2.Common.Extensions;
 * using JerryRigg2.Services.Database;
 * using JerryRigg2.Services.Permission;
 */

namespace JerryRigg2
{
    class Program
    {
        public static JerryRigg JR { get; private set; }

        static void Main(string[] args)
        {

            JerryRigg JR = new JerryRigg();
            JR.RunAsync().GetAwaiter().GetResult();

        }

        public Program()
        {

        }
    }
}
