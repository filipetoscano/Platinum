using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Platinum.Mock.Tests
{
    public abstract class TestBase
    {
        protected void Assert_AreEqual( int yy, int MM, int dd, int HH, int mm, int ss, DateTime dateTime )
        {
            Assert.AreEqual( yy, dateTime.Year );
            Assert.AreEqual( MM, dateTime.Month );
            Assert.AreEqual( dd, dateTime.Day );
            Assert.AreEqual( HH, dateTime.Hour );
            Assert.AreEqual( mm, dateTime.Minute );
            Assert.AreEqual( ss, dateTime.Second );
            Assert.AreEqual( DateTimeKind.Utc, dateTime.Kind );
        }
    }
}
