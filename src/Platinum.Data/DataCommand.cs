using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Platinum.Data
{
    public class DataCommand : DbCommand
    {
        private string _conn;
        private DbCommand _command;


        public DataCommand( DataConnection conn, DbCommand command )
        {
            #region Validations

            if ( conn == null )
                throw new ArgumentNullException( nameof( conn ) );

            if ( command == null )
                throw new ArgumentNullException( nameof( command ) );

            #endregion

            _conn = conn.Name;
            _command = command;
        }


        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        [SuppressMessage( "Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities" )]
        public override string CommandText
        {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
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
            return _command.ExecuteNonQuery();
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
            return _command.ExecuteScalar();
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
            return _command.ExecuteReader( behavior );
        }
    }
}
