using NLog;
using System;
using System.Dynamic;

namespace Platinum.Metrics
{
    public class Metric
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static Meter Meter( string measureName )
        {
            #region Validations

            if ( measureName == null )
                throw new ArgumentNullException( nameof( measureName ) );

            #endregion

            return new Metrics.Meter( logger, measureName );
        }


        public static void Gauge<T>( T measure )
        {
            #region Validations

            if ( measure == null )
                throw new ArgumentNullException( nameof( measure ) );

            #endregion

            logger.Info( typeof( T ).Name, measure );
        }
    }
}
