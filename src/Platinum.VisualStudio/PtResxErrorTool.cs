using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System;
using System.Text;

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

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "// ResxErrorTool" );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}
