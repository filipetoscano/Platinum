using Microsoft.VisualStudio.Shell;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using VSLangProj80;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary>
    /// Test tool.
    /// </summary>
    [ComVisible( true )]
    [Guid( "e820d8a8-03a7-41c2-973e-07cad36e2888" )]
    [CodeGeneratorRegistration( typeof( PtTestTool ), "Test Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( PtTestTool ) )]
    public class PtTestTool : BasePlugin<TestTool>
    {
    }


    /// <summary>
    /// Test tool.
    /// </summary>
    public class TestTool : BaseTool
    {
        /// <summary>
        /// Executes tool.
        /// </summary>
        protected override string Execute( ToolGenerateArgs args )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "// TestTool {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Namespace={0}", args.Namespace );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Input={0}", args.FileName );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Content [bytes]={0}", args.Content.Length );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}