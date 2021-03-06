﻿using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random string value.
    /// </summary>
    public class StringRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            return "STRING #" + R.Next( 100 );
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return value;
        }
    }
}
