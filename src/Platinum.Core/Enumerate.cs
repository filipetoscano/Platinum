using System;
using Platinum.Reflection;

namespace Platinum
{
    /// <summary />
    public class Enumerate
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static T Parse<T>( string value )
        {
            Type tt = typeof( T );
            T t;

            if ( tt.IsNullable() == true )
            {
                tt = Nullable.GetUnderlyingType( tt );

                if ( tt.IsEnum == false )
                    throw new CoreException( ER.Enumerate_TypeNotEnum, tt.FullName );

                if ( value == null )
                    return default( T );
            }
            else
            {
                if ( tt.IsEnum == false )
                    throw new CoreException( ER.Enumerate_TypeNotEnum, tt.FullName );

                if ( value == null )
                    throw new ArgumentNullException( nameof( value ) );
            }

            try
            {
                t = (T) Enum.Parse( tt, value );
            }
            catch ( ArgumentException ex )
            {
                throw new CoreException( ER.Enumerate_Parse, ex, tt.FullName, value );
            }

            return t;
        }


        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object, using a
        /// case-insensitive operation.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type T whose value is represented by value.</returns>
        public static T ParseInsensitive<T>( string value )
        {
            Type tt = typeof( T );
            T t;

            if ( tt.IsNullable() == true )
            {
                tt = Nullable.GetUnderlyingType( tt );

                if ( tt.IsEnum == false )
                    throw new CoreException( ER.Enumerate_TypeNotEnum, tt.FullName );

                if ( value == null )
                    return default( T );
            }
            else
            {
                if ( tt.IsEnum == false )
                    throw new CoreException( ER.Enumerate_TypeNotEnum, tt.FullName );

                if ( value == null )
                    throw new ArgumentNullException( nameof( value ) );
            }

            try
            {
                t = (T) Enum.Parse( tt, value, true );
            }
            catch ( ArgumentException ex )
            {
                throw new CoreException( ER.Enumerate_ParseCaseInsensitive, ex, tt, value );
            }

            return t;
        }
    }
}

/* eof */