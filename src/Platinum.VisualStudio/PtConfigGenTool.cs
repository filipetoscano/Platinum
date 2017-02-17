using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Xsl;
using VSLangProj80;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Generate configuration section handler.
    /// </summary>
    [ComVisible( true )]
    [Guid( "1de76d29-65be-4aba-baba-ae8719e5dd0e" )]
    [CodeGeneratorRegistration( typeof( PtConfigGenTool ), "Test Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtConfigGenTool ) )]
    public class PtConfigGenTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( string fileNamespace, string inputFileName, string inputContent, bool whatIf )
        {
            #region Validations

            if ( fileNamespace == null )
                throw new ArgumentNullException( nameof( fileNamespace ) );

            if ( inputFileName == null )
                throw new ArgumentNullException( nameof( inputFileName ) );

            if ( inputContent == null )
                throw new ArgumentNullException( nameof( inputContent ) );

            #endregion

            /*
             * #1. Load XML document
             */
            XmlDocument doc = X.Load( inputContent, "PlatinumConfigGen.xsd" );


            /*
             * #2. Build arguments
             */
            FileInfo inputFile = new FileInfo( inputFileName );
            string rawName = inputFile.Name.Substring( 0, inputFile.Name.Length - inputFile.Extension.Length );

            XsltArgumentList args = new XsltArgumentList();
            args.AddParam( "ToolVersion", "", Assembly.GetExecutingAssembly().GetName( false ).Version.ToString( 4 ) );
            args.AddParam( "FileName", "", rawName );
            args.AddParam( "FullFileName", "", inputFile.FullName );
            args.AddParam( "Namespace", "", fileNamespace );

            args.AddExtensionObject( "urn:eo-util", new XsltExtensionObject() );


            /*
             * #3. Load transformation
             */
            XslCompiledTransform xslt = X.LoadXslt( "PlatinumConfigGen-ToCode.xslt" );


            /*
             * #4. Apply transformation
             */
            return X.ToText( xslt, args, doc );
        }
    }
}
