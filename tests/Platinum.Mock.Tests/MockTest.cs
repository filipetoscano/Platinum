using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Platinum.Mock.Tests
{
    [TestClass]
    public class MockTest : TestBase
    {
        [TestMethod]
        public void BasicClass()
        {
            var instance = Mocker.Mock<BasicClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            // Where possible :)
            Assert.IsNotNull( instance );

            Assert.IsTrue( instance.DateTimeProperty != DateTime.MinValue );
            Assert.AreEqual( DateTimeKind.Utc, instance.DateTimeProperty.Kind );

            Assert.AreNotEqual( ' ', instance.CharProperty );

            Assert.IsNotNull( instance.StringProperty );
        }


        [TestMethod]
        public void DateAndTimeClass()
        {
            var instance = Mocker.Mock<DateAndTimeClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            // Where possible :)
            Assert.IsNotNull( instance );

            Assert.AreEqual( 0, instance.DateProperty.Hour );
            Assert.AreEqual( 0, instance.DateProperty.Minute );
            Assert.AreEqual( 0, instance.DateProperty.Second );
            Assert.AreEqual( DateTimeKind.Utc, instance.DateProperty.Kind );

            Assert.AreEqual( 1970, instance.TimeProperty.Year );
            Assert.AreEqual( 1, instance.TimeProperty.Month );
            Assert.AreEqual( 1, instance.TimeProperty.Day );
            Assert.AreEqual( DateTimeKind.Utc, instance.TimeProperty.Kind );
        }


        [TestMethod]
        public void BasicNullableClass()
        {
            var instance = Mocker.Mock<BasicNullableClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            // All must be set
            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.BooleanProperty );
            Assert.IsNotNull( instance.ByteProperty );
            Assert.IsNotNull( instance.ShortProperty );
            Assert.IsNotNull( instance.IntegerProperty );
            Assert.IsNotNull( instance.LongProperty );
            Assert.IsNotNull( instance.SingleProperty );
            Assert.IsNotNull( instance.DoubleProperty );
            Assert.IsNotNull( instance.DecimalProperty );
            Assert.IsNotNull( instance.DateTimeProperty );
            Assert.IsNotNull( instance.CharProperty );
            Assert.IsNotNull( instance.EnumProperty );
            Assert.IsNotNull( instance.FlagProperty );
        }


        [TestMethod]
        public void BasicArrayClass()
        {
            var instance = Mocker.Mock<BasicArrayClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            // All most be set
            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.BooleanProperty );
            Assert.IsNotNull( instance.ShortProperty );
            Assert.IsNotNull( instance.IntegerProperty );
            Assert.IsNotNull( instance.LongProperty );
            Assert.IsNotNull( instance.SingleProperty );
            Assert.IsNotNull( instance.DoubleProperty );
            Assert.IsNotNull( instance.DecimalProperty );
            Assert.IsNotNull( instance.DateTimeProperty );
            Assert.IsNotNull( instance.CharProperty );
            Assert.IsNotNull( instance.StringProperty );
            Assert.IsNotNull( instance.EnumProperty );
            Assert.IsNotNull( instance.FlagProperty );

            Assert.IsTrue( instance.BooleanProperty.Length == 3 );
            Assert.IsTrue( instance.ShortProperty.Length == 3 );
            Assert.IsTrue( instance.IntegerProperty.Length == 3 );
            Assert.IsTrue( instance.LongProperty.Length == 3 );
            Assert.IsTrue( instance.SingleProperty.Length == 3 );
            Assert.IsTrue( instance.DoubleProperty.Length == 3 );
            Assert.IsTrue( instance.DecimalProperty.Length == 3 );
            Assert.IsTrue( instance.DateTimeProperty.Length == 3 );
            Assert.IsTrue( instance.CharProperty.Length == 3 );
            Assert.IsTrue( instance.StringProperty.Length == 3 );
            Assert.IsTrue( instance.EnumProperty.Length == 3 );
            Assert.IsTrue( instance.FlagProperty.Length == 3 );

            for ( int i=0; i<3; i++ )
            {
                Assert.IsNotNull( instance.BooleanProperty[ i ] );
                Assert.IsNotNull( instance.ShortProperty[ i ] );
                Assert.IsNotNull( instance.IntegerProperty[ i ] );
                Assert.IsNotNull( instance.LongProperty[ i ] );
                Assert.IsNotNull( instance.SingleProperty[ i ] );
                Assert.IsNotNull( instance.DoubleProperty[ i ] );
                Assert.IsNotNull( instance.DecimalProperty[ i ] );
                Assert.IsNotNull( instance.DateTimeProperty[ i ] );
                Assert.IsNotNull( instance.CharProperty[ i ] );
                Assert.IsNotNull( instance.StringProperty[ i ] );
                Assert.IsNotNull( instance.EnumProperty[ i ] );
                Assert.IsNotNull( instance.FlagProperty[ i ] );
            }
        }


        [TestMethod]
        public void NestedClass()
        {
            var instance = Mocker.Mock<NestedClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.Child );
            Assert.IsNotNull( instance.Child.ChildProperty );
        }


        [TestMethod]
        public void NestedArrayClass()
        {
            var instance = Mocker.Mock<NestedArrayClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.Child );
            Assert.IsTrue( instance.Child.Length == 3 );

            for ( int i = 0; i < 3; i++ )
            {
                Assert.IsNotNull( instance.Child[ i ] );
                Assert.IsNotNull( instance.Child[ i ].ChildProperty );
            }
        }


        [TestMethod]
        public void RecursiveNestedClass()
        {
            var instance = Mocker.Mock<RecursiveNestedClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.Recursive );
            Assert.IsNotNull( instance.Recursive.Recursive );
            Assert.IsNull( instance.Recursive.Recursive.Recursive );
        }


        [TestMethod]
        public void Recursive2NestedClass()
        {
            var instance = Mocker.Mock<Recursive2NestedClass>();
            Debug.WriteLine( ObjectDumper.Dump( instance ) );

            Assert.IsNotNull( instance );
            Assert.IsNotNull( instance.One );
            Assert.IsNotNull( instance.One.Two );
            Assert.IsNotNull( instance.One.Two.One );
            Assert.IsNotNull( instance.One.Two.One.Two );
            Assert.IsNull( instance.One.Two.One.Two.One );
        }
    }
}
