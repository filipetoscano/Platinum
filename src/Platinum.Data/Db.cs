using Platinum.Configuration;
using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Platinum.Data
{
    /// <summary />
    public static class Db
    {
        /// <summary />
        public static DataConnection Connection( string name )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            #endregion


            /*
             * 
             */
            var cs = AppConfiguration.ConnectionGet( name );

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


        /// <summary />
        public static string Command( string commandName )
        {
            #region Validations

            if ( commandName == null )
                throw new ArgumentNullException( nameof( commandName ) );

            #endregion

            /*
             * 
             */
            Assembly assembly = Assembly.GetCallingAssembly();
            string fqName = assembly.FullName.Split( ',' )[ 0 ] + "." + commandName.Replace( "/", "." );
            string resourceName = fqName + ".sql";
            Stream stream = assembly.GetManifestResourceStream( resourceName );

            if ( stream == null )
                throw new DataException( ER.Command_CommandNotFound, commandName, assembly.FullName, resourceName );

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
            string prefix = $"/*# { fqName } #*/";


            /*
             * 
             */
            int ix = sql.IndexOf( "--//" );

            if ( ix > -1 )
                return prefix + sql.Substring( ix + 4 );
            else
                return prefix + sql;
        }
    }
}
