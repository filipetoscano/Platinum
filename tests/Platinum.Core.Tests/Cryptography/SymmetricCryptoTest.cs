using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Cryptography;

namespace Platinum.Core.Tests.Cryptography
{
    [TestClass]
    public class SymmetricCryptoTest
    {
        [TestMethod]
        public void AesManaged()
        {
            string value = "sssssh! secret!";
            string secret;
            string noMore;

            SymmetricCrypto c = new SymmetricCrypto( "AesManaged" );
            secret = c.Encrypt( value );
            noMore = c.Decrypt( secret );

            Assert.AreEqual( value, noMore );
        }


        [TestMethod]
        public void Aes()
        {
            string value = "sssssh! secret!";
            string secret;
            string noMore;

            SymmetricCrypto c = new SymmetricCrypto( "Aes" );
            secret = c.Encrypt( value );
            noMore = c.Decrypt( secret );

            Assert.AreEqual( value, noMore );
        }
    }
}
