using System;

namespace Platinum
{
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
            #region Validation

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            if ( value.Length == 0 )
                throw new ArgumentOutOfRangeException( nameof( value ) );

            #endregion

            T t;

            if ( typeof( T ).IsEnum == false )
                throw new CoreException( ER.Enumerate_TypeNotEnum, typeof( T ).FullName );

            try
            {
                t = (T) Enum.Parse( typeof( T ), value );
            }
            catch ( ArgumentException ex )
            {
                throw new CoreException( ER.Enumerate_Parse, ex, typeof( T ).FullName, value );
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
            #region Validation

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            if ( value.Length == 0 )
                throw new ArgumentOutOfRangeException( nameof( value ) );

            #endregion

            T t;

            if ( typeof( T ).IsEnum == false )
                throw new CoreException( ER.Enumerate_TypeNotEnum, typeof( T ).FullName );

            try
            {
                t = (T) Enum.Parse( typeof( T ), value, true );
            }
            catch ( ArgumentException ex )
            {
                throw new CoreException( ER.Enumerate_ParseCaseInsensitive, ex, typeof( T ).FullName, value );
            }

            return t;
        }
    }
}

/* eof */