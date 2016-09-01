using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Platinum.Data
{
    /// <summary>
    /// Wrapper around concrete <see cref="DbCommand" /> implementation, which
    /// preserves a link to the logical command-name.
    /// </summary>
    public class DataCommand : DbCommand
    {
        private string _conn;
        private string _name;
        private DbCommand _command;


        /// <summary>
        /// Initializes a new instance of 
        /// </summary>
        /// <param name="connection">Logical name of the connection.</param>
        /// <param name="command"></param>
        internal DataCommand( string connection, DbCommand command )
        {
            #region Validations

            if ( connection == null )
                throw new ArgumentNullException( nameof( connection ) );

            if ( command == null )
                throw new ArgumentNullException( nameof( command ) );

            #endregion

            _conn = connection;
            _name = PeekCommandName( command.CommandText );
            _command = command;
        }


        /// <summary>
        /// Gets the logical name of the connection.
        /// </summary>
        public string ConnectionName
        {
            get { return _conn; }
        }


        /// <summary>
        /// Gets the logical name of the command.
        /// </summary>
        public string CommandName
        {
            get { return _name; }
        }


        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        [SuppressMessage( "Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities" )]
        public override string CommandText
        {
            get { return _command.CommandText; }

            set
            {
                _name = PeekCommandName( value );
                _command.CommandText = value;
            }
        }


        /// <summary>
        /// Extracts the command-name from the command-text.
        /// </summary>
        /// <param name="commandText">Command text.</param>
        /// <returns>Logical name of the command.</returns>
        private static string PeekCommandName( string commandText )
        {
            if ( string.IsNullOrEmpty( commandText ) == true )
                return "(undef)";

            if ( commandText.StartsWith( "/*#", StringComparison.Ordinal ) == false )
                return "(undef)";

            int ix = commandText.IndexOf( " #*/", StringComparison.Ordinal );

            if ( ix == -1 )
                return "(undef)";

            return commandText.Substring( 4, ix - 4 );
        }


        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command
        /// and generating an error.
        /// </summary>
        public override int CommandTimeout
        {
            get { return _command.CommandTimeout; }
            set { _command.CommandTimeout = value; }
        }


        /// <summary>
        /// Indicates or specifies how the <see cref="DataCommand.CommandText" /> property
        /// is interpreted.
        /// </summary>
        public override CommandType CommandType
        {
            get { return _command.CommandType; }
            set { _command.CommandType = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible
        /// in a customized interface control.
        /// </summary>
        public override bool DesignTimeVisible
        {
            get { return _command.DesignTimeVisible; }
            set { _command.DesignTimeVisible = value; }
        }


        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="DataRow" /> when
        /// used by the Update method of a <see cref="DbDataAdapter" />.
        /// </summary>
        public override UpdateRowSource UpdatedRowSource
        {
            get { return _command.UpdatedRowSource; }
            set { _command.UpdatedRowSource = value; }
        }


        /// <summary>
        /// Gets or sets the <see cref="DbConnection" /> used by this <see cref="DbCommand" />.
        /// </summary>
        protected override DbConnection DbConnection
        {
            get { return _command.Connection; }
            set { _command.Connection = value; }
        }


        /// <summary>
        /// Gets the collection of <see cref="DbParameter" /> objects.
        /// </summary>
        protected override DbParameterCollection DbParameterCollection
        {
            get { return _command.Parameters; }
        }


        /// <summary>
        /// Gets or sets the <see cref="DbTransaction"/> within which this
        /// <see cref="DbCommand" /> object executes.
        /// </summary>
        protected override DbTransaction DbTransaction
        {
            get { return _command.Transaction; }
            set { _command.Transaction = value; }
        }


        /// <summary>
        /// Attempts to cancels the execution of a <see cref="DbCommand" />.
        /// </summary>
        public override void Cancel()
        {
            _command.Cancel();
        }


        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>
        /// The number of rows affected.
        /// </returns>
        public override int ExecuteNonQuery()
        {
            try
            {
                return _command.ExecuteNonQuery();
            }
            catch ( DbException ex )
            {
                throw new DataException( ER.ExecuteNonQuery, ex, _conn, _name );
            }
        }


        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the result set.
        /// </returns>
        public override object ExecuteScalar()
        {
            try
            {
                return _command.ExecuteScalar();
            }
            catch ( DbException ex )
            {
                throw new DataException( ER.ExecuteScalar, ex, _conn, _name );
            }
        }


        /// <summary>
        /// Executes the command text against the connection.
        /// </summary>
        /// <param name="behavior">
        /// An instance of <see cref="CommandBehavior" />.
        /// </param>
        /// <returns>
        /// A task representing the operation.
        /// </returns>
        protected override DbDataReader ExecuteDbDataReader( CommandBehavior behavior )
        {
            try
            {
                return _command.ExecuteReader( behavior );
            }
            catch ( DbException ex )
            {
                throw new DataException( ER.ExecuteDbDataReader, ex, _conn, _name );
            }
        }


        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        public override void Prepare()
        {
            _command.Prepare();
        }


        /// <summary>
        /// Creates a new instance of a <see cref="DbParameter" /> object.
        /// </summary>
        /// <returns>
        /// A <see cref="DbParameter" /> object.
        /// </returns>
        protected override DbParameter CreateDbParameter()
        {
            return _command.CreateParameter();
        }
    }
}
