using System;
using System.Configuration;
using System.Globalization;

namespace Platinum.Configuration
{
    public static class AppConfiguration
    {
        /// <summary>
        /// Gets the value for the designated application configuration key.
        /// Key must be defined, or otherwise an exception is raised.
        /// </summary>
        /// <typeparam name="T">Type of value which should be returned.</typeparam>
        /// <param name="key">Name of configuration key.</param>
        /// <returns>Type-cast value.</returns>
        public static T Get<T>( string key )
        {
            #region Validations

            if ( key == null )
                throw new ArgumentNullException( nameof( key ) );

            #endregion

            string value = ConfigurationManager.AppSettings[ key ];

            if ( value == null )
                throw new ConfigurationException( ER.Get_Required, key );

            return ToValue<T>( key, value );
        }


        /// <summary>
        /// Gets the value for the designated application configuration key.
        /// If key is not defined, the default value is returned.
        /// </summary>
        /// <typeparam name="T">Type of value which should be returned.</typeparam>
        /// <param name="key">Name of configuration key.</param>
        /// <param name="default">Default value, if key is not found.</param>
        /// <returns>Type-cast value.</returns>
        public static T Get<T>( string key, T @default )
        {
            #region Validations

            if ( key == null )
                throw new ArgumentNullException( nameof( key ) );

            #endregion

            string value = ConfigurationManager.AppSettings[ key ];

            if ( value == null )
                return @default;

            return ToValue<T>( key, value );
        }


        private static T ToValue<T>( string key, string value )
        {
            #region Validations

            if ( key == null )
                throw new ArgumentNullException( nameof( key ) );

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            Type tt = typeof( T );
            T t;

            if ( tt.IsEnum == true )
            {
                try
                {
                    t = Enumerate.Parse<T>( value );
                }
                catch ( ActorException ex )
                {
                    throw new ConfigurationException( ER.Get_NotEnum, ex, key );
                }
            }
            else if ( tt == typeof( Guid ) )
            {
                try
                {
                    object o = new Guid( value );
                    t = (T) o;
                }
                catch ( FormatException ex )
                {
                    throw new ConfigurationException( ER.Get_NotGuid, ex, key, value );
                }
                catch ( OverflowException ex )
                {
                    throw new ConfigurationException( ER.Get_NotGuid, ex, key, value );
                }
            }
            else if ( tt == typeof( bool ) )
            {
                object o;

                if ( value == "true" || value == "True" || value == "1" )
                    o = true;
                else if ( value == "false" || value == "False" || value == "0" )
                    o = false;
                else
                    throw new ConfigurationException( ER.Get_NotBool, key, value );

                t = (T) o;
            }
            else
            {
                try
                {
                    t = (T) Convert.ChangeType( value, tt, CultureInfo.InvariantCulture );
                }
                catch ( InvalidCastException ex )
                {
                    throw new ConfigurationException( ER.Get_ChangeType, ex, key, tt.FullName, value );
                }
            }

            return t;
        }


        /// <summary>
        /// Retrieves a specified configuration section for the current application's
        /// default configuration. The configuration section must be defined in the
        /// application configuration file, or an exception will be raised.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">The configuration section path and name.</param>
        /// <returns>The type-cast configuration section object.</returns>
        public static T SectionGet<T>( string sectionName )
        {
            #region Validations

            if ( sectionName == null )
                throw new ArgumentNullException( nameof( sectionName ) );

            #endregion

            object s;

            try
            {
                s = ConfigurationManager.GetSection( sectionName );
            }
            catch ( ConfigurationErrorsException ex )
            {
                throw new ConfigurationException( ER.Section_Invalid, ex, sectionName );
            }

            if ( s == null )
                throw new ConfigurationException( ER.Section_NotDeclared, sectionName );

            if ( s.GetType().IsAssignableFrom( typeof( T ) ) == false )
                throw new ConfigurationException( ER.Section_NotExpectedType, sectionName, s.GetType().FullName, typeof( T ).FullName );

            return (T) s;
        }


        /// <summary>
        /// Gets the ConnectionStringsSection data for the current
        //  application's default configuration.
        /// </summary>
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }
    }
}

/* eof */