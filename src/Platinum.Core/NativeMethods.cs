using System.Runtime.InteropServices;

namespace Platinum
{
    /// <summary />
    internal static class NativeMethods
    {
        /// <summary>
        /// Higher precision time.
        /// </summary>
        /// <param name="filetime">Timestamp.</param>
        [DllImport( "Kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetSystemTimePreciseAsFileTime" )]
        internal static extern void GetSystemTimePreciseAsFileTime( out long filetime );
    }
}
