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
        public void PatternFail1()
        {
            var req = new RegexClass();
            req.Value = "hello";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression", vr.Errors[ 0 ].Message );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void PatternFail2()
        {
            var req = new RegexBrokenClass();
            req.Value = "hello";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression_Invalid", vr.Errors[ 0 ].Message );
        }
    }
}
