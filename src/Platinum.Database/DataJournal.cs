using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Support.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Platinum.Database
{
    /// <summary>
    /// An implementation of the <see cref="IJournal"/> interface which tracks version numbers for a 
    /// SQL Server database using a table called dbo.DataVersions.
    /// </summary>
    public class DataJournal : IJournal
    {
        private readonly Func<IConnectionManager> connectionManager;
        private readonly Func<IUpgradeLog> log;
        private readonly string schema;
        private readonly string table;


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTableJournal"/> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager.</param>
        /// <param name="logger">The log.</param>
        /// <param name="schema">The schema that contains the table.</param>
        /// <param name="table">The table name.</param>
        public DataJournal( Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table )
        {
            this.schema = schema;
            this.table = table;
            this.connectionManager = connectionManager;

            log = logger;
        }


        /// <summary>
        /// Recalls the version number of the database.
        /// </summary>
        /// <returns>All executed scripts.</returns>
        public string[] GetExecutedScripts()
        {
            log().WriteInformation( "Fetching list of already executed data scripts." );

            var exists = DoesTableExist();
            if ( !exists )
            {
                log().WriteInformation( string.Format( "The {0} table could not be found. The database data is assumed to be at version 0.", CreateTableName( schema, table ) ) );
                return new string[ 0 ];
            }

            var scripts = new List<string>();
            connectionManager().ExecuteCommandsWithManagedConnection( dbCommandFactory =>
            {
                using ( var command = dbCommandFactory() )
                {
                    command.CommandText = GetExecutedScriptsSql( schema, table );
                    command.CommandType = CommandType.Text;

                    using ( var reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                            scripts.Add( (string) reader[ 0 ] );
                    }
                }
            } );

            return scripts.ToArray();
        }


        /// <summary>
        /// Create an SQL statement which will retrieve all executed scripts in order.
        /// </summary>
        protected virtual string GetExecutedScriptsSql( string schema, string table )
        {
            return string.Format( "select [ScriptName] + '#' + [Hash] from {0} order by [ScriptName]", CreateTableName( schema, table ) );
        }


        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="script">The script.</param>
        public void StoreExecutedScript( SqlScript script )
        {
            #region Validations

            if ( script == null )
                throw new ArgumentNullException( nameof( script ) );

            #endregion


            /*
             * 
             */
            var dbScript = script as DataSqlScript;

            if ( dbScript == null )
            {
                log().WriteWarning( string.Format( "Script {0} is not an instance of DataSqlScript: will not be saved", script.Name ) );
                return;
            }


            /*
             * 
             */
            var exists = DoesTableExist();
            if ( !exists )
            {
                log().WriteInformation( string.Format( "Creating the {0} table", CreateTableName( schema, table ) ) );

                connectionManager().ExecuteCommandsWithManagedConnection( dbCommandFactory =>
                {
                    using ( var command = dbCommandFactory() )
                    {
                        command.CommandText = CreateTableSql( schema, table );

                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }

                    log().WriteInformation( string.Format( "The {0} table has been created", CreateTableName( schema, table ) ) );
                } );
            }


            /*
             * 
             */
            connectionManager().ExecuteCommandsWithManagedConnection( dbCommandFactory =>
            {
                using ( var command = dbCommandFactory() )
                {
                    command.CommandText = string.Format( "insert into {0} (ScriptName, Hash, Applied) values (@scriptName, @hash, @applied)", CreateTableName( schema, table ) );

                    var scriptNameParam = command.CreateParameter();
                    scriptNameParam.ParameterName = "scriptName";
                    scriptNameParam.Value = dbScript.AtomicName;
                    command.Parameters.Add( scriptNameParam );

                    var hashParam = command.CreateParameter();
                    hashParam.ParameterName = "hash";
                    hashParam.Value = dbScript.Hash;
                    command.Parameters.Add( hashParam );

                    var appliedParam = command.CreateParameter();
                    appliedParam.ParameterName = "applied";
                    appliedParam.Value = DateTime.Now;
                    command.Parameters.Add( appliedParam );

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            } );
        }

        /// <summary>Generates an SQL statement that, when exectuted, will create the journal database table.</summary>
        /// <param name="schema">Desired schema name supplied by configuration or <c>NULL</c></param>
        /// <param name="table">Desired table name</param>
        /// <returns>A <c>CREATE TABLE</c> SQL statement</returns>
        protected virtual string CreateTableSql( string schema, string table )
        {
            #region Validations

            if ( table == null )
                throw new ArgumentNullException( nameof( table ) );

            #endregion

            var tableName = CreateTableName( schema, table );
            var primaryKeyConstraintName = CreatePrimaryKeyName( table );

            return string.Format( @"create table {0} (
	[Id] int identity(1,1) not null constraint {1} primary key,
	[ScriptName] nvarchar(255) not null,
	[Hash] nvarchar(255) not null,
	[Applied] datetime not null
)", tableName, primaryKeyConstraintName );
        }


        /// <summary>Combine the <c>schema</c> and <c>table</c> values into an appropriately-quoted identifier for the journal table.</summary>
        /// <param name="schema">Desired schema name supplied by configuration or <c>NULL</c></param>
        /// <param name="table">Desired table name</param>
        /// <returns>Quoted journal table identifier</returns>
        protected virtual string CreateTableName( string schema, string table )
        {
            #region Validations

            if ( table == null )
                throw new ArgumentNullException( nameof( table ) );

            #endregion

            return string.IsNullOrEmpty( schema )
                ? SqlObjectParser.QuoteSqlObjectName( table )
                : SqlObjectParser.QuoteSqlObjectName( schema ) + "." + SqlObjectParser.QuoteSqlObjectName( table );
        }


        /// <summary>Convert the <c>table</c> value into an appropriately-quoted identifier for the journal table's unique primary key.</summary>
        /// <param name="table">Desired table name</param>
        /// <returns>Quoted journal table primary key identifier</returns>
        protected virtual string CreatePrimaryKeyName( string table )
        {
            return SqlObjectParser.QuoteSqlObjectName( "PK_" + table + "_Id" );
        }


        /// <summary />
        private bool DoesTableExist()
        {
            return connectionManager().ExecuteCommandsWithManagedConnection( dbCommandFactory =>
            {
                try
                {
                    using ( var command = dbCommandFactory() )
                    {
                        return VerifyTableExistsCommand( command, table, schema );
                    }
                }
                catch ( SqlException )
                {
                    return false;
                }
                catch ( DbException )
                {
                    return false;
                }
            } );
        }


        /// <summary>Verify, using database-specific queries, if the table exists in the database.</summary>
        /// <param name="command">The <c>IDbCommand</c> to be used for the query</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="schemaName">The schema for the table</param>
        /// <returns>True if table exists, false otherwise</returns>
        protected virtual bool VerifyTableExistsCommand( IDbCommand command, string tableName, string schemaName )
        {
            #region Validations

            if ( command == null )
                throw new ArgumentNullException( nameof( command ) );

            if ( tableName == null )
                throw new ArgumentNullException( nameof( tableName ) );

            #endregion

            command.CommandText = string.IsNullOrEmpty( schema )
                            ? string.Format( "select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '{0}'", tableName )
                            : string.Format( "select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '{0}' and TABLE_SCHEMA = '{1}'", tableName, schemaName );
            command.CommandType = CommandType.Text;

            var result = command.ExecuteScalar() as int?;
            return result == 1;
        }
    }
}