using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Javascript.Tests
{
    /// <summary />
    [TestClass]
    public class RuleSetTests
    {
        /// <summary>
        /// Enum used to test conditional tests on enums.
        /// </summary>
        public enum RuleSetEnum
        {
            EnumValue1,
            EnumValue2,
            EnumValue3,
        }


        /// <summary>
        /// Class used to test validation rule set.
        /// </summary>
        public class RuleSetClass
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Value3 { get; set; }
            public RuleSetEnum Enum1 { get; set; }
        }


        /// <summary>
        /// RuleSet #1: Ok.
        /// </summary>
        [TestMethod]
        public void Function_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = "12345";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #1: Value isn't ok, because string is too small.
        /// </summary>
        [TestMethod]
        public void Function_Invalid1()
        {
            var req = new RuleSetClass();
            req.Value1 = "123";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Function_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #1: Value isn't ok, because string is too small.
        /// </summary>
        [TestMethod]
        public void Function_Invalid2()
        {
            var req = new RuleSetClass();
            req.Value1 = "32123";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Function_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #2: Function does not compile.
        /// </summary>
        [TestMethod]
        public void Function_Evaluate()
        {
            var req = new RuleSetClass();
            req.Value1 = "12345";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet2>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Function_Evaluate", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1.Development", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #3: File was not found.
        /// </summary>
        [TestMethod]
        public void Function_NotFound()
        {
            var req = new RuleSetClass();
            req.Value1 = "12345";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Function_NotFound", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1.Development", vr.Errors[ 0 ].Actor );
        }
    }
}
