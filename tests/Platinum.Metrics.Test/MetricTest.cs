using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace Platinum.Metrics.Test
{
    [TestClass]
    public class MetricTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        [TestMethod]
        public void TestMethod1()
        {
            logger.Info( "Start: TestMethod1" );

            SampleMeasure m = new SampleMeasure()
            {
                Partition = "0",
                MessageCount = 10,
                DataPointCount = 60,
                Duration = 450
            };

            Metric.Write( m );

            logger.Info( "End: TestMethod1" );
        }
    }
}
