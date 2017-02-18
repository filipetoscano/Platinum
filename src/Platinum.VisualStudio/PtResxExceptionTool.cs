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
    /// Automatically generates exception classes.
    /// </summary>
    [ComVisible( true )]
    [Guid( "fc195c8e-72c7-47d5-a8fd-e4dba1e32d17" )]
    [CodeGeneratorRegistration( typeof( PtResxExceptionTool ), "Platinum Exception", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtResxExceptionTool ) )]
    public class PtResxExceptionTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( string inputNamespace, string inputFileName, string inputContent, bool whatIf )
        {
            #region Validations

            if ( inputNamespace == null )
                throw new ArgumentNullException( nameof( inputNamespace ) );

            if ( inputFileName == null )
                throw new ArgumentNullException( nameof( inputFileName ) );

            if ( inputContent == null )
                throw new ArgumentNullException( nameof( inputContent ) );

            #endregion

            /*
             * #1. Load XML document
             */
            XmlDocument doc = X.Load( inputContent, "PlatinumResxException.xsd" );


            /*
             * #2. Build arguments
             */
            FileInfo inputFile = new FileInfo( inputFileName );
            string rawName = inputFile.Name.Substring( 0, inputFile.Name.Length - inputFile.Extension.Length );

            XsltArgumentList args = new XsltArgumentList();
            args.AddParam( "ToolVersion", "", Assembly.GetExecutingAssembly().GetName( false ).Version.ToString( 4 ) );
            args.AddParam( "FileName", "", rawName );
            args.AddParam( "FullFileName", "", inputFile.FullName );
            args.AddParam( "Namespace", "", inputNamespace );

            args.AddExtensionObject( "urn:eo-util", new XsltExtensionObject() );


            /*
             * #3. Load transformation
             */
            XslCompiledTransform xslt = X.LoadXslt( "PlatinumResxException-ToCode.xslt" );


            /*
             * #4. Apply transformation
             */
            return X.ToText( xslt, args, doc );
        }
    }
}
