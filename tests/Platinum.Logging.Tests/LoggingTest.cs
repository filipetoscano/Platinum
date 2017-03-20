using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Logging.Tests
{
    [TestClass]
    public class LoggingTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        [TestMethod]
        public void Error_Exception()
        {
            var ex = new TestException( ER.Exception_NoArguments );
            logger.Error( ex );
        }


        [TestMethod]
        public void Debug_String()
        {
            logger.Debug( "hi im here" );
        }
    }
}
