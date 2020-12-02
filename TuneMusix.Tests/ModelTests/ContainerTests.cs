
using NUnit.Framework;
using TuneMusix.Model;

namespace TuneMusix.Tests.ModelTests
{
    [TestFixture]
    class ContainerTests
    {
        private Track track1 = new Track( "a", 1, "url1", null, null, null, null, null, "00:00:12" );

        [Test]
        public void AlbumIsEmpty()
        {
            Album album = new Album( "Album1" );

            Assert.IsTrue( album.IsEmpty );
            album.Add( track1 );
            Assert.IsFalse( album.IsEmpty );
            album.Remove( track1 );
            Assert.IsTrue( album.IsEmpty );
        }

        [Test]
        public void FolderIsEmpty()
        {
            Folder folder1 = new Folder( "Folder1", "URL1", "123" );
            Folder folder2 = new Folder( "Folder2", "URL2", "124" );
            Folder folder3 = new Folder( "Folder3", "URL3", "125" );

            Assert.IsTrue( folder1.IsEmpty );
            folder1.Add( track1 );
            Assert.IsFalse( folder1.IsEmpty );
            folder2.Add( folder1 );
            Assert.IsFalse( folder2.IsEmpty );
            folder3.Add( folder2 );
            Assert.IsFalse( folder3.IsEmpty );
            folder1.Remove( track1 );
            Assert.IsTrue( folder1.IsEmpty );
        }

        [Test]
        public void TrackQueueAdd()
        {
            TrackQueue queue = new TrackQueue();
            bool adding1 = queue.Add( track1 );
            Assert.IsTrue( adding1 );
            Assert.AreEqual( queue.Queue.Count, 1 );
            bool adding2 = queue.Add( track1 );
            Assert.IsFalse( adding2 );
            Assert.AreEqual( queue.Queue.Count, 1 );
        }
    }
}
