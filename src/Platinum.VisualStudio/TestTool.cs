using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using VSLangProj80;

namespace Platinum.VisualStudio
{
    /// <summary>
    /// Test tool.
    /// </summary>
    [ComVisible( true )]
    [Guid( "e820d8a8-03a7-41c2-973e-07cad36e2888" )]
    [CodeGeneratorRegistration( typeof( TestTool ), "Test Tool", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( TestTool ) )]
    public class TestTool : BaseTool
    {
        /// <summary />
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

            sb.AppendFormat( "// TestTool {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Namespace={0}", fileNamespace );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Input={0}", inputFileName );
            sb.Append( Environment.NewLine );

            sb.AppendFormat( "// Content [bytes]={0}", inputContent.Length );
            sb.Append( Environment.NewLine );


            /*
             * 
             */
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Platinum.VisualStudio.Resources.PlatinumConfigGen.xsd";

            using ( Stream stream = assembly.GetManifestResourceStream( resourceName ) )
            using ( StreamReader reader = new StreamReader( stream ) )
            {
                string result = reader.ReadToEnd();

                sb.AppendLine( "/*" );
                sb.Append( result );
                sb.AppendLine( "*/" );
            }

            return sb.ToString();
        }
    }
}