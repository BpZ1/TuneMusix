using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.Util;
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

        

        [TestMethod]
        public void DeleteTrackTest()
        {
            //TODO
        }

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
            //TODO
        }

        [TestMethod]
        public void ShuffleTest()
        {
            //TODO
        }
    }
}
