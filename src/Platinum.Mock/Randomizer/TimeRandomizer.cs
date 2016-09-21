using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random time value.
    /// </summary>
    public class TimeRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            DateTime d = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
            d = d.AddMinutes( R.NextDouble( 24 * 60 ) );

            return d;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            DateTime v1 = DateTime.ParseExact( value, "HH:mm:ss", CultureInfo.InvariantCulture );
            DateTime v2 = new DateTime( 1970, 1, 1, v1.Hour, v1.Minute, v1.Second, DateTimeKind.Utc );

            return v2;
        }
    }


    /// <summary>
    /// Fake class, so that we can distinguish DateTime with only
    /// time component.
    /// </summary>
    internal class Time
    {
    }
}
