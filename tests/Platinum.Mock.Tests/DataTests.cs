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

            decimal d;

            Assert.AreEqual<bool>( true, instance.BooleanProperty );
            Assert.AreEqual<byte>( 11, instance.ByteProperty );
            Assert.AreEqual<char>( 'c', instance.CharProperty );
            Assert_AreEqual( 2003, 3, 3, 0, 0, 0, instance.DateProperty );
            Assert_AreEqual( 2003, 3, 3, 3, 3, 3, instance.DateTimeProperty );
            Assert.AreEqual<decimal>( 42.42m, instance.DecimalProperty );
            Assert.AreEqual<double>( 42.42, instance.DoubleProperty );
            Assert.AreEqual( EnumEnumerate.Value2, instance.EnumProperty );
            Assert.AreEqual<int>( 42, instance.IntegerProperty );
            Assert.AreEqual<long>( 42, instance.LongProperty );
            Assert.AreEqual<short>( 42, instance.ShortProperty );
            Assert.AreEqual<float>( 42.42f, instance.SingleProperty );
            Assert.AreEqual( "hhgg", instance.StringProperty );
            Assert_AreEqual( 1970, 1, 1, 4, 4, 4, instance.TimeProperty );
        }


        [TestMethod]
        public void MatrixCharacter()
        {
            var instance = Mocker.Mock<MatrixCharacter>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.Name );
            Assert.IsTrue( instance.Name.StartsWith( "string", StringComparison.OrdinalIgnoreCase ) == false );
        }
    }
}
