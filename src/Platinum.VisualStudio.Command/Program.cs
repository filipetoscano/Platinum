using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            cl.Solution = args[ 0 ];


            /*
             * Load referenced assemblies into domain, so that they are discoverable.
             * In this case, it is preferable to probing the entire directory -- since
             * these command-line tools tend to be put all together in a bin/ folder.
             */
            foreach ( string key in ConfigurationManager.AppSettings.Keys )
            {
                string assemblyName = ConfigurationManager.AppSettings[ key ];

                try
                {
                    AppDomain.CurrentDomain.Load( assemblyName );
                }
                catch ( Exception ex )
                {
                    Console.Error.WriteLine( "err: failed to load assembly '{0}'.", assemblyName );
                    Console.Error.WriteLine( ex.ToString() );
                    Environment.ExitCode = 1001;
                    return;
                }
            }


            /*
             *
             */
            Type interfaceType = typeof( ITool );
            Dictionary<string, Type> tools = new Dictionary<string, Type>();

            foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies() )
            {
                if ( assembly.IsDynamic == true )
                    continue;

                if ( assembly.FullName.StartsWith( "mscorlib, " ) == true )
                    continue;

                if ( assembly.FullName.StartsWith( "System, " ) == true )
                    continue;

                if ( assembly.FullName.StartsWith( "System." ) == true )
                    continue;

                Console.WriteLine( "loading tools from {0}...", assembly.FullName );


                /*
                 * 
                 */
                Type[] types;

                try
                {
                    types = assembly.GetTypes();
                }
                catch ( ReflectionTypeLoadException ex )
                {
                    Console.WriteLine( "Exception:" );
                    Console.WriteLine( ex.ToString() );

                    foreach ( var le in ex.LoaderExceptions )
                    {
                        Console.WriteLine();
                        Console.WriteLine( "Loader exception:" );
                        Console.WriteLine( le.ToString() );
                    }

                    Environment.ExitCode = 1002;
                    return;
                }
                catch ( Exception ex )
                {
                    Console.WriteLine( ex.ToString() );

                    Environment.ExitCode = 1003;
                    return;
                }

                foreach ( var type in types )
                {
                    if ( type.IsInterface == true )
                        continue;

                    if ( interfaceType.IsAssignableFrom( type ) == true )
                    {
                        Console.WriteLine( "+ {0}", type.Name );
                        tools.Add( type.Name, type );
                    }
                }
            }


            /*
             * 
             */
            int exitCode;

            if ( cl.Solution != null )
                exitCode = RunSolution( cl.Solution, tools, cl.WhatIf );
            else if ( cl.Project != null )
                exitCode = RunProject( cl.Project, tools, cl.WhatIf );
            else
                exitCode = 1000;

            Environment.ExitCode = exitCode;
        }


        /// <summary>
        /// Runs all of the extension custom tools for the specified project.
        /// </summary>
        /// <param name="solution">Path of solution file.</param>
        /// <param name="tools">List of custom tools.</param>
        /// <param name="whatIf">Whether to generate content, or not.</param>
        private static int RunSolution( string solution, Dictionary<string, Type> tools, bool whatIf )
        {
            #region Validations

            if ( solution == null )
                throw new ArgumentNullException( nameof( solution ) );

            if ( tools == null )
                throw new ArgumentNullException( nameof( tools ) );

            #endregion


            /*
             * 
             */
            if ( File.Exists( solution ) == false )
            {
                Console.Error.WriteLine( $"err: Solution '{ solution }' not found." );
                return 10001;
            }

            string rootPath = Path.GetDirectoryName( solution );


            /*
             * 
             */
            string sln = File.ReadAllText( solution );
            string[] lines = sln.Split( '\n' );


            /*
             * 
             */
            Regex regex = new Regex( "^Project\\(\"{.*?}\"\\) = \"(?<name>.*?)\", \"(?<path>.*?.csproj)\", \"{.*?}\"$" );
            var exitCode = 0;

            foreach ( var line in lines )
            {
                var m = regex.Match( line.Trim() );

                if ( m.Success == false )
                    continue;


                /*
                 * 
                 */
                string rel = m.Groups[ "path" ].Value;
                string csproj = Path.Combine( rootPath, rel );
                Console.WriteLine( "csproj=" + m.Groups[ "path" ].Value );

                exitCode += RunProject( csproj, tools, whatIf );
            }

            return 0;
        }



        /// <summary>
        /// Runs all of the extension custom tools for the specified project.
        /// </summary>
        /// <param name="cl">Command-line.</param>
        /// <param name="tools">List of custom tools.</param>
        private static int RunProject( string project, Dictionary<string, Type> tools, bool whatIf )
        {
            #region Validations

            if ( project == null )
                throw new ArgumentNullException( nameof( project ) );

            if ( tools == null )
                throw new ArgumentNullException( nameof( tools ) );

            #endregion


            /*
             * 
             */
            if ( File.Exists( project ) == false )
            {
                Console.Error.WriteLine( $"err: Project '{ project }' not found." );
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
                csproj.Load( project );
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine( $"err: Project '{ project }' failed to load." );
                Console.Error.WriteLine( ex.ToString() );
                return 10002;
            }


            /*
             * 
             */
            XmlElement rootNsElem = (XmlElement) csproj.SelectSingleNode( " /ns:Project/ns:PropertyGroup/ns:RootNamespace ", manager );

            if ( rootNsElem == null )
            {
                Console.Error.WriteLine( $"err: Project '{ project }' does not contain a 'RootNamespace' element." );
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
                    Path.GetDirectoryName( project ),
                    relativePath );
                FileInfo file = new FileInfo( fullName );

                string ns = ToNamespace( rootNs, relativePath );

                Console.WriteLine( "f={0} ns={1} t={2}", relativePath, ns, toolName );

                exitCode += RunTool( tools, toolName, file, ns, whatIf );
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


            /*
             * 
             */
            ToolRunArgs args = new ToolRunArgs();
            args.Namespace = @namespace;
            args.FileName = file.FullName;
            args.WhatIf = whatIf;

            try
            {
                bt.Run( args );
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
