using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.IO;

namespace Platinum.Core.Tests.IO
{
    [TestClass]
    public class FsPathTest
    {
        [TestMethod]
        public void Test01()
        {
            string x = FsPath.Canonicalize( "C:/foo" );

            Assert.AreEqual( @"C:\foo", x );
        }


        [TestMethod]
        public void Test02()
        {
            string x = FsPath.Canonicalize( "C:/foo/bar" );

            Assert.AreEqual( @"C:\foo\bar", x );
        }


        [TestMethod]
        public void Test03()
        {
            string x = FsPath.Canonicalize( "C:/foo/../bar" );

            Assert.AreEqual( @"C:\bar", x );
        }


        [TestMethod]
        public void Test04()
        {
            string x = FsPath.Canonicalize( "C:/foo/./bar" );

            Assert.AreEqual( @"C:\foo\bar", x );
        }


        [TestMethod]
        public void Test05()
        {
            string x = FsPath.Canonicalize( "C:/foo/bar/.." );

            Assert.AreEqual( @"C:\foo", x );
        }


        [TestMethod]
        public void Test06()
        {
            string x = FsPath.Canonicalize( @"C:\foo" );

            Assert.AreEqual( @"C:\foo", x );
        }


        [TestMethod]
        public void Test07()
        {
            string x = FsPath.Canonicalize( @"C:\foo\bar" );

            Assert.AreEqual( @"C:\foo\bar", x );
        }


        [TestMethod]
        public void Test08()
        {
            string x = FsPath.Canonicalize( @"C:\foo\..\bar" );

            Assert.AreEqual( @"C:\bar", x );
        }


        [TestMethod]
        public void Test09()
        {
            string x = FsPath.Canonicalize( @"C:\foo\.\bar" );

            Assert.AreEqual( @"C:\foo\bar", x );
        }


        [TestMethod]
        public void Test10()
        {
            string x = FsPath.Canonicalize( @"C:\foo\bar\.." );

            Assert.AreEqual( @"C:\foo", x );
        }


        [TestMethod]
        public void Test11()
        {
            string x = FsPath.Canonicalize( @"C:\..\foo\bar\.." );

            Assert.AreEqual( @"C:\foo", x );
        }


        [TestMethod]
        public void TestAll()
        {
            string x = FsPath.Canonicalize( @"C:/foo\./bar\../foo" );

            Assert.AreEqual( @"C:\foo\foo", x );
        }
    }
}
