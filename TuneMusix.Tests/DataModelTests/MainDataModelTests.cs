using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Model;

namespace TuneMusix.Tests.DataModelTests
{
    [TestClass]
    public class MainDataModelTests
    {
        DataModel dataModel = DataModel.Instance;

        //MockObjects
        private Playlist playlist1 = new Playlist("1",1);
        private Playlist playlist2 = new Playlist("2", 1);
        private Playlist playlist3 = new Playlist("3", 1);
        private Playlist playlist4 = new Playlist("4", 1);
        private Playlist playlist5 = new Playlist("5", 1);

        private Track track1 = new Track("url1", 1, null, null, null, 0, null, null, null);
        private Track track2 = new Track("url2", 1, null, null, null, 0, null, null, null);
        private Track track3 = new Track("url3", 1, null, null, null, 0, null, null, null);
        private Track track4 = new Track("url4", 1, null, null, null, 0, null, null, null);
        private Track track5 = new Track("url4", 1, null, null, null, 0, null, null, null);

        
        /*
        [TestMethod]
        public void DeleteTrackTest()
        {
            dataModel.TrackList.Add(track1);
            dataModel.TrackList.Add(track2);
            dataModel.TrackList.Add(track3);
            dataModel.TrackList.Add(track4);

            Assert.AreEqual(4,dataModel.TrackList.Count);
            List<Track> tracklist = new List<Track>();
            tracklist.Add(track1);
            tracklist.Add(track2);
            dataModel.Delete(tracklist);
            Assert.AreEqual(2, dataModel.TrackList.Count);
        }
        */
        [TestMethod]
        public void SetCurrentPlaylist()
        {
            Assert.IsNull(dataModel.CurrentPlaylist);
            dataModel.CurrentPlaylist = playlist1;
            Assert.IsNotNull(dataModel.CurrentPlaylist);
            Assert.AreEqual(dataModel.CurrentPlaylist,playlist1);
        }

        [TestMethod]
        public void SetCurrentTrack()
        {
            Assert.IsNull(dataModel.CurrentTrack);
            dataModel.CurrentTrack = track1;
            Assert.IsNotNull(dataModel.CurrentTrack);
            Assert.AreEqual(dataModel.CurrentTrack, track1);
        }

        [TestMethod]
        public void ShuffleTest()
        {
            dataModel.TrackQueue = new ObservableCollection<Track>() { track1, track2, track3, track4, track5 };

            dataModel.ShuffleTrackQueue();

            bool orderChanged = false;

            if(!(dataModel.TrackQueue.IndexOf(track1) == 0)||
                !(dataModel.TrackQueue.IndexOf(track2) == 1)||
                !(dataModel.TrackQueue.IndexOf(track3) == 2) ||
                !(dataModel.TrackQueue.IndexOf(track4) == 3) ||
                !(dataModel.TrackQueue.IndexOf(track5) == 4))
            {
                orderChanged = true;
            }

            Assert.IsTrue(orderChanged);

            dataModel.UnShuffleTrackQueue();

            Assert.IsTrue(dataModel.TrackQueue.IndexOf(track1)==0);
            Assert.IsTrue(dataModel.TrackQueue.IndexOf(track2) == 1);
            Assert.IsTrue(dataModel.TrackQueue.IndexOf(track3) == 2);
            Assert.IsTrue(dataModel.TrackQueue.IndexOf(track4) == 3);
            Assert.IsTrue(dataModel.TrackQueue.IndexOf(track5) == 4);

        }
    }
}
