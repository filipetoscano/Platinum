using NLog;
using System.Dynamic;

namespace Platinum.Metrics
{
    public class Metric
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static void Increment( string measureName )
        {
        }


        public static void Decrement( string measureName )
        {
        }


        public static void Gauge<T>( T measure )
        {
            logger.Info( measure );
        }
    }
}
