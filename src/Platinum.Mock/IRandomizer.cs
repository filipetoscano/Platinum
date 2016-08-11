using System;

namespace Platinum.Mock
{
    public interface IRandomizer
    {
        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property being randomized.
        /// </param>
        /// <param name="type">
        /// Type of the property being randomized.
        /// </param>
        /// <returns>
        /// Randomized value, or null if no value should be set.
        /// </returns>
        object Random( string propertyName, Type type );

        /// <summary>
        /// Parses a data-set value, converting it to the .NET native
        /// type.
        /// </summary>
        /// <param name="type">
        /// Type of the property being randomized.
        /// </param>
        /// <param name="value">
        /// Data-set value.
        /// </param>
        /// <returns>
        /// .NET native value.
        /// </returns>
        object Parse( Type type, string value );
    }
}
