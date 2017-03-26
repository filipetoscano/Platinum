using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Platinum.Configuration;
using Platinum.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Platinum.Database
{
    /// <summary />
    public class DbRunner
    {
        /// <summary />
        public int Run( string[] args )
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Logger logger = LogManager.GetCurrentClassLogger();
            DbOperation operation = DbOperation.Schema | DbOperation.Data;


            /*
             * 
             */
            try
            {
                if ( args.Length > 0 )
                {
                    string argList = string.Join( ",", args );

                    operation = (DbOperation) Enum.Parse( typeof( DbOperation ), argList, true );
                }
            }
            catch ( Exception ex )
            {
                DatabaseToolException dte = new DatabaseToolException( ER.Arguments_Invalid, ex );
                logger.Fatal( dte );

                return dte.Code;
            }


            /*
             * 
             */
            int exitCode;

            try
            {
                exitCode = Run( assembly, logger, operation );
            }
            catch ( ActorException ex )
            {
                logger.Fatal( ex );
                exitCode = ex.Code;
            }
            catch ( Exception ex )
            {
                DatabaseToolException dte = new DatabaseToolException( ER.Tool_UnhandledException, ex );

                logger.Fatal( dte );
                exitCode = dte.Code;
            }

            return exitCode;
        }


        /// <summary />
        private static int Run( Assembly assembly, Logger logger, DbOperation operation )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( logger == null )
                throw new ArgumentNullException( nameof( logger ) );

            #endregion


            /*
             * 
             */
            var appLogger = new NLogLogger( logger );


            /*
             * 
             */
            string application = AppConfiguration.Get<string>( "Application" );
            string environment = AppConfiguration.Get<string>( "Environment" );


            /*
             * 
             */
            var cs = AppConfiguration.ConnectionGet( application );

            if ( string.IsNullOrEmpty( cs.ConnectionString ) == true )
                throw new DatabaseToolException( ER.ConnectionString_Empty, application );

            if ( cs.ConnectionString.StartsWith( "MASTER" ) == true )
                throw new DatabaseToolException( ER.ConnectionString_NotTransformed, application );

            if ( cs.ConnectionString.Contains( "AUTOMATION(" ) == true )
                throw new DatabaseToolException( ER.ConnectionString_Placeholder, application );


            /*
             * 
             */
            Type t = assembly.GetTypes().Where( x => x.FullName.EndsWith( ".Program" ) ).FirstOrDefault();
            string baseNamespace = t.Namespace;
            logger.Debug( "Base namespace: " + baseNamespace );


            /*
             * Reset
             */
            if ( operation.HasFlag( DbOperation.Reset ) == true )
            {
                logger.Info<AuditEventException>( ER.Reset_Run );

                if ( environment == "PROD" || environment == "PRD" )
                    throw new DatabaseToolException( ER.Reset_Production );

                if ( environment == "STAGE" || environment == "QUA" )
                    throw new DatabaseToolException( ER.Reset_QualityAssurance );

                var content = Assembly.GetExecutingAssembly().LoadEmbeddedResource( typeof( DbRunner ), "Utils.DatabaseReset.sql" );
                var script = new SqlScript( "DatabaseReset.sql", content );

                var resetter = DeployChanges.To
                    .SqlDatabase( cs.ConnectionString )
                    .WithScript( script )
                    .JournalTo( new NullJournal() )
                    .LogTo( appLogger )
                    .Build();

                var result = resetter.PerformUpgrade();

                if ( result.Successful == false )
                {
                    DatabaseToolException dte = new DatabaseToolException( ER.Reset_Failed, result.Error );

                    logger.Fatal( dte );
                    return dte.Code;
                }
            }


            /*
             * Schema
             */
            if ( operation.HasFlag( DbOperation.Schema ) == true )
            {
                logger.Info<AuditEventException>( ER.Schema_Run );

                string resxPrefix = baseNamespace + ".Schema.";

                var upgrader = DeployChanges.To
                    .SqlDatabase( cs.ConnectionString )
                    .WithScriptsEmbeddedInAssembly( assembly, ( string f ) => f.StartsWith( resxPrefix, StringComparison.InvariantCulture ) )
                    .LogTo( appLogger )
                    .Build();

                var result = upgrader.PerformUpgrade();
                string scriptList = string.Join( "\n", result.Scripts.Select( x => x.Name ) );

                if ( result.Successful == false )
                {
                    DatabaseToolException dte = new DatabaseToolException( ER.Schema_Failed, result.Error, scriptList );

                    logger.Fatal( dte );
                    return dte.Code;
                }
                else
                {
                    logger.Info<AuditEventException>( ER.Schema_Complete, scriptList );
                }
            }


            /*
             * 
             */
            if ( operation.HasFlag( DbOperation.Data ) == true )
            {
                logger.Info<AuditEventException>( ER.Data_Run );
            }

            return 0;
        }
    }
}