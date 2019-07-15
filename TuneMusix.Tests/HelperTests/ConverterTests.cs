using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TuneMusix.Helpers;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    class ConverterTests
    {
        [TestMethod]
        public void AddTimeStrings()
        {
            string time1 = "00:15:21";
            string time2 = "04:59";
            string time3 = "01:00";
            TimeSpan realTime1 = TimeSpan.FromSeconds(921);

            TimeSpan realTime2 = TimeSpan.FromSeconds(299);

            TimeSpan realTime3 = TimeSpan.FromSeconds(60);

            string result1 = Converter.TimeSpanToString(realTime1);
            string result2 = Converter.TimeSpanToString(realTime2);
            string result3 = Converter.TimeSpanToString(realTime3);

            Assert.Equals(result1, time1);
            Assert.Equals(result2, time2);
            Assert.Equals(result3, time3);
        }
    }
}
