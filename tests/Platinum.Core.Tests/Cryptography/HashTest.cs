using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Cryptography;

namespace Platinum.Core.Tests.Cryptography
{
    [TestClass]
    public class HashTest
    {
        /// <summary>
        /// As per RFC1321
        /// </summary>
        [TestMethod]
        public void MD5()
        {
            Assert.AreEqual( "d41d8cd98f00b204e9800998ecf8427e", Hash.MD5( "" ) );
            Assert.AreEqual( "0cc175b9c0f1b6a831c399e269772661", Hash.MD5( "a" ) );
            Assert.AreEqual( "900150983cd24fb0d6963f7d28e17f72", Hash.MD5( "abc" ) );
            Assert.AreEqual( "f96b697d7cb7938d525a2f31aaf161d0", Hash.MD5( "message digest" ) );
            Assert.AreEqual( "c3fcd3d76192e4007dfb496cca67e13b", Hash.MD5( "abcdefghijklmnopqrstuvwxyz" ) );
            Assert.AreEqual( "d174ab98d277d9f5a5611c2c9f419d9f", Hash.MD5( "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" ) );
            Assert.AreEqual( "57edf4a22be3c955ac49da2e2107b67a", Hash.MD5( "12345678901234567890123456789012345678901234567890123456789012345678901234567890" ) );
        }


        /// <summary>
        /// With http://www.xorbin.com/tools/sha256-hash-calculator
        /// </summary>
        [TestMethod]
        public void SHA256()
        {
            Assert.AreEqual( "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", Hash.SHA256( "" ) );
            Assert.AreEqual( "ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb", Hash.SHA256( "a" ) );
            Assert.AreEqual( "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", Hash.SHA256( "abc" ) );
            Assert.AreEqual( "f7846f55cf23e14eebeab5b4e1550cad5b509e3348fbc4efa3a1413d393cb650", Hash.SHA256( "message digest" ) );
            Assert.AreEqual( "71c480df93d6ae2f1efad1447c66c9525e316218cf51fc8d9ed832f2daf18b73", Hash.SHA256( "abcdefghijklmnopqrstuvwxyz" ) );
        }


        /// <summary>
        /// With http://www.miniwebtool.com/sha512-hash-generator/
        /// </summary>
        [TestMethod]
        public void SHA512()
        {
            Assert.AreEqual( "1f40fc92da241694750979ee6cf582f2d5d7d28e18335de05abc54d0560e0f5302860c652bf08d560252aa5e74210546f369fbbbce8c12cfc7957b2652fe9a75", Hash.SHA512( "a" ) );
            Assert.AreEqual( "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f", Hash.SHA512( "abc" ) );
            Assert.AreEqual( "107dbf389d9e9f71a3a95f6c055b9251bc5268c2be16d6c13492ea45b0199f3309e16455ab1e96118e8a905d5597b72038ddb372a89826046de66687bb420e7c", Hash.SHA512( "message digest" ) );
            Assert.AreEqual( "4dbff86cc2ca1bae1e16468a05cb9881c97f1753bce3619034898faa1aabe429955a1bf8ec483d7421fe3c1646613a59ed5441fb0f321389f77f48a879c7b1f1", Hash.SHA512( "abcdefghijklmnopqrstuvwxyz" ) );
            Assert.AreEqual( "1e07be23c26a86ea37ea810c8ec7809352515a970e9253c26f536cfc7a9996c45c8370583e0a78fa4a90041d71a4ceab7423f19c71b9d5a3e01249f0bebd5894", Hash.SHA512( "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" ) );
            Assert.AreEqual( "72ec1ef1124a45b047e8b7c75a932195135bb61de24ec0d1914042246e0aec3a2354e093d76f3048b456764346900cb130d2a4fd5dd16abb5e30bcb850dee843", Hash.SHA512( "12345678901234567890123456789012345678901234567890123456789012345678901234567890" ) );
        }
    }
}
