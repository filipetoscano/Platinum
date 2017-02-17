using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using VSLangProj80;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Generate code-behind class and ER.resx files.
    /// </summary>
    [ComVisible( true )]
    [Guid( "504e9977-7be1-4518-82d9-9b14a098c773" )]
    [CodeGeneratorRegistration( typeof( PtResxErrorTool ), "Generates error resources.", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtResxErrorTool ) )]
    public class PtResxErrorTool : BaseTool
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
            XmlDocument doc = X.Load( inputContent, "PlatinumResxError.xsd" );


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
            XslCompiledTransform xsl1 = X.LoadXslt( "PlatinumResxError-ToCode.xslt" );
            XslCompiledTransform xsl2 = X.LoadXslt( "PlatinumResxError-ToResx.xslt" );


            /*
             * #4. Apply transformation
             */
            string cs = X.ToText( xsl1, args, doc );
            string resx = X.ToXml( xsl2, args, doc );


            /*
             * #5. Write the contents of the ER.resx file
             */
            string resxPath = Path.Combine( inputFile.DirectoryName, "ER.resx" );
            File.WriteAllText( resxPath, resx, Encoding.UTF8 );

            return cs;
        }
    }
}
