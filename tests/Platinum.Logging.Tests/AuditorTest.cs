using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Platinum.Logging.Tests
{
    [TestClass]
    public class AuditorTest
    {
        /// <summary>
        /// Publish event with no arguments.
        /// </summary>
        [TestMethod]
        public void Event_NoArguments()
        {
            Audit.Event( EV.NoArguments );
        }


        /// <summary>
        /// Publish an event with a single argument. Must show up as exd_
        /// in ElasticSearch.
        /// </summary>
        [TestMethod]
        public void Event_OneArgument()
        {
            Audit.Event( EV.OneArgument, 1 );
        }


        /// <summary>
        /// Publish an event with TWO arguments. Must show up as two exd_
        /// entries in ElasticSearch.
        /// </summary>
        [TestMethod]
        public void Event_TwoArguments()
        {
            Audit.Event( EV.TwoArguments, 2, "string" );
        }


        /// <summary>
        /// Publish an event with TWO arguments. Must show up as two exd_
        /// entries in ElasticSearch.
        /// </summary>
        [TestMethod]
        public void Event_Exception()
        {
            var ex = new NotSupportedException( "Sample exception message." );

            Audit.Event( EV.Exception, ex );
        }


        /// <summary>
        /// Publish an event with TWO arguments. Must show up as two exd_
        /// entries in ElasticSearch.
        /// </summary>
        [TestMethod]
        public void Event_ExceptionAndArguments()
        {
            var ex = new NotSupportedException( "Another exception message." );

            Audit.Event( EV.ExceptionAndArguments, ex, 3, "Xpto" );
        }


        /// <summary>
        /// Publish an event with the log level raised to Fatal.
        /// </summary>
        [TestMethod]
        public void Event_Fatal()
        {
            Audit.Event( EV.Fatal );
        }
    }
}
