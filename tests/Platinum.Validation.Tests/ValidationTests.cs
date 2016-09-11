using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Platinum.Validation.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void RequiredClassOk()
        {
            RequiredClass req = new RequiredClass();
            req.String = "string";
            req.Enum = TheEnum.Value2;
            req.Int = 5;
            req.DateTime = DateTime.UtcNow;

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        [TestMethod]
        public void RequiredClassWithErrors()
        {
            RequiredClass req = new RequiredClass();

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 4, vr.Errors.Count );
        }


        [TestMethod]
        public void NestedClassOk()
        {
            NestedClass req = new NestedClass();
            req.String = "level 1";
            req.Nested = new NestedClass();
            req.Nested.String = "level 2";
            req.Nested.Nested = new NestedClass();
            req.Nested.Nested.String = "level 3";

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        [TestMethod]
        public void NestedClassWithErrors()
        {
            NestedClass req = new NestedClass();
            req.Nested = new NestedClass();
            req.Nested.Nested = new NestedClass();

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 3, vr.Errors.Count );

            Assert.AreEqual( 1, vr.Errors.Where( x => x.Actor == ".String" ).Count() );
            Assert.AreEqual( 1, vr.Errors.Where( x => x.Actor == ".Nested.String" ).Count() );
            Assert.AreEqual( 1, vr.Errors.Where( x => x.Actor == ".Nested.Nested.String" ).Count() );
            Assert.AreEqual( 3, vr.Errors.Where( x => x.Message == "Required" ).Count() );
        }


        [TestMethod]
        public void ArrayClassRequired1()
        {
            ArrayClass req = new ArrayClass();

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( ".Array", vr.Errors[ 0 ].Actor );
            Assert.AreEqual( "Required", vr.Errors[ 0 ].Message );
        }


        [TestMethod]
        public void ArrayClassRequired2()
        {
            ArrayClass req = new ArrayClass();
            req.Array = new ArrayItem[] { };

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( ".Array", vr.Errors[ 0 ].Actor );
            Assert.AreEqual( "Required", vr.Errors[ 0 ].Message );
        }


        [TestMethod]
        public void ArrayClassOk()
        {
            ArrayClass req = new ArrayClass();
            req.Array = new ArrayItem[] {
                new ArrayItem() { String = "string" },
                new ArrayItem() { String = "string" },
                new ArrayItem() { String = "string" },
            };

            var vr = Validator.Validate( req );

            Assert.AreEqual( true, vr.IsValid );
        }


        [TestMethod]
        public void ArrayClassWithErrors()
        {
            ArrayClass req = new ArrayClass();
            req.Array = new ArrayItem[] {
                new ArrayItem() { String = "string" },
                new ArrayItem() {},
                new ArrayItem() { String = "string" },
            };

            var vr = Validator.Validate( req );

            Assert.AreEqual( false, vr.IsValid );
            Assert.AreEqual( 1, vr.Errors.Count );
            Assert.AreEqual( ".Array[1].String", vr.Errors[ 0 ].Actor );
            Assert.AreEqual( "Required", vr.Errors[ 0 ].Message );
        }
    }
}
