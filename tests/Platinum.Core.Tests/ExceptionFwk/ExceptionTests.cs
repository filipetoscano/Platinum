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

            Assert.AreEqual( nameof( ER.Count0 ), ex.Message );
            Assert.AreEqual( App.Name, ex.Actor );
            Assert.AreEqual( 1000, ex.Code );
            Assert.AreEqual( "Message with no placeholders.", ex.Description );

            Assert.AreEqual( 0, ex.Data.Count );
        }


        [TestMethod]
        public void Count1_Ok()
        {
            int arg1 = 1337;

            var ex = new TestException( ER.Count1, arg1 );

            Assert.AreEqual( nameof( ER.Count1 ), ex.Message );
            Assert.AreEqual( App.Name, ex.Actor );
            Assert.AreEqual( 1001, ex.Code );
            Assert.AreEqual( "Message with single placeholder: 1337.", ex.Description );

            Assert.AreEqual( 1, ex.Data.Count );
            Assert.AreEqual( true, ex.Data.Contains( "Arg1" ) );
            Assert.AreEqual( arg1, ex.Data[ "Arg1" ] );
        }


        [TestMethod]
        public void Count1_TooManyArgs()
        {
            int arg1 = 1338;
            int arg2 = 2005;

            var ex = new TestException( ER.Count1, arg1, arg2 );

            Assert.AreEqual( nameof( ER.Count1 ), ex.Message );
            Assert.AreEqual( App.Name, ex.Actor );
            Assert.AreEqual( 1001, ex.Code );
            Assert.AreEqual( "Message with single placeholder: 1338.", ex.Description );

            Assert.AreEqual( 1, ex.Data.Count );
            Assert.AreEqual( true, ex.Data.Contains( "Arg1" ) );
            Assert.AreEqual( arg1, ex.Data[ "Arg1" ] );
        }


        [TestMethod]
        public void Count2_Ok()
        {
            int arg1 = 1337;
            string arg2 = "xpto";

            var ex = new TestException( ER.Count2, arg1, arg2 );

            Assert.AreEqual( nameof( ER.Count2 ), ex.Message );
            Assert.AreEqual( App.Name, ex.Actor );
            Assert.AreEqual( 1002, ex.Code );
            Assert.AreEqual( "Message with two placeholders: 1337 - xpto.", ex.Description );

            Assert.AreEqual( 2, ex.Data.Count );
            Assert.AreEqual( true, ex.Data.Contains( "Arg1" ) );
            Assert.AreEqual( arg1, ex.Data[ "Arg1" ] );
            Assert.AreEqual( true, ex.Data.Contains( "Arg2" ) );
            Assert.AreEqual( arg2, ex.Data[ "Arg2" ] );
        }


        [TestMethod]
        public void FatalLevel_Ok()
        {
            var ex = new TestException( ER.FatalLevel );

            Assert.AreEqual( nameof( ER.FatalLevel ), ex.Message );
            Assert.AreEqual( App.Name, ex.Actor );
            Assert.AreEqual( 3001, ex.Code );
            Assert.AreEqual( "Message with fatal error.", ex.Description );

            Assert.AreEqual( 1, ex.Data.Count );
            Assert.AreEqual( true, ex.Data.Contains( "Pt.Level" ) );
            Assert.AreEqual( "Fatal", ex.Data[ "Pt.Level" ] );
        }
    }
}
