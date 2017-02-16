using Microsoft.VisualStudio.Shell;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using VSLangProj80;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Another tool.
    /// </summary>
    [ComVisible( true )]
    [Guid( "aa4ae541-1a08-4f8e-87c7-a6bc6d635e62" )]
    [CodeGeneratorRegistration( typeof( AnotherTool ), "Another Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( AnotherTool ) )]
    public class AnotherTool : BaseTool
    {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "TestTool";
#pragma warning restore 0414


        /// <summary />
        protected override string Execute( string fileNamespace, string inputFileName, string inputContent )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "// AnotherTool {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Namespace={0}", fileNamespace );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Input={0}", inputFileName );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Content [bytes]={0}", inputContent.Length );
            sb.Append( Environment.NewLine );

            return sb.ToString();
        }
    }
}