using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VSLangProj80;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Test tool.
    /// </summary>
    [ComVisible( true )]
    [Guid( "30e16ee4-d230-4c86-a82a-9113330c2ef1" )]
    [CodeGeneratorRegistration( typeof( TestTool ), "Test Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( TestTool ) )]
    public class TestTool : BaseTool
    {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "TestTool";
#pragma warning restore 0414


        /// <summary />
        protected override string Execute( string fileNamespace, string inputFileName, string inputContent )
        {
            StringBuilder sb = new StringBuilder();

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