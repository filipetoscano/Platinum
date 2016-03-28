using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace Platinum.Xml
{
    [SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X" )]
    public static class X
    {
        /*
         * AppendChild
         */
        [SuppressMessage( "Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode" )]
        public static XmlElement AppendChild( XmlElement parent, string elementName, string @namespace )
        {
            #region Validations

            if ( parent == null )
                throw new ArgumentNullException( "parent" );

            if ( elementName == null )
                throw new ArgumentNullException( "elementName" );

            if ( @namespace == null )
                throw new ArgumentNullException( "namespace" );

            #endregion

            XmlElement element = parent.OwnerDocument.CreateElement( elementName, @namespace );
            parent.AppendChild( element );

            return element;
        }

        [SuppressMessage( "Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode" )]
        public static XmlElement AppendChild( XmlElement parent, string elementName, string @namespace, string innerText )
        {
            #region Validations

            if ( parent == null )
                throw new ArgumentNullException( "parent" );

            if ( elementName == null )
                throw new ArgumentNullException( "elementName" );

            if ( @namespace == null )
                throw new ArgumentNullException( "namespace" );

            #endregion

            XmlElement element = AppendChild( parent, elementName, @namespace );

            if ( string.IsNullOrEmpty( innerText ) == false )
                element.InnerText = innerText;

            return element;
        }

        [SuppressMessage( "Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode" )]
        public static XmlElement AppendChild( XmlDocument document, string elementName, string @namespace )
        {
            #region Validations

            if ( document == null )
                throw new ArgumentNullException( "document" );

            if ( elementName == null )
                throw new ArgumentNullException( "elementName" );

            if ( @namespace == null )
                throw new ArgumentNullException( "namespace" );

            #endregion

            XmlElement element = document.CreateElement( elementName, @namespace );
            document.AppendChild( element );

            return element;
        }



        /*
         * SetAttribute
         */
        [SuppressMessage( "Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode" )]
        public static XmlAttribute SetAttribute( XmlElement element, string attributeName, string attributeValue )
        {
            #region Validations

            if ( element == null )
                throw new ArgumentNullException( "element" );

            if ( attributeName == null )
                throw new ArgumentNullException( "attributeName" );

            if ( attributeValue == null )
                throw new ArgumentNullException( "attributeValue" );

            #endregion

            XmlAttribute attr = element.Attributes[ attributeName, "" ];

            if ( attr == null )
            {
                attr = element.OwnerDocument.CreateAttribute( attributeName, "" );
                element.Attributes.Append( attr );
            }

            attr.Value = attributeValue;

            return attr;
        }

        [SuppressMessage( "Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode" )]
        public static XmlAttribute SetAttribute( XmlElement element, string attributeName, string attributeNamespace, string attributeValue )
        {
            #region Validations

            if ( element == null )
                throw new ArgumentNullException( "element" );

            if ( attributeName == null )
                throw new ArgumentNullException( "attributeName" );

            if ( attributeNamespace == null )
                throw new ArgumentNullException( "attributeNamespace" );

            if ( attributeValue == null )
                throw new ArgumentNullException( "attributeValue" );

            #endregion

            XmlAttribute attr = element.Attributes[ attributeName, attributeNamespace ];

            if ( attr == null )
            {
                attr = element.OwnerDocument.CreateAttribute( attributeName, attributeNamespace );
                element.Attributes.Append( attr );
            }

            attr.Value = attributeValue;

            return attr;
        }

    }
}

/* eof */