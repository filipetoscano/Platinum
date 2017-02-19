using Microsoft.Samples.VisualStudio.GeneratorSample;
using System;
using System.Text;

namespace Platinum.VisualStudio.Plugin
{
    /// <summary />
    public abstract class BasePlugin<T> : BaseCodeGeneratorWithSite where T : IPlugin
    {
        /// <summary />
        protected override byte[] GenerateCode( string inputFileContent )
        {
            /*
             * 
             */
            IPlugin plugin = Activator.CreateInstance<T>();


            /*
             * 
             */
            byte[] outputContent;

            ToolGenerateArgs args = new ToolGenerateArgs()
            {
                Content = inputFileContent,
                Namespace = this.FileNameSpace,
                FileName = this.InputFilePath,
                WhatIf = false
            };

            try
            {
                outputContent = plugin.Generate( args );
            }
            catch ( Exception ex )
            {
                outputContent = Encoding.UTF8.GetBytes( ErrorEmit( ex ) );
            }

            return outputContent;
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
    }
}
