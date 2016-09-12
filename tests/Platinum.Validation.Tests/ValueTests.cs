using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class ValueTests
    {
        /// <summary />
        [TestMethod]
        public void MinValueOk1()
        {
            var req = new MinValueClass1();
            req.Value = 5;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void MinValueOk2()
        {
            var req = new MinValueClass1();
            req.Value = 6;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void MinValueOk3()
        {
            var req = new MinValueClass3();

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void MinValueOk4()
        {
            var req = new MinValueClass3();
            req.Value = 5;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        /// <summary />
        [TestMethod]
        public void MinValueFail1()
        {
            var req = new MinValueClass1();
            req.Value = 4;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "MinValue_LessThan", vr.Errors[ 0 ].Message );
        }


        /// <summary />
        [TestMethod]
        public void MinValueFail2()
        {
            var req = new MinValueClass2();
            req.Value = 5;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "MinValue_EqualTo", vr.Errors[ 0 ].Message );
        }


        /// <summary />
        [TestMethod]
        public void MinValueFail3()
        {
            var req = new MinValueClass3();
            req.Value = 4;

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( "MinValue_LessThan", vr.Errors[ 0 ].Message );
        }
    }
}
