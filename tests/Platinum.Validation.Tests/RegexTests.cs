using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class RegexTests
    {
        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternOk1()
        {
            var req = new RegexClass();
            req.Value = "12345";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternOk2()
        {
            var req = new RegexClass();
            req.Enum = RegexEnum.One;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternFail1()
        {
            var req = new RegexClass();
            req.Value = "hello";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternFail2()
        {
            var req = new RegexClass();
            req.Enum = RegexEnum.Five;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Enum", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternFail3()
        {
            var req = new RegexBrokenClass();
            req.Value = "hello";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value.Development", vr.Errors[ 0 ].Actor );
        }
    }
}
