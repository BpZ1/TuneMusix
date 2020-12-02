using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TuneMusix.Data.Database;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Tests.DatabaseTests
{
    [TestFixture]
    public class DatabaseTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DatabaseAccess.Create( FilePath );
            var databaseFileExists = File.Exists( $"{FilePath}.db" );
            Assert.IsTrue( databaseFileExists, "Database file was not created." );
            CreateTracksAndFolders();
            CreateEffects();
            CreatePlaylists( _tracks );
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DatabaseAccess.Instance.Dispose();
            if ( File.Exists( $"{FilePath}.db" ) )
            {
                File.Delete( $"{FilePath}.db" );
            }
        }

        [Test]
        public void InsertAndReadFromDatabaseTest()
        {
            InsertIntoDatabase();

            var folders = DatabaseAccess.TableManager.GetObjects<Folder>();
            Assert.IsTrue( CheckListEqual( folders, _folders ) );

            var tracks = DatabaseAccess.TableManager.GetObjects<Track>();
            Assert.IsTrue( CheckListEqual( tracks, _tracks ) );

            var playlists = DatabaseAccess.TableManager.GetObjects<Playlist>();
            Assert.IsTrue( CheckListEqual( playlists, _playlists ) );

            var effects = DatabaseAccess.TableManager.GetObjects<BaseEffect>();
            Assert.IsTrue( CheckListEqual( effects, _effects ) );
        }

        private bool CheckListEqual<T>( IEnumerable<T> firstList, IEnumerable<T> secondList )
        {
            var result = true;
            if ( firstList.Count() != secondList.Count() )
            {
                return false;
            }
            for ( var i = 0; i < firstList.Count(); i++ )
            {
                if ( !firstList.ElementAt( i ).Equals( secondList.ElementAt( i ) ) )
                {
                    result = false;
                }
            }
            return result;
        }

        private void InsertIntoDatabase()
        {
            DatabaseAccess.TableManager.Insert<Folder>( _folders );
            DatabaseAccess.TableManager.Insert<Track>( _tracks );
            DatabaseAccess.TableManager.Insert<Playlist>( _playlists );
            DatabaseAccess.TableManager.Insert<BaseEffect>( _effects );
        }

        private void CreateTracksAndFolders()
        {
            _folders = new List<Folder>();
            var tracks1 = new List<Track>();
            for ( var i = 0; i < 5; i++ )
            {
                tracks1.Add( new Track( $"url{i}", $"id{i}", FolderId1, $"title{i}", $"interpret{i}", $"album{i}", 123 + i, $"comm{i}", $"genre{i}", i, $"00:0{i}" ) );
            }
            var folder1 = new Folder( "folder1", "folderUrl1", FolderId1 );
            folder1.AddRange( tracks1 );
            _folders.Add( folder1 );
            var tracks2 = new List<Track>();
            for ( var i = 5; i < 10; i++ )
            {
                tracks2.Add( new Track( $"url{i}", $"id{i}", FolderId2, $"title{i}", $"interpret{i}", $"album{i}", 123 + i, $"comm{i}", $"genre{i}", i, $"00:0{i}" ) );
            }
            var folder2 = new Folder( "folder2", "folderUrl2", FolderId2 );
            folder2.AddRange( tracks2 );
            _folders.Add( folder2 );
            _tracks = new List<Track>();
            _tracks.AddRange( tracks1.Concat( tracks2 ) );
        }

        private void CreateEffects()
        {
            _effects = new List<BaseEffect>();
            _effects.Add( new ChorusEffect()
            {
                QueueIndex = 0
            } );
            _effects.Add( new DistortionEffect()
            {
                QueueIndex = 1
            } );
            _effects.Add( new FlangerEffect()
            {
                QueueIndex = 2
            } );
            _effects.Add( new EqualizerEffect()
            {
                QueueIndex = 3
            } );
            _effects.Add( new GargleEffect()
            {
                QueueIndex = 4
            } );
            _effects.Add( new EchoEffect()
            {
                QueueIndex = 5
            } );
            _effects.Add( new CompressorEffect()
            {
                QueueIndex = 6
            } );
            _effects.Add( new ReverbEffect()
            {
                QueueIndex = 7
            } );
        }

        private void CreatePlaylists( IEnumerable<Track> tracks )
        {
            _playlists = new List<Playlist>();
            var playlist1 = new Playlist( "playlist1", "playlistId1" );
            playlist1.AddRange( tracks );
            _playlists.Add( playlist1 );

            var playlist2 = new Playlist( "playlist2", "playlistId2" );
            playlist2.AddRange( tracks );
            _playlists.Add( playlist2 );
        }

        private List<Track> _tracks;
        private List<BaseEffect> _effects;
        private List<Playlist> _playlists;
        private List<Folder> _folders;
        private const string FolderId1 = "folderId1";
        private const string FolderId2 = "folderId2";
        private const string DatabaseFileName = "TestDatabase";
        private static readonly string FilePath = Path.Combine( Directory.GetCurrentDirectory(), DatabaseFileName );
    }
}
