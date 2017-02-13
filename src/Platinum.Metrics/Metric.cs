using NLog;

namespace Platinum.Metrics
{
    public class Metric
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static void Gauge( object measure )
        {
            logger.Info( measure );
        }


        public static void Gauge<T>( T measure )
        {
            logger.Info( measure );
        }
    }
}
