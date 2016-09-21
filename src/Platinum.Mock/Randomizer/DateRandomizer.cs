using System;
using System.Collections.Generic;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random date value.
    /// </summary>
    public class DateRandomizer : IRandomizer, IMockFunction
    {
        /// <summary />
        public object Random( Type type )
        {
            DateTime d = DateTime.UtcNow;
            d = d.AddMinutes( R.NextDouble( 10 * 24 * 60 ) );

            return d.Date;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            DateTime d = DateTime.ParseExact( value, "yyyy-MM-dd", CultureInfo.InvariantCulture );
            return DateTime.SpecifyKind( d, DateTimeKind.Utc );
        }


        /// <summary />
        public object Random( Dictionary<string,string> settings )
        {
            #region Validations

            if ( settings == null )
                throw new ArgumentNullException( nameof( settings ) );

            #endregion


            /*
             * 
             */
            DateTime today = DateTime.UtcNow;

            int yearMin;
            int yearMax;

            if ( settings.Get<string>( "absolute" ) == "true" )
            {
                yearMin = settings.Get<int>( "yearFrom", today.Year );
                yearMax = settings.Get<int>( "yearTo", today.Year );

                if ( yearMax <= yearMin )
                    throw new MockException( ER.DateRandomizer_YearAbsRange, yearMin, yearMax );
            }
            else
            {
                yearMin = settings.Get<int>( "yearFrom" ) + today.Year;
                yearMax = settings.Get<int>( "yearTo" ) + today.Year;

                if ( yearMax <= yearMin )
                    throw new MockException( ER.DateRandomizer_YearRelRange, yearMin, yearMax );
            }


            /*
             * 
             */
            double yearDelta = R.NextDouble( yearMin, yearMax );
            double daysDelta = yearDelta * 365;

            DateTime d = new DateTime( 1, 1, 1, 0, 0, 0, DateTimeKind.Utc );
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
