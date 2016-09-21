using System;

namespace Platinum.Validation
{
    /// <summary />
    internal static class Extensions
    {
        /// <summary />
        internal static bool HasValue( this object value )
        {
            if ( value == null )
                return false;

            if ( value is string )
            {
                string v = (string) value;
                return v.Length > 0;
            }
            
            if ( value is Array )
            {
                Array a = (Array) value;
                return a.Length > 0;
            }

            return true;
        }
    }
}
