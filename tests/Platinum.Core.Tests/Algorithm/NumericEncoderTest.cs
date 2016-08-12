using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Algorithm;
using System;

namespace Platinum.Core.Tests.Algorithm
{
    [TestClass]
    public class NumericEncoderTest
    {
        [TestMethod]
        public void AlphaCode()
        {
            NumericEncoder ne = NumericEncoder.Build( "Alpha" );

            int nr = 100;
            var code = ne.ToCode( 100 );
            var nn = ne.ToNumber( code );

            Assert.AreEqual( ne.EncodedLength, code.Length );
            Assert.AreEqual( "0002V", code );
            Assert.AreEqual( nr, nn );
        }


        [TestMethod]
        public void SymbolCode()
        {
            NumericEncoder ne = NumericEncoder.Build( "Symbol" );

            int nr = Int32.MaxValue;
            var code = ne.ToCode( nr );
            var nn = ne.ToNumber( code );

            Assert.AreEqual( ne.EncodedLength, code.Length );
            Assert.AreEqual( "001JUz^v", code );
            Assert.AreEqual( nr, nn );
        }
    }
}
