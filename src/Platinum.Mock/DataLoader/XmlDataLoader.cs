using Platinum.Resolver;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Platinum.Mock.DataLoader
{
    /// <summary />
    public class XmlDataLoader : IDataLoader
    {
        /// <summary />
        public Data Load( Dictionary<string, string> settings )
        {
            #region Validations

            if ( settings == null )
                throw new ArgumentNullException( nameof( settings ) );

            #endregion


            /*
             * 
             */
            Data d = new Data();
            d.Functions = new Dictionary<string, Dictionary<string, string>>();
            d.Sets = new Dictionary<string, List<string>>();


            /*
             * 
             */
            UrlResolver resolver = new UrlResolver();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            XmlNamespaceManager manager = new XmlNamespaceManager( new NameTable() );
            manager.AddNamespace( "m", "urn:platinum/mock" );


            /*
             * 
             */
            foreach ( string key in settings.Keys )
            {
                if ( key.StartsWith( "file" ) == false )
                    continue;

                string value = settings[ key ];
                XmlDocument doc = new XmlDocument();

                using ( XmlReader xr = XmlReader.Create( value, xrs ) )
                {
                    doc.Load( xr );
                }


                /*
                 * Sets
                 */
                foreach ( XmlElement setElem in doc.SelectNodes( " /m:data/m:set ", manager ) )
                {
                    List<string> values = new List<string>();

                    foreach ( XmlElement vElem in setElem.SelectNodes( " m:v ", manager ) )
                    {
                        values.Add( vElem.InnerText );
                    }

                    d.Sets.Add( setElem.Attributes[ "name" ].Value, values );
                }


                /*
                 * Functions
                 */
                foreach ( XmlElement setElem in doc.SelectNodes( " /m:data/m:function ", manager ) )
                {
                    Dictionary<string, string> nvc = new Dictionary<string, string>();

                    foreach ( XmlElement elem in setElem.SelectNodes( " m:* ", manager ) )
                    {
                        nvc[ elem.LocalName ] = elem.InnerText;
                    }

                    d.Functions.Add( setElem.Attributes[ "name" ].Value, nvc );
                }
            }

            return d;
        }
    }
}
