using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class RuleTests
    {
        /// <summary />
        [TestMethod]
        public void Ok1()
        {
            var req = new ValidateClass1();
            req.Name = "filipe";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void Fail1()
        {
            var req = new ValidateClass1();
            req.Name = null;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "Required", vr.Errors[ 0 ].Message );
        }


        /// <summary />
        [TestMethod]
        public void Fail2()
        {
            var req = new ValidateClass1();
            req.Name = "luis";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "StringLength_Min", vr.Errors[ 0 ].Message );
        }
    }
}
