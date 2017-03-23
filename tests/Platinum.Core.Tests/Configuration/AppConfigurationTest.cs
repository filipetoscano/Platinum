using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Configuration;
using System;

namespace Platinum.Core.Tests.Configuration
{
    [TestClass]
    public class AppConfigurationTest
    {
        /// <summary>
        /// Duration is valid.
        /// </summary>
        [TestMethod]
        public void Duration_Ok1()
        {
            var d = AppConfiguration.Get<Duration>( "Duration.Ok1" );

            Assert.AreEqual( 1, d.Hours );
            Assert.AreEqual( 2, d.Minutes );
            Assert.AreEqual( 3, d.Seconds );
        }


        /// <summary>
        /// Duration is invalid.
        /// </summary>
        [TestMethod]
        public void Duration_NotOk()
        {
            try
            {
                AppConfiguration.Get<Duration>( "Duration.Nok" );

                Assert.Fail( "Expected exception" );
            }
            catch ( ConfigurationException ex )
            {
                if ( ex.Message != "Get_NotDuration" )
                    Assert.Fail( "Wrong exception thrown: {0}", ex.ToString() );
            }
        }


        /// <summary>
        /// TimeSpan is valid.
        /// </summary>
        [TestMethod]
        public void TimeSpan_Ok1()
        {
            var d = AppConfiguration.Get<TimeSpan>( "TimeSpan.Ok1" );

            Assert.AreEqual( 0, d.Days );
            Assert.AreEqual( 1, d.Hours );
            Assert.AreEqual( 2, d.Minutes );
            Assert.AreEqual( 3, d.Seconds );
        }


        /// <summary>
        /// TimeSpan is valid.
        /// </summary>
        [TestMethod]
        public void TimeSpan_Ok2()
        {
            var d = AppConfiguration.Get<TimeSpan>( "TimeSpan.Ok2" );

            Assert.AreEqual( 1, d.Days );
            Assert.AreEqual( 0, d.Hours);
            Assert.AreEqual( 0, d.Minutes );
            Assert.AreEqual( 0, d.Seconds );
        }


        /// <summary>
        /// TimeSpan is valid.
        /// </summary>
        [TestMethod]
        public void TimeSpan_Ok3()
        {
            var d = AppConfiguration.Get<TimeSpan>( "TimeSpan.Ok3" );

            Assert.AreEqual( 1, d.Days );
            Assert.AreEqual( 2, d.Hours );
            Assert.AreEqual( 3, d.Minutes );
            Assert.AreEqual( 4, d.Seconds );
        }


        /// <summary>
        /// TimeSpan is invalid.
        /// </summary>
        [TestMethod]
        public void TimeSpan_NotOk()
        {
            try
            {
                var d = AppConfiguration.Get<TimeSpan>( "TimeSpan.Nok" );

                Assert.Fail( "Expected exception" );
            }
            catch ( ConfigurationException ex )
            {
                if ( ex.Message != "Get_NotTimeSpan" )
                    Assert.Fail( "Wrong exception thrown: {0}", ex.ToString() );
            }
        }
    }
}
