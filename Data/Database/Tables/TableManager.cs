using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    public class TableManager : Singleton<TableManager>
    {
        public TableManager()
        {
            CreateTables();
            _folderReconstructor = new FolderReconstructor( _folderTable );
            _playlistReconstructor = new PlaylistReconstructor( _playlistTable, _playlistTracksTable );
            _albumReconstructor = new AlbumReconstructor();
        }

        public void Insert( IEnumerable<Track> tracks )
        {
            _trackTable.Insert( tracks );
            _trackTable.ExecuteCommands();
        }

        public void Delete( IEnumerable<Track> tracks )
        {
            _trackTable.Delete( tracks );
            _trackTable.ExecuteCommands();
        }

        public void Insert<T>( T insertionObject )
        {
            IDatabaseTable<T> table = GetTable<T>();
            table.Insert( insertionObject );
            table.ExecuteCommands();
        }

        public void Insert<T>( IEnumerable<T> insertionObjects )
        {
            IDatabaseTable<T> table = GetTable<T>();
            table.Insert( insertionObjects );
            table.ExecuteCommands();
        }

        public void Delete<T>( T insertionObject )
        {
            IDatabaseTable<T> table = GetTable<T>();
            table.Delete( insertionObject );
            table.ExecuteCommands();
        }

        public void AddTrackToPlaylist( Playlist playlist, Track track )
        {
            _playlistTracksTable.Insert( new PlaylistTrack( track.Id, playlist.Id ) );
            _playlistTracksTable.ExecuteCommands();
        }

        public void Delete<T>( IEnumerable<T> insertionObjects )
        {
            IDatabaseTable<T> table = GetTable<T>();
            table.Delete( insertionObjects );
            table.ExecuteCommands();
        }

        public void AddTracksToPlaylist( Playlist playlist, IEnumerable<Track> tracks )
        {
            foreach ( var track in tracks )
            {
                _playlistTracksTable.Insert( new PlaylistTrack( track.Id, playlist.Id ) );
            }
            _playlistTracksTable.ExecuteCommands();
        }

        public void RemoveTrackFromPlaylist( Playlist playlist, Track track )
        {
            _playlistTracksTable.Delete( new PlaylistTrack( track.Id, playlist.Id ) );
            _playlistTracksTable.ExecuteCommands();
        }

        public void RemoveTracksFromPlaylist( Playlist playlist, IEnumerable<Track> tracks )
        {
            foreach ( var track in tracks )
            {
                _playlistTracksTable.Delete( new PlaylistTrack( track.Id, playlist.Id ) );
            }
            _playlistTracksTable.ExecuteCommands();
        }

        public IEnumerable<T> GetObjects<T>()
        {
            if ( typeof( T ) == typeof( Playlist ) )
            {
                var tracks = _trackTable.SelectAll();
                return ( IEnumerable<T> ) _playlistReconstructor.CreatePlaylists( tracks );
            }
            if ( typeof( T ) == typeof( Folder ) )
            {
                var tracks = _trackTable.SelectAll();
                return ( IEnumerable<T> ) _folderReconstructor.CreateFolders( tracks );
            }
            if ( typeof( T ) == typeof( Album ) )
            {
                var tracks = _trackTable.SelectAll();
                return ( IEnumerable<T> ) _albumReconstructor.CreateAlbums( tracks );
            }
            var table = GetTable<T>();
            return table.SelectAll();
        }

        private void CreateTables()
        {
            _folderTable = FolderTable.Create();
            _trackTable = TrackTable.Create();
            _playlistTable = PlaylistTable.Create();
            _playlistTracksTable = PlaylistTracksTable.Create();
            _optionsTable = OptionsTable.Create();
            _effectsTable = EffectsTable.Create();
        }

        private IDatabaseTable<T> GetTable<T>()
        {
            if ( typeof( T ) == typeof( Folder ) )
            {
                return ( IDatabaseTable<T> ) _folderTable;
            }
            if ( typeof( T ) == typeof( Track ) )
            {
                return ( IDatabaseTable<T> ) _trackTable;
            }
            if ( typeof( T ) == typeof( Options ) )
            {
                return ( IDatabaseTable<T> ) _optionsTable;
            }
            if ( typeof( T ) == typeof( BaseEffect ) )
            {
                return ( IDatabaseTable<T> ) _effectsTable;
            }
            if ( typeof( T ) == typeof( Playlist ) )
            {
                return ( IDatabaseTable<T> ) _playlistTable;
            }
            throw new InvalidOperationException();
        }

        private FolderReconstructor _folderReconstructor;
        private PlaylistReconstructor _playlistReconstructor;
        private AlbumReconstructor _albumReconstructor;
        private FolderTable _folderTable;
        private TrackTable _trackTable;
        private PlaylistTable _playlistTable;
        private PlaylistTracksTable _playlistTracksTable;
        private OptionsTable _optionsTable;
        private EffectsTable _effectsTable;
    }
}
