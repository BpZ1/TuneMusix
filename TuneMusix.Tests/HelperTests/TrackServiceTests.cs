using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    public class TrackServiceTests
    {
        private Track track1 = new Track("aaaaaa", 123);
        private Track track2 = new Track("bbbbbb", 456);
        private Track track3 = new Track("cccccc", 567);
        private Track track4 = new Track("dddddd", 678);

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
        public void ContainsTest()
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
    }
}
