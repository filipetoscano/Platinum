using DbUp.Engine;
using DbUp.Engine.Transactions;
using Platinum.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Platinum.Database
{
    /// <summary />
    public class DbConfigScriptProvider : IScriptProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary />
        public DbConfigScriptProvider( Assembly assembly, string resxPrefix, string environment )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( resxPrefix == null )
                throw new ArgumentNullException( nameof( resxPrefix ) );

            if ( environment == null )
                throw new ArgumentNullException( nameof( environment ) );

            #endregion

            this.Assembly = assembly;
            this.ResourcePrefix = resxPrefix;
            this.Environment = environment;
        }


        /// <summary />
        public Assembly Assembly
        {
            get;
            private set;
        }


        /// <summary />
        public string ResourcePrefix
        {
            get;
            private set;
        }


        /// <summary />
        public string Environment
        {
            get;
            private set;
        }


        /// <summary />
        public IEnumerable<SqlScript> GetScripts( IConnectionManager connectionManager )
        {
            try
            {
                return GetScripts();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                throw;
            }
        }


        /// <summary />
        private List<SqlScript> GetScripts()
        {
            /*
             * 
             */
            List<SqlScript> scripts = new List<SqlScript>();

            var resources = this.Assembly.GetManifestResourceNames().Where( x => x.StartsWith( this.ResourcePrefix ) == true );

            if ( resources.Count() == 0 )
                return scripts;


            /*
             * Extract all of the embedded resources to temporary directory.
             */
            string tmp = Path.Combine( Path.GetTempPath(), "PD-" + Guid.NewGuid().ToString() );
            DirectoryInfo dir = Directory.CreateDirectory( tmp );
            logger.Debug( "Extracting to directory: '{0}'", tmp );

            foreach ( var resx in resources )
            {
                string fileName = resx.Substring( this.ResourcePrefix.Length );
                logger.Debug( "Extracting: '{0}'", fileName );

                using ( var fs = File.Create( Path.Combine( tmp, fileName ) ) )
                {
                    using ( var stream = this.Assembly.GetManifestResourceStream( resx ) )
                    {
                        stream.Seek( 0, SeekOrigin.Begin );
                        stream.CopyTo( fs );
                    }
                }
            }


            /*
             * 
             */
            foreach ( var file in dir.GetFiles( "*.xml", SearchOption.TopDirectoryOnly ).OrderBy( x => x.Name ) )
            {
                var script = BuildScript( file, this.Environment );

                if ( script != null )
                    scripts.Add( script );
            }


            /*
             * 
             */
            try
            {
                // Directory.Delete( tmp, true );
            }
            catch ( Exception ex )
            {
                logger.Debug( "Failed to delete temporary directory '{0}'.", tmp );
                logger.Debug( ex.ToString() );
            }


            /*
             * 
             */
            return scripts;
        }


        /// <summary>
        /// Builds a SQL script from the XML data file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="environment">Environment which is being targetted.</param>
        /// <returns>Instance of SQL script, or null if the resource is not supposed to be run
        /// on the target environment.</returns>
        private static SqlScript BuildScript( FileInfo file, string environment )
        {
            #region Validations

            if ( file == null )
                throw new ArgumentNullException( nameof( file ) );

            if ( environment == null )
                throw new ArgumentNullException( nameof( environment ) );

            #endregion


            /*
             * Based on resourceName, can we run on this environment?
             */
            if ( TargetsEnvironment( file, environment ) == false )
            {
                logger.Debug( "{0}: skip (not for '{1}')", file.Name, environment );
                return null;
            }


            /*
             * 
             */
            string exePath = @"C:\programs\bin\dbconfig.exe";
            string sqlFile = Path.Combine( file.DirectoryName, Path.GetFileNameWithoutExtension( file.Name ) + ".sql" );


            /*
             * 
             */
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.WorkingDirectory = file.DirectoryName;
            psi.FileName = exePath;
            psi.Arguments = $"--kiss --env={ environment } \"{ file.Name }\" ";
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;

            Process p = Process.Start( psi );
            p.WaitForExit();


            /*
             * 
             */
            FileInfo sql = new FileInfo( sqlFile );

            if ( sql.Exists == false )
            {
                Console.WriteLine( p.StandardOutput.ReadToEnd() );
                Console.WriteLine( p.StandardError.ReadToEnd() );

                // TODO: improve
                throw new ApplicationException( "Output file not found" );
            }

            if ( sql.Length == 0 )
            {
                logger.Debug( "{0}: skip (empty transformation)", file.Name );
                return null;
            }


            /*
             * 
             */
            string content = File.ReadAllText( sqlFile );
            string name = file.Name;
            string hash = Platinum.Cryptography.Hash.SHA256( content );

            logger.Debug( "{0}: transformed", file.Name );
            return new DataSqlScript( name, hash, content );
        }


        /// <summary />
        private static bool TargetsEnvironment( FileInfo file, string environment )
        {
            #region Validations

            if ( file == null )
                throw new ArgumentNullException( nameof( file ) );

            if ( environment == null )
                throw new ArgumentNullException( nameof( environment ) );

            #endregion

            Regex regex = new Regex( @"-(?<env>DEV|INT|TST|QUA|PRE|PRD|LOCAL|TEST|STAGE|PROD)\.xml$" );
            var match = regex.Match( file.Name );

            if ( match.Success == false )
                return true;

            return match.Groups[ "env" ].Value == environment;
        }
    }
}
