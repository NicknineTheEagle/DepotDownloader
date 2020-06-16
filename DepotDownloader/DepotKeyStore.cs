using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DepotDownloader
{
    static class DepotKeyStore
    {
        private static Dictionary<uint, byte[]> depotKeysCache = new Dictionary<uint, byte[]>();

        public static void LoadFromFile( string filename )
        {
            string[] lines = File.ReadAllLines( filename );

            foreach ( string line in lines )
            {
                if ( string.IsNullOrWhiteSpace( line ) )
                    continue;

                string[] split = line.Split( ';' );

                if ( split.Length != 2 )
                {
                    throw new FormatException( String.Format( "Invalid depot key line: {0}", line ) );
                }

                if ( ContainsKey( uint.Parse( split[0] ) ) )
                {
                    Console.WriteLine( "Warning: Duplicate key line for depot {0}", uint.Parse( split[0] ) );
                    continue;
                }

                depotKeysCache.Add( uint.Parse( split[0] ), StringToByteArray( split[1] ) );
            }
        }

        private static byte[] StringToByteArray( string hex )
        {
            return Enumerable.Range( 0, hex.Length )
                             .Where( x => x % 2 == 0 )
                             .Select( x => Convert.ToByte( hex.Substring( x, 2 ), 16 ) )
                             .ToArray();
        }

        public static bool ContainsKey( uint depotId )
        {
            return depotKeysCache.ContainsKey( depotId );
        }

        public static byte[] Get( uint depotId )
        {
            return depotKeysCache[depotId];
        }
    }
}
