using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random long value.
    /// </summary>
    public class LongRandomizer : IRandomizer
    {
        public object Random( string propertyName, Type type )
        {
            long s = (long) R.Next( 32767 );

            return s;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return long.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
