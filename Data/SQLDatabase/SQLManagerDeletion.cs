using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    partial class SQLManager
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
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commands)
                {
                    command.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        /// <summary>
        /// deletes a track from the tracks database table.
        /// </summary>
        /// <param name="track"></param>
        public void Delete(Track track)
        {
            SQLiteCommand com = new SQLiteCommand("DELETE FROM tracks WHERE ID=@ID;", dbConnection);
            com.Parameters.AddWithValue("ID", track.ID);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                com.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }   
        /// <summary>
        /// Deletes a folder from the folders database table.
        /// </summary>
        /// <param name="folder"></param>
        public void Delete(Folder folder)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM folders WHERE ID=@ID;",dbConnection);
            command.Parameters.AddWithValue("ID",folder.ID);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        /// <summary>
        /// Deletes a playlist from the playlists database table.
        /// </summary>
        /// <param name="playlist"></param>
        public void Delete(Playlist playlist)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlists WHERE ID=@ID;", dbConnection);
            command.Parameters.AddWithValue("ID", playlist.ID);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        /// <summary>
        /// Deletes a list of Tracks from the playlistTracks database table.
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="tracklist"></param>
        public void DeletePlaylistTracks(Playlist playlist,List<Track> tracklist)
        {
            List<SQLiteCommand> commandList = new List<SQLiteCommand>();
            foreach (Track track in tracklist)
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", dbConnection);
                command.Parameters.AddWithValue("trackID",track.ID);
                command.Parameters.AddWithValue("playlistID",playlist.ID);
                commandList.Add(command);
            }
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandList)
                {
                    command.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }

        }
        /// <summary>
        /// Deletes a single Track from the playlistTracks database table.
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="track"></param>
        public void DeletePlaylistTrack(Playlist playlist, Track track)
        {

            SQLiteCommand command = new SQLiteCommand("DELETE FROM playlisttracks WHERE trackID=@trackID,playlistID=@playlistID;", dbConnection);
            command.Parameters.AddWithValue("trackID", track.ID);
            command.Parameters.AddWithValue("playlistID", playlist.ID);           
            try
            {
                BeginCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();              
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }

        }

    }
}
