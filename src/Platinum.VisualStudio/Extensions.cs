using System;
using System.Xml;

namespace Platinum.VisualStudio
{
    /// <summary />
    internal static class Extensions
    {
        /// <summary />
        internal static XmlElement SelectSingle( this XmlNode node, string xpath, XmlNamespaceManager manager = null )
        {
            #region Validations

            if ( node == null )
                throw new ArgumentNullException( nameof( node ) );

            if ( xpath == null )
                throw new ArgumentNullException( nameof( xpath ) );

            #endregion

            return (XmlElement) node.SelectSingleNode( xpath, manager );
        }


        /// <summary />
        internal static XmlElement Append( this XmlDocument document, string name, string namespaceURI = null )
        {
            #region Validations

            if ( document == null )
                throw new ArgumentNullException( nameof( document ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            XmlElement element = document.CreateElement( name, namespaceURI );
            document.AppendChild( element );

            return element;
        }


        /// <summary />
        internal static XmlElement Append( this XmlElement parent, string name, string namespaceURI = null )
        {
            #region Validations

            if ( parent == null )
                throw new ArgumentNullException( nameof( parent ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            XmlElement element = parent.OwnerDocument.CreateElement( name, namespaceURI );
            parent.AppendChild( element );

            return element;
        }


        /// <summary />
        internal static XmlElement Prepend( this XmlElement parent, string name, string namespaceURI = null )
        {
            #region Validations

            if ( parent == null )
                throw new ArgumentNullException( nameof( parent ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            XmlElement element = parent.OwnerDocument.CreateElement( name, namespaceURI );

            if ( parent.HasChildNodes == false )
                parent.AppendChild( element );
            else
                parent.InsertBefore( element, parent.FirstChild );

            return element;
        }


        /// <summary />
        internal static XmlElement Attribute( this XmlElement element, string name, string value )
        {
            #region Validations

            if ( element == null )
                throw new ArgumentNullException( nameof( element ) );

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion

            XmlAttribute attr = element.OwnerDocument.CreateAttribute( name );
            attr.Value = value;

            element.Attributes.Append( attr );

            return element;
        }
    }
}
