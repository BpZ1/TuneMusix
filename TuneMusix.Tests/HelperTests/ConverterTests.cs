using NUnit.Framework;
using System;
using TuneMusix.Helpers;

namespace TuneMusix.Tests.HelperTests
{
    [TestFixture]
    class ConverterTests
    {
        [Test]
        public void AddTimeStrings()
        {
            string time1 = "15:21";
            string time2 = "4:59";
            string time3 = "1:00";
            TimeSpan realTime1 = TimeSpan.FromSeconds( 921 );

            TimeSpan realTime2 = TimeSpan.FromSeconds( 299 );

            TimeSpan realTime3 = TimeSpan.FromSeconds( 60 );

            string result1 = Converter.TimeSpanToString( realTime1 );
            string result2 = Converter.TimeSpanToString( realTime2 );
            string result3 = Converter.TimeSpanToString( realTime3 );

            Assert.AreEqual( time1, result1 );
            Assert.AreEqual( time2, result2 );
            Assert.AreEqual( time3, result3 );
        }
    }
}
