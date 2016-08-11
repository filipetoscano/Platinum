using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Platinum.Resolver;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Collections.Generic;

namespace Platinum.Core.Tests
{
    [TestClass]
    public class ResolverTest
    {
        [TestMethod]
        public void SchemeRelfile()
        {
            UrlResolver resolver = new UrlResolver();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            XmlDocument doc = new XmlDocument();

            using ( XmlReader xr = XmlReader.Create( "relfile:///Resolver/hello.xml", xrs ) )
            {
                doc.Load( xr );
            }

            Assert.IsNotNull( doc );
            Assert.IsNotNull( doc.DocumentElement );
            Assert.IsNotNull( doc.DocumentElement.LocalName );
            Assert.AreEqual( "hi", doc.DocumentElement.LocalName );
        }


        [TestMethod]
        public void SchemeAssembly1()
        {
            UrlResolver resolver = new UrlResolver();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            XmlDocument doc = new XmlDocument();

            using ( XmlReader xr = XmlReader.Create( "assembly:///Platinum.Core.Tests/~/Resolver/hello.xml", xrs ) )
            {
                doc.Load( xr );
            }

            Assert.IsNotNull( doc );
            Assert.IsNotNull( doc.DocumentElement );
            Assert.IsNotNull( doc.DocumentElement.LocalName );
            Assert.AreEqual( "hi", doc.DocumentElement.LocalName );
        }


        [TestMethod]
        public void SchemeAssembly2()
        {
            UrlResolver resolver = new UrlResolver();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            XmlDocument doc = new XmlDocument();

            using ( XmlReader xr = XmlReader.Create( "assembly:///Platinum.Core.Tests/Platinum.Core.Tests/Resolver/hello.xml", xrs ) )
            {
                doc.Load( xr );
            }

            Assert.IsNotNull( doc );
            Assert.IsNotNull( doc.DocumentElement );
            Assert.IsNotNull( doc.DocumentElement.LocalName );
            Assert.AreEqual( "hi", doc.DocumentElement.LocalName );
        }


        [TestMethod]
        public void CustomSchemeToFile()
        {
            UrlResolver resolver = new UrlResolver( true );

            XslCompiledTransform xslt = new XslCompiledTransform();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            using ( XmlReader xr = XmlReader.Create( "c1:///hello.xslt", xrs ) )
            {
                xslt.Load( xr, null, resolver );
            }

            List<string> files = resolver.GetFileUri();

            Assert.IsNotNull( files );
            Assert.AreEqual( 2, files.Count );
        }


        [TestMethod]
        public void CustomSchemeToAssembly()
        {
            UrlResolver resolver = new UrlResolver( true );

            XslCompiledTransform xslt = new XslCompiledTransform();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            using ( XmlReader xr = XmlReader.Create( "c2:///hello.xslt", xrs ) )
            {
                xslt.Load( xr, null, resolver );
            }

            List<string> files = resolver.GetFileUri();
            Assert.IsNotNull( files );
            Assert.AreEqual( 0, files.Count );
        }
    }
}
