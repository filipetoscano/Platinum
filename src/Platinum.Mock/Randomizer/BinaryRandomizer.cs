using System;

namespace Platinum.Mock.Randomizer
{
    /// <summary>
    /// Generates a random value for binary content, which is
    /// represented through the .NET byte[] type.
    /// </summary>
    public class BinaryRandomizer : IRandomizer
    {
        const string Base64Marker = "base64:";

        /// <summary />
        public object Random( Type type )
        {
            return null;
        }


        /// <summary />
        public object Parse( Type type, string value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            if ( value.StartsWith( Base64Marker, StringComparison.Ordinal ) == true )
                return Convert.FromBase64String( value.Substring( Base64Marker.Length ) );

            // TODO: load from FS

            return null;
        }
    }
}
