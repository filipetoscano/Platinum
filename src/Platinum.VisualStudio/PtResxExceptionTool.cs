using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System;
using System.Text;

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

            sb.AppendFormat( "// PtResxExceptionTool" );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}
