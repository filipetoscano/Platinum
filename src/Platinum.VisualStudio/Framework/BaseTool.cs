using System;
using System.IO;
using System.Text;

namespace Platinum.VisualStudio
{
    /// <summary />
    public abstract class BaseTool : IPlugin, ITool, IGenerator
    {
        /// <summary>
        /// Command-line support.
        /// </summary>
        /// <param name="args">Generation arguments.</param>
        void ITool.Run( ToolRunArgs args )
        {
            /*
             * 
             */
            FileInfo file = new FileInfo( args.FileName );
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

            ToolGenerateArgs toolArgs = new ToolGenerateArgs()
            {
                Namespace = args.Namespace,
                FileName = args.FileName,
                Content = inputContent,
                WhatIf = args.WhatIf,
                IsInteractive = false,
            };

            try
            {
                outputContent = Execute( toolArgs );
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
            if ( args.WhatIf == true )
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
        /// Plugin support.
        /// </summary>
        /// <param name="args">Generation arguments.</param>
        /// <returns>Content.</returns>
        byte[] IPlugin.Generate( ToolGenerateArgs args )
        {
            string outputContent;

            try
            {
                outputContent = Execute( args );
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
        /// Tool chain support.
        /// </summary>
        /// <param name="args">Generation arguments.</param>
        /// <returns>Generated content.</returns>
        string IGenerator.Generate( ToolGenerateArgs args )
        {
            return Execute( args );
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
        protected abstract string Execute( ToolGenerateArgs args );
    }
}
