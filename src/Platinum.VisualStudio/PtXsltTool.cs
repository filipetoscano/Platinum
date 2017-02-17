using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System;
using System.Text;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Applies an XSLT, generating C# as 'code-behind'.
    /// </summary>
    [ComVisible( true )]
    [Guid( "fc3f5148-b5c0-4877-bc80-7023e32de35d" )]
    [CodeGeneratorRegistration( typeof( PtXsltTool ), "XSLT Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtXsltTool ) )]
    public class PtXsltTool : BaseTool
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

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "// XsltTool" );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}
