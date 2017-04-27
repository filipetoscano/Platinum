using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Platinum.Configuration;
using Platinum.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Platinum.Database
{
    /// <summary />
    public class DbRunner
    {
        private List<IScriptProvider> _data = new List<IScriptProvider>();


        /// <summary>
        /// Adds an additional (specific) data provider.
        /// </summary>
        /// <typeparam name="T">Typf of script provider.</typeparam>
        public void AddDataProvider<T>() where T : IScriptProvider
        {
            T instance = Activator.Create<T>();

            _data.Add( instance );
        }


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
                exitCode = Run( assembly, logger, operation, _data );
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
        private static int Run( Assembly assembly, Logger logger, DbOperation operation, List<IScriptProvider> custom )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( logger == null )
                throw new ArgumentNullException( nameof( logger ) );

            if ( custom == null )
                throw new ArgumentNullException( nameof( custom ) );

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
                logger.Debug( "Resetting database." );

                if ( environment == "PROD" || environment == "PRD" || environment == "PRE" )
                    throw new DatabaseToolException( ER.Reset_Production );

                if ( environment == "STAGE" || environment == "QUA" )
                    throw new DatabaseToolException( ER.Reset_QualityAssurance );

                var content = Assembly.GetExecutingAssembly().LoadEmbeddedResource( typeof( DbRunner ), "Scripts.DatabaseReset.sql" );
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
                    DatabaseToolException dte = new DatabaseToolException( EV.Reset_Failed, result.Error );
                    Audit.Event( dte );

                    return dte.Code;
                }
                else
                {
                    Audit.Event( EV.Reset_Complete );
                }
            }


            /*
             * Schema
             */
            if ( operation.HasFlag( DbOperation.Schema ) == true )
            {
                logger.Debug( "Running schema files." );

                string resxPrefix = baseNamespace + ".Schema.";

                var upgrader = DeployChanges.To
                    .SqlDatabase( cs.ConnectionString )
                    .WithScriptsEmbeddedInAssembly( assembly, ( string f ) => f.StartsWith( resxPrefix, StringComparison.InvariantCulture ) )
                    .LogTo( appLogger )
                    .Build();

                var result = upgrader.PerformUpgrade();

                if ( result.Scripts.Count() > 0 )
                {
                    foreach ( var script in result.Scripts )
                    {
                        Audit.Event( EV.Schema_Executed, script.Name );
                    }
                }

                if ( result.Successful == false )
                {
                    string scriptName = result.ErrorScript();
                    DatabaseToolException dte = new DatabaseToolException( EV.Schema_Failed, result.Error, scriptName );
                    Audit.Event( dte );

                    return dte.Code;
                }
            }


            /*
             * Data
             */
            if ( operation.HasFlag( DbOperation.Data ) == true )
            {
                logger.Debug( "Running data files." );

                string resxPrefix = baseNamespace + ".Data.";

                var db = DeployChanges.To;

                var upgrader = DeployChanges.To
                    .SqlDatabase( cs.ConnectionString )
                    .WithScripts( new DbConfigScriptProvider( assembly, resxPrefix, environment ) )
                    .LogTo( appLogger )
                    .JournalToData( "dbo", "DataVersions" )
                    .Build();

                var result = upgrader.PerformUpgrade();
                string scriptList = string.Join( "\n", result.Scripts.Select( x => x.Name ) );

                if ( result.Scripts.Count() > 0 )
                {
                    foreach ( var script in result.Scripts )
                    {
                        Audit.Event( EV.Data_Executed, script.Name );
                    }
                }

                if ( result.Successful == false )
                {
                    string scriptName = result.ErrorScript();
                    DatabaseToolException dte = new DatabaseToolException( EV.Data_Failed, result.Error, scriptName );
                    Audit.Event( dte );

                    return dte.Code;
                }
            }


            /*
             * Custom data
             */
            if ( operation.HasFlag( DbOperation.Data ) == true && custom.Count() > 0 )
            {
                foreach ( var provider in custom )
                {
                    string providerType = provider.GetType().FullName;
                    logger.Debug( "Running '{0}'.", providerType );

                    var upgrader = DeployChanges.To
                        .SqlDatabase( cs.ConnectionString )
                        .WithScripts( provider )
                        .LogTo( appLogger )
                        .JournalToData( "dbo", "DataVersions" )
                        .Build();

                    var result = upgrader.PerformUpgrade();
                    string scriptList = string.Join( "\n", result.Scripts.Select( x => x.Name ) );

                    if ( result.Scripts.Count() > 0 )
                    {
                        foreach ( var script in result.Scripts )
                        {
                            Audit.Event( EV.CustomData_Executed, providerType, script.Name );
                        }
                    }

                    if ( result.Successful == false )
                    {
                        string scriptName = result.ErrorScript();
                        DatabaseToolException dte = new DatabaseToolException( EV.CustomData_Failed, result.Error, providerType, scriptName );
                        Audit.Event( dte );

                        return dte.Code;
                    }
                }
            }

            return 0;
        }
    }
}