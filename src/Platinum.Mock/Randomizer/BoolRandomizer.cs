using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random boolean value.
    /// </summary>
    public class BoolRandomizer : IRandomizer
    {
        /// <summary />
        public object Random( Type type )
        {
            bool b = R.NextDouble() > 0.5;

            return b;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            if ( value == "true" )
                return true;

            if ( value == "1" )
                return true;

            if ( value == "false" )
                return false;

            if ( value == "0" )
                return false;

            throw new ArgumentOutOfRangeException( nameof( value ), value, "Invalid xsd:boolean value." );
        }
    }
}
