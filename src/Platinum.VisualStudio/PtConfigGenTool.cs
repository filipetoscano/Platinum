using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System;
using System.Text;

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

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "// ConfigGenTool" );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}
