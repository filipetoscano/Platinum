using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random value for binary content, which is
    /// represented through the .NET byte[] type.
    /// </summary>
    public class BinaryRandomizer : IRandomizer
    {
        public object Random( string propertyName, Type type )
        {
            return null;
        }


        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            return null;
        }
    }
}
