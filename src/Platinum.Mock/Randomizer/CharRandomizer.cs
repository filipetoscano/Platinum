using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random character value.
    /// </summary>
    public class CharRandomizer : IRandomizer
    {
        public object Random( string propertyName, Type type )
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char c = s[ R.Next( s.Length ) ];

            return c;
        }


        public object Parse( Type type, string value )
        {
            return null;
        }
    }
}
