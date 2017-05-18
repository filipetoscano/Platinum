using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace Platinum.Logging.Tests
{
    [TestClass]
    public class LoggingTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        [TestMethod]
        public void Error_ActorException()
        {
            var ex = new TestException( ER.Exception_NoArguments );
            logger.Fatal( ex );
        }


        [TestMethod]
        public void Error_ActorExceptionWithMessage()
        {
            var ex = new TestException( ER.Exception_NoArguments );

            logger.Error( ex, "Error_ActorExceptionWithMessage: Message and exception." );
        }


        [TestMethod]
        public void Debug_String()
        {
            logger.Debug( "Debug_String: Raw debug" );
        }
    }
}
