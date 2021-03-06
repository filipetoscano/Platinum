﻿using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random date and time value.
    /// </summary>
    public class DateTimeRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            DateTime d = DateTime.UtcNow;
            d = d.AddMinutes( R.NextDouble( 10 * 24 * 60 ) );

            return d;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            DateTime d = DateTime.ParseExact( value, "s", CultureInfo.InvariantCulture );
            return DateTime.SpecifyKind( d, DateTimeKind.Utc );
        }
    }
}
