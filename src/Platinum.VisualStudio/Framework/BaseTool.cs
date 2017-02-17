using Microsoft.Samples.VisualStudio.GeneratorSample;
using System;
using System.IO;
using System.Text;

namespace Platinum.VisualStudio
{
    /// <summary />
    public abstract class BaseTool : BaseCodeGeneratorWithSite, ITool
    {
        /// <summary />
        protected override byte[] GenerateCode( string inputFileContent )
        {
            string outputContent;

            try
            {
                outputContent = Execute( this.FileNameSpace, this.InputFilePath, inputFileContent, false );
            }
            catch ( ToolException ex )
            {
                outputContent = ErrorEmit( ex );
            }
            catch ( Exception ex )
            {
                outputContent = ErrorEmit( ex );
            }

            return Encoding.UTF8.GetBytes( outputContent );
        }



        /// <summary>
        /// Executes the generator tool, through a command-line interface.
        /// </summary>
        /// <param name="inputFileName">Input file name.</param>
        /// <param name="fileNamespace">CLR namespace.</param>
        /// <param name="whatIf">Whether to perform changes or not.</param>
        void ITool.Execute( string inputFileName, string fileNamespace, bool whatIf )
        {
            #region Validations

            if ( inputFileName == null )
                throw new ArgumentNullException( nameof( inputFileName ) );

            if ( fileNamespace == null )
                throw new ArgumentNullException( nameof( fileNamespace ) );

            #endregion


            /*
             * 
             */
            FileInfo file = new FileInfo( inputFileName );
            string inputContent;

            try
            {
                inputContent = File.ReadAllText( file.FullName );
            }
            catch ( Exception ex )
            {
                throw new ToolException( $"Failed to load '{ file.FullName }'.", ex );
            }


            /*
             * Execute tool.
             */
            string outputContent;

            try
            {
                outputContent = Execute( fileNamespace, inputFileName, inputContent, whatIf );
            }
            catch ( ToolException )
            {
                throw;
            }
            catch ( Exception ex )
            {
                throw new ToolException( $"Unhandled exception executing tool '{ this.GetType().Name }'.", ex );
            }


            /*
             * Write primary output.
             */
            if ( whatIf == true )
                return;

            string outputFile = Path.Combine( file.DirectoryName, Path.GetFileNameWithoutExtension( file.FullName ) + ".cs" );

            try
            {
                File.WriteAllText( outputFile, outputContent, new UTF8Encoding( false ) );
            }
            catch ( Exception ex )
            {
                throw new ToolException( $"Failed to write to '{ outputFile }'.", ex );
            }
        }


        /// <summary>
        /// Generates (output) content based on the exception which occurred during
        /// tool execution.
        /// </summary>
        /// <param name="exception">
        /// Exception which occurred during tool execution.
        /// </param>
        /// <returns>
        /// C# content.
        /// </returns>
        private static string ErrorEmit( Exception exception )
        {
            #region Validations

            if ( exception == null )
                throw new ArgumentNullException( nameof( exception ) );

            #endregion

            StringBuilder sb = new StringBuilder();

            foreach ( var line in exception.ToString().Split( '\n' ) )
            {
                sb.Append( "// " );
                sb.Append( line.TrimEnd() );
                sb.Append( Environment.NewLine );
            }

            return sb.ToString();
        }


        /// <summary />
        protected abstract string Execute( string fileNamespace, string inputFileName, string inputContent, bool whatIf );
    }
}
