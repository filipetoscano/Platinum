using System;
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
    /// 
    /// </summary>
    public class PtDataStatementsTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            /*
             * #1. Load the .csproj in the same folder.
             */
            DirectoryInfo dir = new DirectoryInfo( Path.GetDirectoryName( args.FileName ) );

            FileInfo[] files = dir.GetFiles( "*.csproj" );

            if ( files.Length == 0 )
                throw new ToolException( "No .csproj found in current directory." );

            if ( files.Length > 1 )
                throw new ToolException( "More than one .csproj found in current directory." );


            /*
             * #2. Collect all embedded SQL files in .csproj
             */
            List<string> sqlFiles = new List<string>();

            XmlDocument csproj = new XmlDocument();
            csproj.Load( files[ 0 ].FullName );

            XmlNamespaceManager manager = new XmlNamespaceManager( new NameTable() );
            manager.AddNamespace( "vs", "http://schemas.microsoft.com/developer/msbuild/2003" );

            foreach ( XmlElement embedded in csproj.SelectNodes( " //vs:EmbeddedResource[ @Include ] ", manager ) )
            {
                string include = embedded.Attributes[ "Include" ].InnerText;

                if ( include.EndsWith( ".sql", StringComparison.InvariantCulture ) == false )
                    continue;

                sqlFiles.Add( include );
            }


            /*
             * #1. Load XML document
             */
            XmlDocument doc = X.Load( args.Content, "PlatinumDataStatements.xsd" );


            /*
             * #2. Build arguments
             */
            FileInfo inputFile = new FileInfo( args.FileName );
            string rawName = inputFile.Name.Substring( 0, inputFile.Name.Length - inputFile.Extension.Length );

            XsltArgumentList xsltArgs = new XsltArgumentList();
            xsltArgs.AddParam( "ToolVersion", "", Assembly.GetExecutingAssembly().GetName( false ).Version.ToString( 4 ) );
            xsltArgs.AddParam( "FileName", "", rawName );
            xsltArgs.AddParam( "FullFileName", "", inputFile.FullName );
            xsltArgs.AddParam( "Namespace", "", args.Namespace );

            xsltArgs.AddExtensionObject( "urn:eo-util", new XsltExtensionObject() );


            /*
             * #3. Load transformation
             */
            XslCompiledTransform xslt = X.LoadXslt( "PlatinumDataStatements-ToCode.xslt" );


            /*
             * #4. Apply transformation
             */
            return X.ToText( xslt, xsltArgs, doc );
        }
    }
}
