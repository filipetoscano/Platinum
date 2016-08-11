﻿using System;
using System.Globalization;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random integer value.
    /// </summary>
    public class IntegerRandomizer : IRandomizer
    {
        public object Random( string propertyName, Type type )
        {
            int i = R.Next( 32767 );

            return i;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return int.Parse( value, CultureInfo.InvariantCulture );
        }
    }
}
