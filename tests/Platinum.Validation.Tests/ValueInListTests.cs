using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class ValueInListTests
    {
        /// <summary />
        [TestMethod]
        public void StringOk()
        {
            var req = new ValueInList1();
            req.Value = "One";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void StringFail()
        {
            var req = new ValueInList1();
            req.Value = "Three";

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "ValueInList_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value", vr.Errors[ 0 ].Actor );
        }


        /// <summary />
        [TestMethod]
        public void EnumOk()
        {
            var req = new ValueInList2();
            req.Value = ValueInListEnum.One;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void EnumFail()
        {
            var req = new ValueInList2();
            req.Value = ValueInListEnum.Three;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "ValueInList_Invalid", vr.Errors[ 0 ].Message );
            Assert.AreEqual( ".Value", vr.Errors[ 0 ].Actor );
        }
    }
}
