using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    public sealed partial class DatabaseLegacy : IDatabase
    {
        public void Delete( IEnumerable<Track> tracks )
        {
            List<SQLiteCommand> commands = new List<SQLiteCommand>();
            foreach ( Track track in tracks )
            {
                SQLiteCommand com = new SQLiteCommand( "DELETE FROM tracks WHERE ID=@ID;", _dbConnection );
                com.Parameters.AddWithValue( "ID", track.Id );
                commands.Add( com );
            }

            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach ( SQLiteCommand command in commands )
                {
                    command.ExecuteNonQuery();
                }
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Delete( Track track )
        {
            SQLiteCommand com = new SQLiteCommand( "DELETE FROM tracks WHERE ID=@ID;", _dbConnection );
            com.Parameters.AddWithValue( "ID", track.Id );
            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                com.ExecuteNonQuery();
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        public void Delete( IEnumerable<Folder> folders )
        {
            List<SQLiteCommand> commands = new List<SQLiteCommand>();
            foreach ( Folder folder in folders )
            {
                SQLiteCommand com = new SQLiteCommand( "DELETE FROM folders WHERE ID=@ID;", _dbConnection );
                com.Parameters.AddWithValue( "ID", folder.Id );
                commands.Add( com );
            }

            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach ( SQLiteCommand command in commands )
                {
                    command.ExecuteNonQuery();
                }
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Delete( Folder folder )
        {
            SQLiteCommand command = new SQLiteCommand( "DELETE FROM folders WHERE ID=@ID;", _dbConnection );
            command.Parameters.AddWithValue( "ID", folder.Id );
            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Delete( Playlist playlist )
        {
            SQLiteCommand command = new SQLiteCommand( "DELETE FROM playlists WHERE ID=@ID;", _dbConnection );
            command.Parameters.AddWithValue( "ID", playlist.Id );
            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Delete( Playlist playlist, IEnumerable<Track> tracks )
        {
            List<PlaylistTrack> playlistTracks = new List<PlaylistTrack>();
            foreach ( Track track in tracks )
            {
                playlistTracks.Add( new PlaylistTrack( track.Id, playlist.Id ) );
            }
            delete( playlistTracks );
        }

        private void delete( IEnumerable<PlaylistTrack> playlistTracks )
        {
            List<SQLiteCommand> commandList = new List<SQLiteCommand>();
            foreach ( PlaylistTrack track in playlistTracks )
            {
                SQLiteCommand command = new SQLiteCommand( "DELETE FROM playlisttracks WHERE trackID=@trackID AND playlistID=@playlistID;", _dbConnection );
                command.Parameters.AddWithValue( "trackID", track.TrackID );
                command.Parameters.AddWithValue( "playlistID", track.PlaylistID );
                commandList.Add( command );
            }
            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach ( SQLiteCommand command in commandList )
                {
                    command.ExecuteNonQuery();
                }
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Delete( Playlist playlist, Track track )
        {
            PlaylistTrack playlistTrack = new PlaylistTrack( track.Id, playlist.Id );
            Delete( playlistTrack );
        }

        private void Delete( PlaylistTrack playlistTrack )
        {
            SQLiteCommand command = new SQLiteCommand( "DELETE FROM playlisttracks WHERE trackID=@trackID AND playlistID=@playlistID;", _dbConnection );
            command.Parameters.AddWithValue( "trackID", playlistTrack.TrackID );
            command.Parameters.AddWithValue( "playlistID", playlistTrack.PlaylistID );
            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                _endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

    }
}
