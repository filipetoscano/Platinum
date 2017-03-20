using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Logging.Tests
{
    [TestClass]
    public class AuditorTest
    {
        [TestMethod]
        public void Event_NoArguments()
        {
            Audit.Event( ER.Event_NoArguments );
        }


        [TestMethod]
        public void Event_OneArgument()
        {
            Audit.Event( ER.Event_OneArgument, 1 );
        }


        [TestMethod]
        public void Event_TwoArguments()
        {
            Audit.Event( ER.Event_TwoArguments, 2, "string" );
        }
    }
}
