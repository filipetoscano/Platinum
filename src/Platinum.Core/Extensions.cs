using System.Collections.Generic;

namespace Platinum
{
    /// <summary>
    public static class Extensions
    {
        /// <summary>
        public static T Mirror<T>( this object obj ) where T : class
        {
            return Platinum.Mirror.To<T>( obj );
        }


        /// <summary>
        public static void AddRange<T>( this ICollection<T> target, IEnumerable<T> items )
        {
            foreach ( var item in items )
            {
                target.Add( item );
            }
        }
    }
}
