using System.Collections.Generic;

namespace Platinum.Mock
{
    /// <summary>
    /// Describes a function which can generate mock values,
    /// based on the settings that it receives.
    /// </summary>
    public interface IMockFunction
    {
        /// <summary>
        /// Generates a random value, based on the received settings.
        /// </summary>
        /// <param name="settings">
        /// Settings bag.
        /// </param>
        /// <returns>
        /// Random value.
        /// </returns>
        object Random( Dictionary<string,string> settings );
    }
}
