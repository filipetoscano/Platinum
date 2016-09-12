using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class StringLengthTests
    {
        /// <summary>
        /// Min: Exact length.
        /// </summary>
        [TestMethod]
        public void MinLengthOk1()
        {
            var req = new StringMinLengthClass();
            req.Value = "12345";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Min: Allow null.
        /// </summary>
        [TestMethod]
        public void MinLengthOk2()
        {
            var req = new StringMinLengthClass();
            req.Value = null;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Min: Shorter.
        /// </summary>
        [TestMethod]
        public void MinLengthFail()
        {
            var req = new StringMinLengthClass();
            req.Value = "1234";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
        }


        /// <summary>
        /// Max: Exact length.
        /// </summary>
        [TestMethod]
        public void MaxLengthOk1()
        {
            var req = new StringMaxLengthClass();
            req.Value = "1234567890";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Max: Allow null.
        /// </summary>
        [TestMethod]
        public void MaxLengthOk2()
        {
            var req = new StringMaxLengthClass();
            req.Value = null;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Max: Longer.
        /// </summary>
        [TestMethod]
        public void MaxLengthFail()
        {
            var req = new StringMaxLengthClass();
            req.Value = "12345678901";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Max", vr.Errors[ 0 ].Message );
        }


        /// <summary>
        /// Min/Max: Exact max length.
        /// </summary>
        [TestMethod]
        public void LengthOk1()
        {
            var req = new StringLengthClass();
            req.Value = "1234567890";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Min/Max: Exact min length.
        /// </summary>
        [TestMethod]
        public void LengthOk2()
        {
            var req = new StringLengthClass();
            req.Value = "12345";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Min/Max: Allow null.
        /// </summary>
        [TestMethod]
        public void LengthOk3()
        {
            var req = new StringLengthClass();
            req.Value = null;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Min/Max: Shorter.
        /// </summary>
        [TestMethod]
        public void LengthFail1()
        {
            var req = new StringLengthClass();
            req.Value = "1234";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
        }


        /// <summary>
        /// Min/Max: Longer.
        /// </summary>
        [TestMethod]
        public void LengthFail2()
        {
            var req = new StringLengthClass();
            req.Value = "12345678901";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Max", vr.Errors[ 0 ].Message );
        }
    }
}
