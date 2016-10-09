using Dapper;
using System;
using System.Data;

namespace Platinum.Data.TypeHandler
{
    /// <summary>
    /// Ensures that all dates read from the database, are automatically
    /// read as UTC.
    /// </summary>
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        /// <summary>
        /// Assign the value of a parameter before a command executes.
        /// </summary>
        /// <param name="parameter">
        /// The parameter to configure.
        /// </param>
        /// <param name="value">
        /// Parameter value.
        /// </param>
        public override void SetValue( IDbDataParameter parameter, DateTime value )
        {
            parameter.Value = value;
        }


        /// <summary>
        /// Parse a database value back to a typed value.
        /// </summary>
        /// <param name="value">
        /// The value from the database.
        /// </param>
        /// <returns>
        /// The typed value.
        /// </returns>
        public override DateTime Parse( object value )
        {
            return DateTime.SpecifyKind( (DateTime) value, DateTimeKind.Utc );
        }
    }
}
