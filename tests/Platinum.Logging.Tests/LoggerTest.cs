using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Logging.Tests
{
    [TestClass]
    public class LoggerTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [TestMethod]
        public void TestMethod1()
        {
            logger.Event( "" );
        }
    }
}
