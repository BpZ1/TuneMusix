using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    public sealed partial class Database : IDatabase
    {
        public void Delete(List<Track> tracks)
        {
            List<SQLiteCommand> commands = new List<SQLiteCommand>();
            foreach (Track track in tracks)
            {
                SQLiteCommand com = new SQLiteCommand("DELETE FROM tracks WHERE ID=@ID;",_dbConnection);
                com.Parameters.AddWithValue("ID",track.ID);
                commands.Add(com);
            }

            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commands)
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
       
        public void Delete(Track track)
        {
            SQLiteCommand com = new SQLiteCommand("DELETE FROM tracks WHERE ID=@ID;", _dbConnection);
            com.Parameters.AddWithValue("ID", track.ID);
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
        public void Delete(List<Folder> folders)
        {
            List<SQLiteCommand> commands = new List<SQLiteCommand>();
            foreach (Folder folder in folders)
            {
                SQLiteCommand com = new SQLiteCommand("DELETE FROM folders WHERE ID=@ID;", _dbConnection);
                com.Parameters.AddWithValue("ID", folder.ID);
                commands.Add(com);
            }

            OpenDBConnection();
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commands)
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
        
        public void Delete(Folder folder)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM folders WHERE ID=@ID;",_dbConnection);
            command.Parameters.AddWithValue("ID",folder.ID);
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
       
        public void Delete(Playlist playlist)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlists WHERE ID=@ID;", _dbConnection);
            command.Parameters.AddWithValue("ID", playlist.ID);
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
       
        public void Delete(Playlist playlist, List<Track> tracks)
        {
            List<PlaylistTrack> playlistTracks = new List<PlaylistTrack>();
            foreach (Track track in tracks)
            {
                playlistTracks.Add(new PlaylistTrack(track.ID, playlist.ID));
            }
            this.delete(playlistTracks);
        }

        private void delete(List<PlaylistTrack> playlistTracks)
        {
            List<SQLiteCommand> commandList = new List<SQLiteCommand>();
            foreach (PlaylistTrack track in playlistTracks)
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", _dbConnection);
                command.Parameters.AddWithValue("trackID", track.TrackID);
                command.Parameters.AddWithValue("playlistID", track.PlaylistID);
                commandList.Add(command);
            }
            try
            {
                _beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandList)
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
       
        public void Delete(Playlist playlist, Track track)
        {
            PlaylistTrack playlistTrack = new PlaylistTrack(track.ID, playlist.ID);
            this.Delete(playlistTrack);
        }

        private void Delete(PlaylistTrack playlistTrack)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", _dbConnection);
            command.Parameters.AddWithValue("trackID", playlistTrack.TrackID);
            command.Parameters.AddWithValue("playlistID", playlistTrack.PlaylistID);           
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
