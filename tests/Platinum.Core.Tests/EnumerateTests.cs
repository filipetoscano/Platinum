using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Platinum.Core.Tests
{
    [TestClass]
    public class EnumerateTests
    {
        [TestMethod]
        public void ParseEnum()
        {
            var v = Enumerate.Parse<TestEnum>( "Value1" );

            Assert.AreEqual( TestEnum.Value1, v );
        }


        [TestMethod]
        public void ParseEnumNull()
        {
            try
            {
                Enumerate.Parse<TestEnum>( null );

                Assert.Fail( "Expected ArgumentNullException" );
            }
            catch ( ArgumentNullException )
            {
            }
        }


        [TestMethod]
        public void ParseNullable()
        {
            var v = Enumerate.Parse<TestEnum?>( "Value1" );

            Assert.AreEqual( TestEnum.Value1, v );
        }


        [TestMethod]
        public void ParseNullableNull()
        {
            var v = Enumerate.Parse<TestEnum?>( null );

            Assert.AreEqual( null, v );
        }


        [TestMethod]
        public void ParseFlag()
        {
            var v = Enumerate.Parse<TestFlagsEnum>( "Value1,Value2,Value4" );

            Assert.IsTrue( v.HasFlag( TestFlagsEnum.Value1 ) );
            Assert.IsTrue( v.HasFlag( TestFlagsEnum.Value2 ) );
            Assert.IsTrue( v.HasFlag( TestFlagsEnum.Value4 ) );
        }
    }
}
