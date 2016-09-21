using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random character value.
    /// </summary>
    public class CharRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char c = s[ R.Next( s.Length ) ];

            return c;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            if ( value.Length == 0 )
                return null;

            return value[ 0 ];
        }
    }
}
