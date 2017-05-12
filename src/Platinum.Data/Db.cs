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
        /// <summary>
        /// Returns a new (non-open) connection, that has been configured
        /// in the application file with @name.
        /// </summary>
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

            try
            {
                db.ConnectionString = cs.ConnectionString;
            }
            catch ( ArgumentException ex )
            {
                throw new DataException( ER.Connection_ConnectionString, ex, name );
            }

            return db;
        }


        /// <summary>
        /// Loads a statement that has been embedded into the calling assembly.
        /// </summary>
        /// <remarks>
        /// This method will also perform the following actions:
        /// - Ignore all content before the --// marker. This allows SQL files to
        ///   be locally executable as well as by Dapper;
        /// - Injects some metadata at the top of the statement, which will then
        ///   be picked up by the DataCommand instance.
        /// </remarks>
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

            string assemblyName = assembly.FullName.Split( ',' )[ 0 ];
            string fqName = assemblyName + "." + commandName.Replace( "/", "." );

            string resourceName = fqName + ".sql";
            Stream stream = assembly.GetManifestResourceStream( resourceName );

            if ( stream == null )
                throw new DataException( ER.Command_CommandNotFound, commandName, assemblyName, resourceName );

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
            string prefix = $"/*# { assemblyName }#{ commandName } #*/";


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
