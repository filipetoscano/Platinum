using System;

namespace Platinum
{
    /// <summary />
    public static class PreciseDateTime
    {
        private static Lazy<bool> _isPrecise = new Lazy<bool>( () =>
        {
            try
            {
                PreciseTimeGet();
                return true;
            }
            catch ( EntryPointNotFoundException )
            {
                return false;
            }
        } );


        /// <summary>
        /// Gets a <see cref="DateTime" /> object that is set to the current date and time on this
        /// computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public static DateTime UtcNow
        {
            get
            {
                if ( _isPrecise.Value == false )
                    return DateTime.UtcNow;
                else
                    return PreciseTimeGet();
            }
        }


        /// <summary>
        /// Gets whether the timestamps returned by the current class are precise
        /// or not.
        /// </summary>
        public static bool IsPrecise
        {
            get { return _isPrecise.Value; }
        }


        /// <summary />
        private static DateTime PreciseTimeGet()
        {
            long preciseTime;
            NativeMethods.GetSystemTimePreciseAsFileTime( out preciseTime );

            return DateTime.FromFileTimeUtc( preciseTime );
        }
    }
}
