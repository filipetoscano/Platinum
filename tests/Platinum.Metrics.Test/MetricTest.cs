using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Config;
using System;
using System.IO;

namespace Platinum.Metrics.Test
{
    [TestClass]
    public class MetricTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod]
        public void TestMethod1()
        {
            LogManager.Configuration = Config();

            logger.Info( "Start: TestMethod1" );

            SampleMeasure m = new SampleMeasure()
            {
                Partition = "0",
                MessageCount = 10,
                DataPointCount = 60,
                Duration = 450
            };

            Metric.Gauge<SampleMeasure>( m );

            Metric.Gauge<SampleMeasure>( new SampleMeasure()
            {
                Partition = "0",
                MessageCount = 10,
                DataPointCount = 60,
                Duration = 450,
            } );

            logger.Info( "End: TestMethod1" );
        }


        private static LoggingConfiguration Config()
        {
            string path = Path.Combine( Environment.CurrentDirectory, @"..\..\NLog.config" );

            return new XmlLoggingConfiguration( path );
        }
    }
}
