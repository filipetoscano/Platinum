using NLog;
using System;
using System.Dynamic;

namespace Platinum.Metrics
{
    /// <summary />
    public class Meter
    {
        private readonly Logger _logger;
        private readonly string _measureName;


        /// <summary />
        internal Meter( Logger logger, string measureName )
        {
            #region Validations

            if ( logger == null )
                throw new ArgumentNullException( nameof( logger ) );

            if ( measureName == null )
                throw new ArgumentNullException( nameof( measureName ) );

            #endregion

            _logger = logger;
            _measureName = measureName;
        }


        /// <summary />
        public void Mark()
        {
            dynamic measure = new ExpandoObject();
            measure.Value = 1;

            _logger.Info<dynamic>( _measureName, measure );
        }
    }
}
