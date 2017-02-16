using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Core.Tests.ExceptionFwk
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void Count0()
        {
            var ex = new TestException( ER.Count0 );

            Assert.AreEqual( ex.Actor, App.Name );
            Assert.AreEqual( ex.Code, 1000 );
        }


        [TestMethod]
        public void Count1_Ok()
        {
            int arg1 = 1337;

            var ex = new TestException( ER.Count1, arg1 );

            Assert.AreEqual( ex.Actor, App.Name );
            Assert.AreEqual( ex.Code, 1001 );
            Assert.AreEqual( ex.Data.Contains( "Arg1" ), true );
            Assert.AreEqual( ex.Data[ "Arg1" ], arg1 );
        }


        [TestMethod]
        public void Count1_TooManyArgs()
        {
            int arg1 = 1337;
            int arg2 = 2005;

            var ex = new TestException( ER.Count1, arg1, arg2 );

            Assert.AreEqual( ex.Actor, App.Name );
            Assert.AreEqual( ex.Code, 1001 );
            Assert.AreEqual( ex.Data.Contains( "Arg1" ), true );
            Assert.AreEqual( ex.Data[ "Arg1" ], arg1 );
            Assert.AreEqual( ex.Data.Contains( "Arg2" ), false );
        }
    }
}
