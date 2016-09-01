using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platinum.Globalization;
using System;
using System.Globalization;

namespace Platinum.Core.Tests.Globalization
{
    [TestClass]
    public class GlobalizationTest
    {
        [TestMethod]
        public void NoOverride()
        {
            CultureInfo ci = new CultureInfo( "en-GB" );
            CultureInfo cx = CultureFactory.GetCulture( "en-GB" );

            Assert.AreEqual( ci.NumberFormat.NumberDecimalSeparator, cx.NumberFormat.NumberDecimalSeparator );
            Assert.AreEqual( ci.NumberFormat.CurrencyGroupSeparator, cx.NumberFormat.CurrencyGroupSeparator );

            Assert.AreEqual( ci.DateTimeFormat.ShortDatePattern, cx.DateTimeFormat.ShortDatePattern );
            Assert.AreEqual( ci.DateTimeFormat.DateSeparator, cx.DateTimeFormat.DateSeparator );
            Assert.AreEqual( ci.DateTimeFormat.TimeSeparator, cx.DateTimeFormat.TimeSeparator );
            Assert.AreEqual( ci.DateTimeFormat.FirstDayOfWeek, cx.DateTimeFormat.FirstDayOfWeek );
        }


        [TestMethod]
        public void Override()
        {
            CultureInfo ci = new CultureInfo( "en-US" );
            CultureInfo cx = CultureFactory.GetCulture( "en-US" );

            Assert.AreEqual( ci.NumberFormat.NumberDecimalSeparator, cx.NumberFormat.NumberDecimalSeparator );
            Assert.AreEqual( ci.NumberFormat.CurrencyGroupSeparator, cx.NumberFormat.CurrencyGroupSeparator );

            Assert.AreEqual( "yyyy-MM-dd", cx.DateTimeFormat.ShortDatePattern );
            Assert.AreEqual( "/", cx.DateTimeFormat.DateSeparator );
            Assert.AreEqual( ci.DateTimeFormat.TimeSeparator, cx.DateTimeFormat.TimeSeparator );
            Assert.AreEqual( DayOfWeek.Monday, cx.DateTimeFormat.FirstDayOfWeek );
        }


        [TestMethod]
        public void FakeCulture()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            CultureInfo cx = CultureFactory.GetCulture( "ft-FT" );

            Assert.AreEqual( ci.NumberFormat.NumberDecimalSeparator, cx.NumberFormat.NumberDecimalSeparator );
            Assert.AreEqual( ci.NumberFormat.CurrencyGroupSeparator, cx.NumberFormat.CurrencyGroupSeparator );

            Assert.AreEqual( ci.DateTimeFormat.ShortDatePattern, cx.DateTimeFormat.ShortDatePattern );
            Assert.AreEqual( ci.DateTimeFormat.DateSeparator, cx.DateTimeFormat.DateSeparator );
            Assert.AreEqual( ci.DateTimeFormat.TimeSeparator, cx.DateTimeFormat.TimeSeparator );
            Assert.AreEqual( ci.DateTimeFormat.FirstDayOfWeek, cx.DateTimeFormat.FirstDayOfWeek );
        }
    }
}
