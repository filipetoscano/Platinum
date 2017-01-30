using System.Reflection;

namespace Platinum.Reflection
{
    /// <summary />
    public static class EntryAssembly
    {
        /// <summary />
        public static Assembly GetEntryAssembly()
        {
            return EntryAssemblyAttribute.GetEntryAssembly();
        }
    }
}
