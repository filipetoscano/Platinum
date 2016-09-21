using Dapper;
using System.Data;
using System.Data.Common;

namespace Platinum.Data
{
    /// <summary />
    public static class Extensions
    {
        /// <summary>
        /// Execute parameterized SQL that selects a single string value,
        /// and converts into enumerate value.
        /// </summary>
        /// <typeparam name="T">Enumerate type.</typeparam>
        /// <param name="cnn">Connection.</param>
        /// <param name="command">Command.</param>
        /// <returns>Value of first selected cell, as enumerate.</returns>
        public static T ExecuteEnum<T>( this DbConnection cnn, CommandDefinition command )
        {
            string scalar = cnn.ExecuteScalar<string>( command );

            return Enumerate.Parse<T>( scalar );
        }


        /// <summary>
        /// Execute parameterized SQL that selects a single string value,
        /// and converts into enumerate value.
        /// </summary>
        /// <typeparam name="T">Enumerate type.</typeparam>
        /// <param name="cnn">Connection.</param>
        /// <param name="sql">SQL statement/command.</param>
        /// <param name="param">Parameters.</param>
        /// <param name="transaction">Whether command participates in transaction.</param>
        /// <param name="commandTimeout">Command timeout.</param>
        /// <param name="commandType">Type of command.</param>
        /// <returns>Value of first selected cell, as enumerate.</returns>
        public static T ExecuteEnum<T>( this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default( int? ), CommandType? commandType = default( CommandType? ) )
        {
            string scalar = cnn.ExecuteScalar<string>( sql, param, transaction, commandTimeout, commandType );

            return Enumerate.Parse<T>( scalar );
        }
    }
}
