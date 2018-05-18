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
                SQLiteCommand com = new SQLiteCommand("DELETE FROM tracks WHERE ID=@ID;",dbConnection);
                com.Parameters.AddWithValue("ID",track.ID);
                commands.Add(com);
            }

            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commands)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
       
        public void Delete(Track track)
        {
            SQLiteCommand com = new SQLiteCommand("DELETE FROM tracks WHERE ID=@ID;", dbConnection);
            com.Parameters.AddWithValue("ID", track.ID);
            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                com.ExecuteNonQuery();
                endCommand.ExecuteNonQuery();
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
                SQLiteCommand com = new SQLiteCommand("DELETE FROM folders WHERE ID=@ID;", dbConnection);
                com.Parameters.AddWithValue("ID", folder.ID);
                commands.Add(com);
            }

            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commands)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        
        public void Delete(Folder folder)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM folders WHERE ID=@ID;",dbConnection);
            command.Parameters.AddWithValue("ID",folder.ID);
            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
       
        public void Delete(Playlist playlist)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlists WHERE ID=@ID;", dbConnection);
            command.Parameters.AddWithValue("ID", playlist.ID);
            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                endCommand.ExecuteNonQuery();
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
                SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", dbConnection);
                command.Parameters.AddWithValue("trackID", track.TrackID);
                command.Parameters.AddWithValue("playlistID", track.PlaylistID);
                commandList.Add(command);
            }
            try
            {
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandList)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
       
        public void Delete(Playlist playlist, Track track)
        {
            PlaylistTrack playlistTrack = new PlaylistTrack(track.ID, playlist.ID);
            this.delete(playlistTrack);
        }

        private void delete(PlaylistTrack playlistTrack)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", dbConnection);
            command.Parameters.AddWithValue("trackID", playlistTrack.TrackID);
            command.Parameters.AddWithValue("playlistID", playlistTrack.PlaylistID);           
            try
            {
                beginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();              
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        
    }
}
