using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    public class TrackServiceTests
    {
        private Track track1 = new Track("aaaaaa", 123, null, null, null, 0, null, null, null);
        private Track track2 = new Track("bbbbbb", 456, null, null, null, 0, null, null, null);
        private Track track3 = new Track("cccccc", 567, null, null, null, 0, null, null, null);
        private Track track4 = new Track("dddddd", 678, null, null, null, 0, null, null, null);

        private void setTracks()
        {
            track1.Album = "peter";
            track1.Interpret = "thomas";
            track1.Title = "horst";

            track2.Album = "hans";
            track2.Interpret = "willi";
            track2.Title = "dieter";

            track3.Album = "georg";
            track3.Interpret = "franz";
            track3.Title = "richard";

            track4.Album = "fabian";
            track4.Interpret = "anna";
            track4.Title = "ivone";
        }

        [TestMethod]
        public void Contains()
        {
            setTracks();

            //test of album search
            Assert.IsTrue(TrackService.Contains(track1, "thomas", false));
            Assert.IsTrue(TrackService.Contains(track1, "mas", false));
            Assert.IsFalse(TrackService.Contains(track2, "udo", false));
            Assert.IsFalse(TrackService.Contains(track2, "do", false));


            //test of title seach
            Assert.IsTrue(TrackService.Contains(track3, "richard", false));
            Assert.IsTrue(TrackService.Contains(track4, "one", false));
            Assert.IsFalse(TrackService.Contains(track3, "peter", false));
            Assert.IsFalse(TrackService.Contains(track4, "ter", false));

            //test of interpret search
            Assert.IsTrue(TrackService.Contains(track1, "thomas", false));
            Assert.IsTrue(TrackService.Contains(track3, "anz", false));
            Assert.IsFalse(TrackService.Contains(track1, "thomasioni", false));
            Assert.IsFalse(TrackService.Contains(track3, "chart", false));

        }

        [TestMethod]
        public void AddDurationValid()
        {
            string duration1 = "11:24:44";
            string duration2 = "43";
            string duration3 = "00:44";
            string duration4 = "12:33";
            string duration5 = "52:00";
            string duration6 = "";

            Assert.AreEqual("11:25:27", TrackService.AddDurations(duration1, duration2));
            Assert.AreEqual("11:24:44", TrackService.AddDurations(duration1, duration6));
            Assert.AreEqual("00:13:17", TrackService.AddDurations(duration3, duration4));
            Assert.AreEqual("00:00:00", TrackService.AddDurations(duration6, duration6));
            Assert.AreEqual("00:52:44", TrackService.AddDurations(duration5, duration3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgumentAddDuration()
        {
            TrackService.AddDurations(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void InvalidArgumentAddDuration()
        {
            TrackService.AddDurations("ab:as:dd", "dd");
        }
    }
}
