using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random time value.
    /// </summary>
    public class TimeRandomizer : IRandomizer
    {
        public object Random( string propertyName, Type type )
        {
            DateTime d = new DateTime( 1, 1, 1, 0, 0, 0, DateTimeKind.Utc );
            d = d.AddMinutes( R.NextDouble( 24 * 60 ) );

            return d;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return DateTime.Parse( "hh:mm:ss", CultureInfo.InvariantCulture );
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
