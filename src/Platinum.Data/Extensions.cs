using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

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


        /// <summary>
        /// Converts an enumeration into a SQL Server TVP.
        /// </summary>
        /// <param name="rows">
        /// Sets of dynamic rows.
        /// </param>
        /// <param name="typeName">
        /// Name of the SQL Server TVP.
        /// </param>
        /// <returns></returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>( this IEnumerable<T> rows, string typeName )
        {
            /*
             * 
             */
            DataTable dt = new DataTable();
            List<PropertyInfo> properties = new List<PropertyInfo>();

            Type rowType = rows.GetType().GenericTypeArguments.Last();

            foreach ( var p in rowType.GetProperties() )
            {
                dt.Columns.Add( p.Name, p.PropertyType );
                properties.Add( p );
            }


            /*
             * 
             */
            foreach ( var row in rows )
            {
                DataRow r = dt.NewRow();

                foreach ( var prop in properties )
                {
                    r[ prop.Name ] = prop.GetValue( row );
                }

                dt.Rows.Add( r );
            }

            return dt.AsTableValuedParameter( typeName );
        }
    }
}
