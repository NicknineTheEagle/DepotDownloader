// This file is subject to the terms and conditions defined
// in file 'LICENSE', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepotDownloader
{
    static class DepotKeyStore
    {
        private static readonly Dictionary<uint, byte[]> depotKeysCache = new Dictionary<uint, byte[]>();

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
                    throw new FormatException(String.Format("Invalid depot key line: {0}", line));
                }

                if (ContainsKey(uint.Parse(split[0])))
                {
                    Console.WriteLine("Warning: Duplicate key line for depot {0}", uint.Parse(split[0]));
                    continue;
                }

                depotKeysCache.Add(uint.Parse(split[0]), StringToByteArray(split[1]));
            }
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static bool ContainsKey(uint depotId)
        {
            return depotKeysCache.ContainsKey(depotId);
        }

        public static byte[] Get(uint depotId)
        {
            return depotKeysCache[depotId];
        }
    }
}
