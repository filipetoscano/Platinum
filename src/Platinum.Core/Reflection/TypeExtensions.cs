using System;

namespace Platinum.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets whether the given type is a custom class.
        /// </summary>
        /// <param name="type">Runtime type.</param>
        /// <returns>True if type is a custom class, false otherwise.</returns>
        public static bool IsCustomClass( this Type type )
        {
            if ( type.IsClass == false )
                return false;

            if ( type == typeof( string ) )
                return false;

            return true;
        }


        /// <summary>
        /// Gets whether the given type is nullable.
        /// </summary>
        /// <param name="type">Runtime type.</param>
        /// <returns>True if type is nullable, false otherwise.</returns>
        public static bool IsNullable( this Type type )
        {
            return type.IsGenericType == true
                && type.GetGenericTypeDefinition() == typeof( Nullable<> );
        }
    }
}
