using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Generate code-behind class and ER.resx files.
    /// </summary>
    public class PtResxErrorTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            /*
             * #1. Load XML document
             */
            XmlDocument doc = X.Load( args.Content, "PlatinumResxError.xsd" );


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
            XslCompiledTransform xsl1 = X.LoadXslt( "PlatinumResxError-ToCode.xslt" );
            XslCompiledTransform xsl2 = X.LoadXslt( "PlatinumResxError-ToResx.xslt" );


            /*
             * #4. Apply transformation
             */
            string cs = X.ToText( xsl1, xsltArgs, doc );
            string resx = X.ToXml( xsl2, xsltArgs, doc );


            /*
             * #5. Write the contents of the ER.resx file
             */
            if ( args.WhatIf == false )
            {
                string resxPath = Path.Combine( inputFile.DirectoryName, "ER.resx" );
                File.WriteAllText( resxPath, resx, Encoding.UTF8 );
            }

            return cs;
        }
    }
}
