using DbUp.Engine;
using DbUp.Engine.Transactions;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Platinum.Database
{
    public class DirectScriptProvider : IScriptProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary />
        public DirectScriptProvider( Assembly assembly, string resxPrefix )
        {
            #region Validations

            if ( assembly == null )
                throw new ArgumentNullException( nameof( assembly ) );

            if ( resxPrefix == null )
                throw new ArgumentNullException( nameof( resxPrefix ) );

            #endregion

            this.Assembly = assembly;
            this.ResourcePrefix = resxPrefix;
        }


        /// <summary />
        public Assembly Assembly
        {
            get;
            private set;
        }


        /// <summary />
        public string ResourcePrefix
        {
            get;
            private set;
        }


        /// <summary />
        public IEnumerable<SqlScript> GetScripts( IConnectionManager connectionManager )
        {
            try
            {
                return GetScripts();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                throw;
            }
        }



        /// <summary />
        private List<SqlScript> GetScripts()
        {
            /*
             * 
             */
            List<SqlScript> scripts = new List<SqlScript>();

            var resources = this.Assembly.GetManifestResourceNames().Where( x => x.StartsWith( this.ResourcePrefix ) == true );

            if ( resources.Count() == 0 )
                return scripts;


            /*
             * 
             */
            foreach ( var resx in resources )
            {
                string content;

                using ( var stream = this.Assembly.GetManifestResourceStream( resx ) )
                {
                    using ( var reader = new StreamReader( stream ) )
                    {
                        content = reader.ReadToEnd();
                    }
                }

                string name = resx;
                string hash = Platinum.Cryptography.Hash.SHA256( content );

                var script = new DataSqlScript( name, hash, content );

                scripts.Add( script );
            }

            return scripts;
        }
    }
}
