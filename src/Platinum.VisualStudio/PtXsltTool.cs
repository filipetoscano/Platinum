﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Applies an XSLT, generating C# as 'code-behind'.
    /// </summary>
    public class PtXsltTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            /*
             * 
             */
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml( args.Content );
            }
            catch ( XmlException ex )
            {
                throw new ToolException( "File is not a valid Xml document", ex );
            }

            XPathNavigator xpNav = doc.DocumentElement.CreateNavigator();


            /*
             * 
             */
            XmlProcessingInstruction pi = (XmlProcessingInstruction) doc.SelectSingleNode( " processing-instruction( \"codebehind\" ) " );

            if ( pi == null )
                throw new ToolException( "Processing instruction <?codebehind?> not found" );


            /*
             * 
             */
            Regex regex = new Regex( "transformation=['\"](?<xslt>.*?)['\"]" );

            Match m = regex.Match( pi.Value );

            if ( m.Success == false )
                throw new ToolException( "Processing instruction <?codebehind?> does not contain @transformation attribute" );


            /*
             * 
             */
            FileInfo inputFile = new FileInfo( args.FileName );

            string xsltRaw = m.Groups[ "xslt" ].Value;
            string xslt = Path.Combine( inputFile.DirectoryName, xsltRaw );
            string rawName = inputFile.Name.Substring( 0, inputFile.Name.Length - inputFile.Extension.Length );


            /*
             * 
             */
            Uri fileUri = new Uri( inputFile.FullName );
            Uri directoryUri = new Uri( inputFile.DirectoryName );

            XsltArgumentList xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam( "ToolVersion", "", Assembly.GetExecutingAssembly().GetName( false ).Version.ToString( 4 ) );
            xsltArgs.AddParam( "FileName", "", rawName );
            xsltArgs.AddParam( "FullFileName", "", inputFile.FullName );
            xsltArgs.AddParam( "UriFileName", "", fileUri.AbsoluteUri );
            xsltArgs.AddParam( "UriDirectory", "", directoryUri.AbsoluteUri );
            xsltArgs.AddParam( "Namespace", "", args.Namespace );

            xsltArgs.AddExtensionObject( "urn:eo-util", new XsltExtensionObject() );


            /*
             * 
             */
            XsltSettings settings = new XsltSettings( true, true );
            XmlResolver resolver = new XmlUrlResolver();

            XslCompiledTransform xsl = new XslCompiledTransform();

            using ( XmlReader xr = XmlReader.Create( xslt ) )
            {
                try
                {
                    xsl.Load( xr, settings, resolver );
                }
                catch ( XsltCompileException ex )
                {
                    throw new ToolException( "Error loading transformation", ex );
                }
                catch ( XmlException ex )
                {
                    throw new ToolException( "Error loading transformation", ex );
                }
            }


            /*
             * 
             */
            StringBuilder sb = new StringBuilder();

            using ( TextWriter writer = new StringWriter( sb, CultureInfo.InvariantCulture ) )
            {
                try
                {
                    xsl.Transform( xpNav, xsltArgs, writer );
                }
                catch ( XmlException ex )
                {
                    throw new ToolException( "Error during transformation", ex );
                }
                catch ( XsltException ex )
                {
                    throw new ToolException( "Error during transformation", ex );
                }
            }

            return sb.ToString();
        }



        /// <summary />
        private static void ValidateDocument( FileInfo fileInfo, XmlDocument document )
        {
            #region Validations

            if ( fileInfo == null )
                throw new ArgumentNullException( nameof( fileInfo ) );

            if ( document == null )
                throw new ArgumentNullException( nameof( document ) );

            #endregion

            /*
             * 
             */
            XmlNamespaceManager manager = new XmlNamespaceManager( new NameTable() );
            manager.AddNamespace( "xsi", "http://www.w3.org/2001/XMLSchema-instance" );

            XmlNode schema = document.SelectSingleNode( " */@xsi:schemaLocation ", manager );

            if ( schema == null )
                return;


            /*
             * 
             */
            string[] parts = schema.Value.Replace( "\n", " " ).Replace( "\r", "" ).Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

            if ( (parts.Length % 2) != 0 )
                throw new ToolException( "invalid value in xsi:schemaLocation" );

            for ( int i = 0; i < parts.Length; i += 2 )
            {
                string ns = parts[ i ];
                string xsd = parts[ i + 1 ];

                string xsdPath = Path.Combine( fileInfo.DirectoryName, xsd );

                try
                {
                    document.Schemas.Add( ns, xsdPath );
                }
                catch ( XmlSchemaException ex )
                {
                    throw new ToolException( $"invalid schema '{ xsd }'", ex );
                }
                catch ( FileNotFoundException ex )
                {
                    throw new ToolException( $"invalid schema '{ xsd }'", ex );
                }
            }


            /*
             * 
             */
            List<string> validationErrors = new List<string>();

            try
            {
                document.Validate(
                    new ValidationEventHandler(
                        delegate ( Object sender, ValidationEventArgs e )
                        {
                            validationErrors.Add( e.Exception.Message );
                        }
                    )
                );
            }
            catch ( XmlSchemaValidationException ex )
            {
                throw new ToolException( "error validating against schema", ex );
            }

            if ( validationErrors.Count > 0 )
            {
                string err = string.Join( "\n", validationErrors );
                throw new ToolException( "found the following errors during validation:\n" + err );
            }
        }
    }
}
