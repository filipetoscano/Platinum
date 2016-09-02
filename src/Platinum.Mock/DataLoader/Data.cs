using System.Collections.Generic;

namespace Platinum.Mock.DataLoader
{
    public class Data
    {
        /// <summary>
        /// List of functions.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Functions { get; set; }

        /// <summary>
        /// List of sets.
        /// </summary>
        public Dictionary<string, List<string>> Sets { get; set; }
    }
}
