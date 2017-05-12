using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Platinum.Data
{
    /// <summary>
    /// Wrapper around concrete <see cref="DbConnection" /> implementation, which preserves
    /// the link to the logical configuration name.
    /// </summary>
    public partial class DataConnection : DbConnection
    {
        private string _name;
        private DbConnection _connection;


        /// <summary>
        /// Initializes a new instance of <see cref="DataConnection" /> with the
        /// given name and <see cref="DbConnection" /> instance.
        /// </summary>
        /// <param name="name">Logical name of connection, as defined in configuration
        /// settings.</param>
        /// <param name="conn">Connection instance.</param>
        public DataConnection( string name, DbConnection conn )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            if ( conn == null )
                throw new ArgumentNullException( nameof( conn ) );

            #endregion

            this._name = name;
            this._connection = conn;
        }


        /// <summary>
        /// Gets the name of the connection.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }


        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        public override string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }


        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database
        /// name specified in the connection string before the connection is opened.
        /// </summary>
        public override string Database
        {
            get { return _connection.Database; }
        }


        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        public override string DataSource
        {
            get { return _connection.DataSource; }
        }


        /// <summary>
        /// Gets a string that represents the version of the server to which the object is
        /// connected.
        /// </summary>
        public override string ServerVersion
        {
            get { return _connection.ServerVersion; }
        }


        /// <summary>
        /// Gets a string that describes the state of the connection.
        /// </summary>
        public override ConnectionState State
        {
            get { return _connection.State; }
        }


        /// <summary>
        /// Changes the current database for an open connection.
        /// </summary>
        /// <param name="databaseName">
        /// Specifies the name of the database for the connection to use.
        /// </param>
        public override void ChangeDatabase( string databaseName )
        {
            _connection.ChangeDatabase( databaseName );
        }


        /// <summary>
        /// Closes the connection to the database. This is the preferred method of closing
        /// any open connection.
        /// </summary>
        public override void Close()
        {
            _connection.Close();
        }


        /// <summary>
        /// Opens a database connection with the settings specified by the System.Data.Common.DbConnection.ConnectionString.
        /// </summary>
        public override void Open()
        {
            try
            {
                _connection.Open();
            }
            catch ( SqlException ex )
            {
                switch ( ex.Number )
                {
                    case -1:
                        throw new DataException( ER.Open_ConnectFailed, this.Name );

                    case 18456:
                        /*
                         * Read more information about 18456 states here:
                         * http://sqlblog.com/blogs/aaron_bertrand/archive/2011/01/14/sql-server-v-next-denali-additional-states-for-error-18456.aspx
                         * 
                         * Sadly, Express doesn't seem to return any of these better errors.
                         * The value of ex.State is always 1 :-(
                         */
                        throw new DataException( ER.Open_LoginFailed, ex, this.Name, ex.State );

                    case 4060:
                        throw new DataException( ER.Open_DatabaseInvalid, this.Name );

                    default:
                        throw new DataException( ER.Open_Failed, this.Name );
                }
            }
        }


        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the isolation level for the transaction.</param>
        /// <returns>An object representing the new transaction.</returns>
        protected override DbTransaction BeginDbTransaction( IsolationLevel isolationLevel )
        {
            return _connection.BeginTransaction( isolationLevel );
        }


        /// <summary>
        /// Creates and returns a <see cref="DbCommand"/> object associated with the
        /// current connection.
        /// </summary>
        /// <returns>A <see cref="DbCommand"/> object.</returns>
        [SuppressMessage( "Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities" )]
        protected override DbCommand CreateDbCommand()
        {
            var cmd = _connection.CreateCommand();
            return new DataCommand( this.Name, cmd );
        }
    }
}
