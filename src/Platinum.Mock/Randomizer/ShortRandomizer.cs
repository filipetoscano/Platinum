using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random short value.
    /// </summary>
    public class ShortRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            short s = (short) R.Next( 32767 );

            return s;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return short.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
