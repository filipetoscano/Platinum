﻿using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random decimal value.
    /// </summary>
    public class DecimalRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            decimal d = (decimal) R.NextDouble( 100000 );

            return d;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return decimal.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
