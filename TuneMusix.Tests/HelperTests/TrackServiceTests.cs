using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    public class TrackServiceTests
    {
        private Track track1 = new Track( 0, 123, null, "aaaaaa", null, null, null, null, null );
        private Track track2 = new Track( 0, 234, null, "bbbbbb", null, null, null, null, null );
        private Track track3 = new Track( 0, 345, null, "cccccc", null, null, null, null, null );
        private Track track4 = new Track( 0, 567, null, "dddddd", null, null, null, null, null );

        private void setTracks()
        {
            track1.Album.Value = "peter";
            track1.Interpret.Value = "thomas";
            track1.Title.Value = "horst";

            track2.Album.Value = "hans";
            track2.Interpret.Value = "willi";
            track2.Title.Value = "dieter";

            track3.Album.Value = "georg";
            track3.Interpret.Value = "franz";
            track3.Title.Value = "richard";

            track4.Album.Value = "fabian";
            track4.Interpret.Value = "anna";
            track4.Title.Value = "ivone";
        }

        [TestMethod]
        public void Contains()
        {
            setTracks();

            //test of album search
            Assert.IsTrue( TrackService.Contains( track1, "thomas", false ) );
            Assert.IsTrue( TrackService.Contains( track1, "mas", false ) );
            Assert.IsFalse( TrackService.Contains( track2, "udo", false ) );
            Assert.IsFalse( TrackService.Contains( track2, "do", false ) );


            //test of title seach
            Assert.IsTrue( TrackService.Contains( track3, "richard", false ) );
            Assert.IsTrue( TrackService.Contains( track4, "one", false ) );
            Assert.IsFalse( TrackService.Contains( track3, "peter", false ) );
            Assert.IsFalse( TrackService.Contains( track4, "ter", false ) );

            //test of interpret search
            Assert.IsTrue( TrackService.Contains( track1, "thomas", false ) );
            Assert.IsTrue( TrackService.Contains( track3, "anz", false ) );
            Assert.IsFalse( TrackService.Contains( track1, "thomasioni", false ) );
            Assert.IsFalse( TrackService.Contains( track3, "chart", false ) );

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

            Assert.AreEqual( "11:25:27", TrackService.AddDurations( duration1, duration2 ) );
            Assert.AreEqual( "11:24:44", TrackService.AddDurations( duration1, duration6 ) );
            Assert.AreEqual( "00:13:17", TrackService.AddDurations( duration3, duration4 ) );
            Assert.AreEqual( "00:00:00", TrackService.AddDurations( duration6, duration6 ) );
            Assert.AreEqual( "00:52:44", TrackService.AddDurations( duration5, duration3 ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void NullArgumentAddDuration()
        {
            TrackService.AddDurations( null, null );
        }

        [TestMethod]
        [ExpectedException( typeof( FormatException ) )]
        public void InvalidArgumentAddDuration()
        {
            TrackService.AddDurations( "ab:as:dd", "dd" );
        }
    }
}
