using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random float value.
    /// </summary>
    public class SingleRandomizer : IRandomizer
    {
        public object Random( Type type )
        {
            float f = (float) R.NextDouble( 100000 );

            return f;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return float.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
