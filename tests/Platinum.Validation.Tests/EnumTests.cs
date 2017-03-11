using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class EnumTests
    {
        public enum EnumType
        {
            First,
            Second,
            Third,
        }

        public class EnumClass
        {
            public EnumType Value { get; set; }
        }

        public class NullableEnumClass
        {
            public EnumType? Value { get; set; }
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void EnumIsDefined()
        {
            var req = new EnumClass();
            req.Value = EnumType.First;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Correctly identifies an error.
        /// </summary>
        [TestMethod]
        public void EnumIsNotDefined()
        {
            var req = new EnumClass();
            req.Value = (EnumType) 50;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void NullableEnumIsDefined()
        {
            var req = new NullableEnumClass();
            req.Value = EnumType.First;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Matches.
        /// </summary>
        [TestMethod]
        public void NullableEnumIsNull()
        {
            var req = new NullableEnumClass();
            req.Value = null;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// Correctly identifies an error.
        /// </summary>
        [TestMethod]
        public void NullableEnumIsNotDefined()
        {
            var req = new NullableEnumClass();
            req.Value = (EnumType) 50;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
        }
    }
}
