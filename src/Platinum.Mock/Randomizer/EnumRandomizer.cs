using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random enumerate value.
    /// </summary>
    public class EnumRandomizer : IRandomizer
    {
        public object Random( Type type )
        {
            Array values = Enum.GetValues( type );

            return values.GetValue( R.Next( values.Length ) );
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return Enum.Parse( type, value );
        }
    }
}
