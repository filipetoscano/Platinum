using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public class PtDataStatementsTool : BaseTool
    {
        internal const string NS = "urn:platinum/data/statements";


        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            /*
             * #1. Load the .csproj in the same folder.
             */
            DirectoryInfo dir = new DirectoryInfo( Path.GetDirectoryName( args.FileName ) );

            FileInfo[] projects = dir.GetFiles( "*.csproj" );

            if ( projects.Length == 0 )
                throw new ToolException( "No .csproj found in current directory." );

            if ( projects.Length > 1 )
                throw new ToolException( "More than one .csproj found in current directory." );


            /*
             * #2. Collect all embedded SQL files in .csproj
             */
            List<string> sqlFiles = new List<string>();

            XmlDocument csproj = new XmlDocument();
            csproj.Load( projects[ 0 ].FullName );

            XmlNamespaceManager manager = new XmlNamespaceManager( new NameTable() );
            manager.AddNamespace( "vs", "http://schemas.microsoft.com/developer/msbuild/2003" );
            manager.AddNamespace( "pd", "urn:platinum/data/statements" );

            foreach ( XmlElement embedded in csproj.SelectNodes( " //vs:EmbeddedResource[ @Include ] ", manager ) )
            {
                string include = embedded.Attributes[ "Include" ].InnerText;

                if ( include.EndsWith( ".sql", StringComparison.InvariantCulture ) == false )
                    continue;

                string noPrefix = include.Substring( 0, include.Length - 4 );
                string slashes = noPrefix.Replace( "\\", "/" );

                sqlFiles.Add( slashes );
            }


            /*
             * #3. Build -internal file.
             */
            XmlDocument idoc = new XmlDocument();
            var root = idoc.Append( "statements-internal", NS );

            foreach ( string relativePath in sqlFiles )
            {
                string[] segments = relativePath.Split( new char[] { '/' } );
                var path = segments.Take( segments.Length - 1 );
                var file = segments.Last();

                var folder = Find( root, manager, path );

                var xfile = folder.Append( "file", NS );
                xfile.Attribute( "name", file )
                     .Attribute( "resx", relativePath );
            }


            /*
             * #4. Load XML document
             */
            XmlDocument doc = X.Load( idoc.OuterXml, "PlatinumDataStatements.xsd" );


            /*
             * #5. Build arguments
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
             * #6. Load transformation
             */
            XslCompiledTransform xslt = X.LoadXslt( "PlatinumDataStatements-ToCode.xslt" );


            /*
             * #7. Apply transformation
             */
            return X.ToText( xslt, xsltArgs, doc );
        }


        /// <summary />
        private static XmlElement Find( XmlElement parent, XmlNamespaceManager manager, IEnumerable<string> path )
        {
            #region Validations

            if ( parent == null )
                throw new ArgumentNullException( nameof( parent ) );

            if ( manager == null )
                throw new ArgumentNullException( nameof( manager ) );

            if ( path == null )
                throw new ArgumentNullException( nameof( path ) );

            #endregion

            XmlElement folder = parent;
            XmlElement scope = parent;

            foreach ( var p in path )
            {
                folder = scope.SelectSingle( $" pd:add[ @name = '{ p }' ]", manager );

                if ( folder == null )
                {
                    folder = scope.Prepend( "add", NS );
                    folder.Attribute( "name", p );
                }

                scope = folder;
            }

            return folder;
        }
    }
}
