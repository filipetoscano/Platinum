using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random byte value.
    /// </summary>
    public class ByteRandomizer : IRandomizer
    {
        public object Random( Type type )
        {
            byte b = (byte) R.Next( 255 );

            return b;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return byte.Parse( value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture );
        }
    }
}
