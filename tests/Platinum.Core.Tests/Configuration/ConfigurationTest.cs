using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Cryptography;

namespace Platinum.Core.Tests.Configuration
{
    [TestClass]
    public class ConfigurationTest
    {
        /// <summary>
        /// Section
        /// </summary>
        [TestMethod]
        public void TopProperty()
        {
            var config = ConfigurationTestConfiguration.Current;

            Assert.IsNotNull( config );
            Assert.AreEqual( true, config.Bool );
            Assert.AreEqual( TheEnum.Value1, config.Enum );
            Assert.AreEqual( 1, config.Int );
            Assert.AreEqual( "pass the test, Luke", config.String );

            Assert.IsNotNull( config.SingleChild );
            Assert.AreEqual( "these aren't the droids you are looking for", config.SingleChild.Property );

            Assert.IsNotNull( config.MultipleChildren );
            Assert.AreEqual( 2, config.MultipleChildren.Count );

            Assert.IsNotNull( config.MultipleChildren[ 0 ] );
            Assert.AreEqual( 2, config.MultipleChildren[ 0 ].Settings.Count );
            Assert.AreEqual( "k1", config.MultipleChildren[ 0 ].Settings[ 0 ].Key );
            Assert.AreEqual( "v1", config.MultipleChildren[ 0 ].Settings[ 0 ].Value );

            Assert.IsNotNull( config.MultipleChildren[ 1 ] );
            Assert.AreEqual( 0, config.MultipleChildren[ 1 ].Settings.Count );
        }
    }
}
