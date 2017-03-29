using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Automatically generates validation rule set.
    /// </summary>
    public class PtValidationRuleSetTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            /*
             * #1. Load XML document
             */
            XmlDocument doc = X.Load( args.Content, "PlatinumValidationRuleSet.xsd" );


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
            XslCompiledTransform xslt = X.LoadXslt( "PlatinumValidationRuleSet-ToCode.xslt" );


            /*
             * #4. Apply transformation
             */
            return X.ToText( xslt, xsltArgs, doc );
        }
    }
}
