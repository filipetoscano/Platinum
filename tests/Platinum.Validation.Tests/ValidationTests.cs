using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

            Assert.IsTrue( vr.IsValid );
        }


        [TestMethod]
        public void RequiredClassWithErrors()
        {
            RequiredClass req = new RequiredClass();

            var vr = Validator.Validate( req );

            Assert.IsFalse( vr.IsValid );
            Assert.AreEqual( 4, vr.Errors.Count );
        }
    }
}
