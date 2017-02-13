using Dapper;
using System.Data;

namespace Platinum.Data.TypeHandler
{
    /// <summary>
    /// Ensures that all dates read from the database, are automatically
    /// read as UTC.
    /// </summary>
    public class DurationHandler : SqlMapper.TypeHandler<Duration>
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
        public override void SetValue( IDbDataParameter parameter, Duration value )
        {
            parameter.DbType = DbType.String;
            parameter.Value = value.ToString();
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
        public override Duration Parse( object value )
        {
            string valueStr = (string) value;

            return Duration.Parse( valueStr );
        }
    }
}
