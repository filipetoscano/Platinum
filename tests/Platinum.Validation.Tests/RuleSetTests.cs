using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Platinum.Validation.Tests
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
        public void RuleSet1_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = "12345";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #1: Not Ok, because too short.
        /// </summary>
        [TestMethod]
        public void RuleSet1_NotOkMin()
        {
            var req = new RuleSetClass();
            req.Value1 = "123";
            req.Value2 = null;
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #1: Not Ok, because field is forbidden.
        /// </summary>
        [TestMethod]
        public void RuleSet1_Forbidden()
        {
            var req = new RuleSetClass();
            req.Value1 = "12345";
            req.Value2 = "11111";
            req.Value3 = null;

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Forbidden", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value2", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #1: Not Ok, because too long.
        /// </summary>
        [TestMethod]
        public void RuleSet1_NotOkMax()
        {
            var req = new RuleSetClass();
            req.Value1 = "123456789AB";

            var vr = Validator.Validate<RuleSetClass, RuleSet1>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Max", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #2: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet2_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = null;
            req.Value2 = "55555";
            req.Value3 = "OK";

            var vr = Validator.Validate<RuleSetClass, RuleSet2>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #2: Not ok, because .Value1 is forbidden.
        /// </summary>
        [TestMethod]
        public void RuleSet2_FailForbidden()
        {
            var req = new RuleSetClass();
            req.Value1 = "11111";
            req.Value2 = "55555";
            req.Value3 = "OK";

            var vr = Validator.Validate<RuleSetClass, RuleSet2>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Forbidden", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #2: Not ok, because since Value3 is OK, .Value2 must
        /// be longer.
        /// </summary>
        [TestMethod]
        public void RuleSet2_FailLength1()
        {
            var req = new RuleSetClass();
            req.Value1 = null;
            req.Value2 = "555";
            req.Value3 = "OK";

            var vr = Validator.Validate<RuleSetClass, RuleSet2>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value2", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #2: Not ok, because since Value3 is not OK, .Value2
        /// must be longer.
        /// </summary>
        [TestMethod]
        public void RuleSet2_FailLength2()
        {
            var req = new RuleSetClass();
            req.Value1 = null;
            req.Value2 = "55555";
            req.Value3 = "OTHER";

            var vr = Validator.Validate<RuleSetClass, RuleSet2>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value2", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #3: Ok 1. Length is 2, because of EnumValue1.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Ok1()
        {
            var req = new RuleSetClass();
            req.Value1 = "22";
            req.Enum1 = RuleSetEnum.EnumValue1;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #3: Fail 1. Length must be 2, because of EnumValue1.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Fail1()
        {
            var req = new RuleSetClass();
            req.Value1 = "2222";
            req.Enum1 = RuleSetEnum.EnumValue1;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Max", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #3: Ok 2. Length is 4, because of EnumValue2.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Ok2()
        {
            var req = new RuleSetClass();
            req.Value1 = "4444";
            req.Enum1 = RuleSetEnum.EnumValue2;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #3: Ok 2. Length must be 4, because of EnumValue2.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Fail2()
        {
            var req = new RuleSetClass();
            req.Value1 = "444444";
            req.Enum1 = RuleSetEnum.EnumValue2;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Max", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #3: Ok 3. Null value, because of EnumValue3.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Ok3()
        {
            var req = new RuleSetClass();
            req.Value1 = null;
            req.Enum1 = RuleSetEnum.EnumValue3;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #3: Fail 3. Forbidden value, because of EnumValue3.
        /// </summary>
        [TestMethod]
        public void RuleSet3_Fail3()
        {
            var req = new RuleSetClass();
            req.Value1 = "FF";
            req.Enum1 = RuleSetEnum.EnumValue3;

            var vr = Validator.Validate<RuleSetClass, RuleSet3>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Forbidden", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #4: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet4_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = "111";
            req.Value2 = "222";
            req.Value3 = "333";

            var vr = Validator.Validate<RuleSetClass, RuleSet4>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #4: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet4_Fail1()
        {
            var req = new RuleSetClass();
            req.Value1 = "111";
            req.Value2 = "222";
            req.Value3 = "3";

            var vr = Validator.Validate<RuleSetClass, RuleSet4>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value3", vr.Errors[ 0 ].Actor );
        }


        /// <summary>
        /// RuleSet #5: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet5_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = "111";
            req.Value2 = "XX222XX";

            var vr = Validator.Validate<RuleSetClass, RuleSet5>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 2, vr.Errors.Count );
            Assert.AreEqual( "RegularExpression_Strict_NotStart", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1.Development", vr.Errors[ 0 ].Actor );
            Assert.AreEqual( "RegularExpression_Strict_NotEnd", vr.Errors[ 1 ].Message );
            Assert.AreEqual( ".Value1.Development", vr.Errors[ 1 ].Actor );
        }


        /// <summary>
        /// RuleSet #6: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet6_Ok()
        {
            var req = new RuleSetClass();
            req.Value1 = "One";
            req.Value2 = "TWO";

            var vr = Validator.Validate<RuleSetClass, RuleSet6>( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary>
        /// RuleSet #6: Ok.
        /// </summary>
        [TestMethod]
        public void RuleSet6_Fail()
        {
            var req = new RuleSetClass();
            req.Value1 = "ONE";
            req.Value2 = "2";

            var vr = Validator.Validate<RuleSetClass, RuleSet6>( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 2, vr.Errors.Count );
            Assert.AreEqual( "ValueInList_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value1", vr.Errors[ 0 ].Actor );
            Assert.AreEqual( "ValueInList_Invalid", vr.Errors[ 1 ].Message );
            Assert.AreEqual( ".Value2", vr.Errors[ 1 ].Actor );
        }
    }
}
