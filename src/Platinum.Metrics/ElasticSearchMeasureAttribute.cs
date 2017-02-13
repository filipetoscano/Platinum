using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Metrics
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false )]
    public class ElasticSearchMeasureAttribute : Attribute
    {
        public ElasticSearchMeasureAttribute( string index )
        {
            #region Validations

            if ( index == null )
                throw new ArgumentNullException( nameof( index ) );

            #endregion

            this.Index = index;
        }



        public string Index
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets/sets the document type name.
        /// </summary>
        public string DocumentType
        {
            get;
            set;
        }
    }
}
