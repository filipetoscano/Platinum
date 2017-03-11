using System;

namespace Platinum.Reflection
{
    /// <summary />
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


        /// <summary>
        /// Gets the base type of a nullable type.
        /// </summary>
        /// <param name="type">Runtime type.</param>
        /// <returns>Base type, of nullable type.</returns>
        public static Type GetNullableType( this Type type )
        {
            if ( IsNullable( type ) == false )
                throw new ArgumentOutOfRangeException( nameof( type ), "Type is not nullable." );

            return type.GetGenericArguments()[ 0 ];
        }
    }
}
