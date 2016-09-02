using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Platinum.Mock.Tests
{
    [TestClass]
    public class DataTests : TestBase
    {
        [TestMethod]
        public void BasicDataClass()
        {
            var instance = Mocker.Mock<BasicDataClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            // Where possible :)
            Assert.IsNotNull( instance );

            Assert.AreEqual( true, instance.BooleanProperty );
            Assert.AreEqual( 'b', instance.ByteProperty );
            Assert.AreEqual( 'c', instance.CharProperty );
            Assert_AreEqual( 2003, 3, 3, 0, 0, 0, instance.DateProperty );
            Assert_AreEqual( 2003, 3, 3, 3, 3, 3, instance.DateTimeProperty );
            Assert.AreEqual( 42.42, instance.DecimalProperty );
            Assert.AreEqual( 42.42, instance.DoubleProperty );
            Assert.AreEqual( EnumEnumerate.Value2, instance.EnumProperty );
            Assert.AreEqual( 42, instance.IntegerProperty );
            Assert.AreEqual( 42, instance.LongProperty );
            Assert.AreEqual( 42, instance.ShortProperty );
            Assert.AreEqual( 42.42, instance.SingleProperty );
            Assert.AreEqual( "hhgg", instance.StringProperty );
            Assert_AreEqual( 1970, 1, 1, 3, 3, 3, instance.DateTimeProperty );
        }
    }
}
