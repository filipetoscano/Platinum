using DbUp.Engine;
using System;

namespace Platinum.Database
{
    /// <summary />
    public class DataSqlScript : SqlScript
    {
        /// <summary />
        public DataSqlScript( string name, string hash, string contents )
            : base( name + "#" + hash, contents )
        {
            #region Validations

            if ( name == null )
                throw new ArgumentNullException( nameof( name ) );

            if ( hash == null )
                throw new ArgumentNullException( nameof( hash ) );

            #endregion

            this.AtomicName = name;
            this.Hash = hash;
        }


        /// <summary />
        public string AtomicName
        {
            get;
            private set;
        }


        /// <summary />
        public string Hash
        {
            get;
            private set;
        }
    }
}
