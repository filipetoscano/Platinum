using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Platinum.Reflection
{
    /// <summary />
    public static class AppDomainUtils
    {
        private static bool _initialized;
        private static object _lock = new object();


        /// <summary>
        /// Pre-loads all assemblies in the base directory of the current
        /// domain.
        /// </summary>
        /// <param name="isWebApplication">
        /// If the current application is a web application, the directory
        /// with the assemblies is actually under the directory of the
        /// current domain.
        /// </param>
        public static void PreLoad( bool isWebApplication = false )
        {
            if ( _initialized == false )
            {
                lock ( _lock )
                {
                    if ( _initialized == false )
                    {
                        DoPreLoad( isWebApplication );

                        Thread.MemoryBarrier();
                        _initialized = true;
                    }
                }
            }
        }


        /// <summary />
        private static void DoPreLoad( bool isWebApplication )
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            if ( isWebApplication == true )
                path = Path.Combine( path, "bin" );

            PreLoadAssembliesFromPath( path );
        }


        /// <summary />
        private static void PreLoadAssembliesFromPath( string path )
        {
            #region Validations

            if ( path == null )
                throw new ArgumentNullException( nameof( path ) );

            #endregion

            FileInfo[] files = files = new DirectoryInfo( path ).GetFiles( "*.dll", SearchOption.TopDirectoryOnly );

            foreach ( var fi in files )
            {
                var assemblyPath = fi.FullName;
                var assemblyName = AssemblyName.GetAssemblyName( assemblyPath );

                if ( AppDomain.CurrentDomain.GetAssemblies()
                    .Any( assembly => AssemblyName.ReferenceMatchesDefinition( assemblyName, assembly.GetName() ) ) == false )
                {
                    try
                    {
                        Assembly.Load( assemblyName );
                    }
                    catch ( Exception )
                    {
                        // Silently ignore!
                    }
                }
            }
        }
    }
}
