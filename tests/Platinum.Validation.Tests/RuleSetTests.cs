using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    /// <summary />
    [TestClass]
    public class RuleSetTests
    {
        /// <summary>
        /// Class used to test validation rule set.
        /// </summary>
        public class RuleSetClass
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Value3 { get; set; }
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
        }





        /// <summary>
        /// RuleSet #1: Not Ok, because too short.
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
        }
    }
}
