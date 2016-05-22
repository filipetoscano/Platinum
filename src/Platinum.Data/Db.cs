using Platinum.Configuration;
using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Platinum.Data
{
    public static class Db
    {
        public static DataConnection Connection( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion


            /*
             * 
             */
            var cs = AppConfiguration.ConnectionStrings[ name ];

            if ( cs == null )
                throw new DataException( ER.Connection_NotFound, name );

            if ( string.IsNullOrEmpty( cs.ProviderName ) == true )
                throw new DataException( ER.Connection_NoProviderName, name );

            if ( string.IsNullOrEmpty( cs.ConnectionString ) == true )
                throw new DataException( ER.Connection_NoConnection, name );


            /*
             * 
             */
            DbProviderFactory factory = DbProviderFactories.GetFactory( cs.ProviderName );

            if ( factory == null )
                throw new DataException( ER.Connection_NoFactory, name, cs.ProviderName );


            /*
             * 
             */
            DbConnection conn = factory.CreateConnection();
            DataConnection db = new DataConnection( name, conn );
            db.ConnectionString = cs.ConnectionString;

            return db;
        }


        public static string Sql( string commandName )
        {
            #region Validations

            if ( commandName == null )
                throw new ArgumentNullException( nameof( commandName ) );

            #endregion

            /*
             * 
             */
            Assembly assembly = Assembly.GetCallingAssembly();
            string resourceName = assembly.FullName.Split( ',' )[ 0 ] + "." + commandName.Replace( "/", "." ) + ".sql";
            Stream stream = assembly.GetManifestResourceStream( resourceName );

            if ( stream == null )
                throw new DataException( ER.Sql_CommandNotFound, assembly.FullName, commandName, resourceName );

            /*
             * 
             */
            string sql;

            using ( StreamReader reader = new StreamReader( stream ) )
            {
                sql = reader.ReadToEnd();
            }

            /*
             * 
             */
            int ix = sql.IndexOf( "--//" );

            if ( ix > -1 )
                return sql.Substring( ix + 4 );
            else
                return sql;
        }
    }
}
