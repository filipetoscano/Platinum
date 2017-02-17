using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Platinum.VisualStudio.Command
{
    /// <summary />
    public class Program
    {
        /// <summary />
        public static void Main( string[] args )
        {
            /*
             * TODO
             */
            CommandLine cl = new CommandLine();
            cl.Project = args[ 0 ];



            /*
             *
             */
            Type interfaceType = typeof( ITool );

            var rs = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                     from type in assembly.GetTypes()
                     where type.IsInterface == false
                     where interfaceType.IsAssignableFrom( type ) == true
                     select type;

            Dictionary<string, Type> tools = new Dictionary<string, Type>();

            foreach ( var t in rs )
            {
                tools.Add( t.Name, t );
            }


            /*
             * 
             */
            int exitCode;

            if ( cl.Project != null )
                exitCode = RunProject( cl, tools );
            else
                exitCode = 2;

            Environment.ExitCode = exitCode;
        }



        /// <summary>
        /// Runs all of the extension custom tools for the specified project.
        /// </summary>
        /// <param name="cl">Command-line.</param>
        /// <param name="tools">List of custom tools.</param>
        private static int RunProject( CommandLine cl, Dictionary<string, Type> tools )
        {
            #region Validations

            if ( cl == null )
                throw new ArgumentNullException( nameof( cl ) );

            if ( tools == null )
                throw new ArgumentNullException( nameof( tools ) );

            #endregion


            /*
             * 
             */
            if ( File.Exists( cl.Project ) == false )
            {
                Console.Error.WriteLine( $"err: Project '{ cl.Project }' not found." );
                return 10001;
            }


            /*
             *
             */
            XmlNamespaceManager manager = new XmlNamespaceManager( new NameTable() );
            manager.AddNamespace( "ns", "http://schemas.microsoft.com/developer/msbuild/2003" );

            XmlDocument csproj = new XmlDocument();

            try
            {
                csproj.Load( cl.Project );
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine( $"err: Project '{ cl.Project }' failed to load." );
                Console.Error.WriteLine( ex.ToString() );
                return 10002;
            }


            /*
             * 
             */
            XmlElement rootNsElem = (XmlElement) csproj.SelectSingleNode( " /ns:Project/ns:PropertyGroup/ns:RootNamespace ", manager );

            if ( rootNsElem == null )
            {
                Console.Error.WriteLine( $"err: Project '{ cl.Project }' does not contain a 'RootNamespace' element." );
                return 10003;
            }

            string rootNs = rootNsElem.InnerText;


            /*
             * 
             */
            int exitCode = 0;

            foreach ( XmlElement contentElem in csproj.SelectNodes( @" //ns:Content[ @Include and ns:Generator ] | " +
                                                                     " //ns:None[ @Include and ns:Generator ] ", manager ) )
            {
                string relativePath = contentElem.Attributes[ "Include" ].Value;
                string toolName = contentElem.SelectSingleNode( " ns:Generator ", manager ).InnerText;

                if ( tools.ContainsKey( toolName ) == false )
                    continue;


                /*
                 * 
                 */
                string fullName = Path.Combine(
                    Path.GetDirectoryName( cl.Project ),
                    relativePath );
                FileInfo file = new FileInfo( fullName );

                string ns = ToNamespace( rootNs, relativePath );

                Console.WriteLine( "f={0} ns={1} t={2}", relativePath, ns, toolName );

                if ( cl.WhatIf == false )
                    exitCode += RunTool( tools, toolName, file, ns, cl.WhatIf );
            }


            /*
             * 
             */
            return exitCode;
        }


        /// <summary>
        /// Generates a CLR namespace from the two parts.
        /// </summary>
        /// <param name="rootNs">Root namespace of project.</param>
        /// <param name="relativePath">Relative path of file to the project file.</param>
        /// <returns>CLR namespace of target file.</returns>
        private static string ToNamespace( string rootNs, string relativePath )
        {
            #region Validations

            if ( rootNs == null )
                throw new ArgumentNullException( nameof( rootNs ) );

            if ( relativePath == null )
                throw new ArgumentNullException( nameof( relativePath ) );

            #endregion

            int ix = relativePath.LastIndexOf( "\\", StringComparison.Ordinal );

            if ( ix == -1 )
                return rootNs;

            return string.Concat( rootNs, ".", relativePath.Substring( 0, ix ).Replace( "\\", "." ) );
        }


        /// <summary />
        private static int RunTool( Dictionary<string, Type> tools, string tool, FileInfo file, string @namespace, bool whatIf )
        {
            #region Validations

            if ( tools == null )
                throw new ArgumentNullException( nameof( tools ) );

            if ( tool == null )
                throw new ArgumentNullException( nameof( tool ) );

            if ( file == null )
                throw new ArgumentNullException( nameof( file ) );

            if ( @namespace == null )
                throw new ArgumentNullException( nameof( @namespace ) );

            #endregion


            /*
             * 
             */
            Type toolType = tools[ tool ];
            ITool bt = (ITool) Activator.CreateInstance( toolType );
            int status = 0;

            try
            {
                bt.Execute( file.FullName, @namespace, whatIf );
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine( "error: " );
                Console.Error.WriteLine( ex.ToString() );
                status = 1;
            }

            return status;
        }
    }
}
