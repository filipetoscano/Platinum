using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Platinum.Xml
{
    /// <summary>
    /// Serializes and deserializes objects into and from XML string.
    /// Augments the functionality of <see cref="XmlSerializer" /> by
    /// caching the (rather expensive) instances of <see cref="XmlSerializer"/>.
    /// </summary>
    public static class XmlSerializerCache
    {
        private static Dictionary<Type, XmlSerializer> _cache = new Dictionary<Type, XmlSerializer>();


        /// <summary>
        /// Serializes the specified <see cref="object" /> into XML.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <returns>The object serialized into XML.</returns>
        public static string Serialize( object value )
        {
            #region Validations

            if ( value == null )
                throw new ArgumentNullException( nameof( value ) );

            #endregion

            Type t = value.GetType();
            XmlSerializer ser = BuildSerializer( t );


            /*
             * 
             */
            StringBuilder sb = new StringBuilder();

            using ( XmlWriter writer = XmlWriter.Create( sb ) )
            {
                ser.Serialize( writer, value );
            }

            return sb.ToString();
        }


        /// <summary>
        /// Deserializes the XML document described in the xml string.
        /// </summary>
        /// <typeparam name="T">Type to deserialize.</typeparam>
        /// <param name="xml">Serialized representation of T.</param>
        /// <returns>Instance of <see cref="T"/>.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
        public static T Deserialize<T>( string xml )
        {
            #region Validations

            if ( xml == null )
                throw new ArgumentNullException( "xml" );

            #endregion

            Type t = typeof( T );
            XmlSerializer ser = BuildSerializer( t );


            /*
             * 
             */
            T instance;

            using ( StringReader sr = new StringReader( xml ) )
            {
                using ( XmlReader reader = XmlReader.Create( sr ) )
                {
                    instance = (T) ser.Deserialize( reader );
                }
            }

            return instance;
        }


        /// <summary>
        /// Deserializes the XML document described in the xml string,
        /// validating it against the specified <see cref="XmlSchema" />.
        /// </summary>
        /// <typeparam name="T">Type to deserialize.</typeparam>
        /// <param name="schema">Schema to validate against.</param>
        /// <param name="xml">Serialized representation of T.</param>
        /// <returns>Instance of <see cref="T"/>.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA2202:Do not dispose objects multiple times" )]
        public static T Deserialize<T>( XmlSchema schema, string xml )
        {
            #region Validations

            if ( schema == null )
                throw new ArgumentNullException( nameof( schema ) );

            if ( xml == null )
                throw new ArgumentNullException( nameof( xml ) );

            #endregion

            Type t = typeof( T );
            XmlSerializer ser = BuildSerializer( t );


            /*
             * 
             */
            List<Exception> errors = new List<Exception>();

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add( schema );

            var settings = new XmlReaderSettings
            {
                Schemas = schemas,
                ValidationType = ValidationType.Schema,
                ValidationFlags =
                    XmlSchemaValidationFlags.ProcessIdentityConstraints |
                    XmlSchemaValidationFlags.ReportValidationWarnings
            };

            settings.ValidationEventHandler +=
                delegate( object sender, ValidationEventArgs args )
                {
                    if ( args.Exception != null )
                        errors.Add( args.Exception );
                };


            /*
             * 
             */
            T instance;

            using ( StringReader sr = new StringReader( xml ) )
            {
                using ( XmlReader reader = XmlReader.Create( sr, settings ) )
                {
                    instance = (T) ser.Deserialize( reader );
                }
            }

            if ( errors.Count > 0 )
            {
                AggregateException ae = new AggregateException( errors );
                throw new XmlException( ER.XmlSerializer_Schema, ae );
            }

            return instance;
        }


        private static XmlSerializer BuildSerializer( Type type )
        {
            #region Validations

            if ( type == null )
                throw new ArgumentNullException( nameof( type ) );

            #endregion

            if ( _cache.ContainsKey( type ) == false )
            {
                lock ( _cache )
                {
                    if ( _cache.ContainsKey( type ) == false )
                    {
                        XmlSerializer serializer = new XmlSerializer( type );
                        _cache.Add( type, serializer );
                    }
                }
            }

            return _cache[ type ];
        }
    }
}

/* eof */