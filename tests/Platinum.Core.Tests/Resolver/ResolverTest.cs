using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Resolver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;

namespace Platinum.Core.Tests.Resolver
{
    [TestClass]
    public class ResolverTest
    {
        [TestMethod]
        public void SchemeRelfile()
        {
            /*
             * We won't bother supporting running MSTest, since that would
             * require a change to RelfileResolver.
             */
            if ( Assembly.GetExecutingAssembly().Location.Contains( @"\TestResults\" ) == true )
                return;


            /*
             * 
             */
            string p = "";

            if ( AppDomain.CurrentDomain.FriendlyName.StartsWith( "UnitTestAdapter" ) == true )
                p = "../../";


            /*
             * 
             */
            UrlResolver resolver = new UrlResolver();

            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.XmlResolver = resolver;

            XmlDocument doc = new XmlDocument();

            using ( XmlReader xr = XmlReader.Create( $"relfile:///{ p }Resolver/hello.xml", xrs ) )
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
