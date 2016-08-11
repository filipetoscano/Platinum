using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random date value.
    /// </summary>
    public class DateRandomizer : IRandomizer, IMockFunction
    {
        public object Random( string propertyName, Type type )
        {
            DateTime d = DateTime.UtcNow;
            d = d.AddMinutes( R.NextDouble( 10 * 24 * 60 ) );

            return d.Date;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return DateTime.ParseExact( value, "yyyy-MM-dd", CultureInfo.InvariantCulture );
        }


        public object Random( NameValueCollection settings )
        {
            #region Validations

            if ( settings == null )
                throw new ArgumentNullException( nameof( settings ) );

            #endregion

            int yyMin = 0;
            int yyMax = 0;

            double yearDelta = R.NextDouble( yyMin, yyMax );
            double daysDelta = yearDelta * 365;

            DateTime d = DateTime.UtcNow;
            d = d.AddDays( daysDelta );

            return d.Date;
        }
    }


    /// <summary>
    /// Fake class, so that we can distinguish DateTime with only
    /// date component.
    /// </summary>
    internal class Date
    {
    }
}
