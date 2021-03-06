﻿using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random double value.
    /// </summary>
    public class DoubleRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            double d = R.NextDouble( 100000 );

            return d;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return double.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
