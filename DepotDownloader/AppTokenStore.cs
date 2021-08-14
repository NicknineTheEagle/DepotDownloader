// This file is subject to the terms and conditions defined
// in file 'LICENSE', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.IO;

namespace DepotDownloader
{
    static class AppTokenStore
    {
        private static readonly Dictionary<uint, ulong> appTokens = new Dictionary<uint, ulong>();

        public static void LoadFromFile(string filename)
        {
            var lines = File.ReadAllLines(filename);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var split = line.Split(';');

                if (split.Length != 2)
                {
                    throw new FormatException(String.Format("Invalid app token line: {0}", line));
                }

                if (ContainsKey(uint.Parse(split[0])))
                {
                    Console.WriteLine("Warning: Duplicate token line for app {0}", uint.Parse(split[0]));
                    continue;
                }

                appTokens.Add(uint.Parse(split[0]), ulong.Parse(split[1]));
            }
        }

        public static bool ContainsKey(uint appId)
        {
            return appTokens.ContainsKey(appId);
        }

        public static ulong Get(uint appId)
        {
            return appTokens.ContainsKey(appId) ? appTokens[appId] : 0;
        }
    }
}
