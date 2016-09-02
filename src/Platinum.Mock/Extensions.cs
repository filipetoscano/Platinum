using Platinum.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using KvConfig = Platinum.Configuration.KeyValueConfigurationElement;

namespace Platinum.Mock
{
    internal static class Extensions
    {
        internal static T Get<T>( this Dictionary<string, string> dict, string key )
        {
            if ( dict.ContainsKey( key ) == false )
                return default( T );

            return (T) Convert.ChangeType( dict[ key ], typeof( T ), CultureInfo.InvariantCulture );
        }


        internal static T Get<T>( this Dictionary<string, string> dict, string key, T @default )
        {
            if ( dict.ContainsKey( key ) == false )
                return @default;

            return (T) Convert.ChangeType( dict[ key ], typeof( T ), CultureInfo.InvariantCulture );
        }


        internal static Dictionary<string,string> AsDictionary( this ConfigurationElementCollection<KvConfig> config )
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            foreach ( var kv in config )
            {
                d[ kv.Key ] = kv.Value;
            }

            return d;
        }
    }
}
