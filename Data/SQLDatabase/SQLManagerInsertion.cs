using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    partial class SQLManager
    {

        /// <summary>
        /// Adds a given Track to the Database.
        /// </summary>
        /// <param name="track"></param>
        private void AddTrack(Track track)
        {
            Logger.Log("Track: '" + track.sourceURL + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO tracks (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "title," +
                                                                                         "interpret," +
                                                                                         "album," +
                                                                                         "releaseyear," +
                                                                                         "comm," +
                                                                                         "genre," +
                                                                                         "rating)" +
                                                                                 "VALUES(@ID," +
                                                                                        "@folderID," +
                                                                                        "@URL," +
                                                                                        "@Title," +
                                                                                        "@Interpret," +
                                                                                        "@Album," +
                                                                                        "@ReleaseYear," +
                                                                                        "@Comm," +
                                                                                        "@Genre," +
                                                                                        "@Rating);",                                                                                   
                                                                                        dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", track.ID);
            if (track.FolderID == 0)
            {
                sqlcommand.Parameters.AddWithValue("folderID", null);
            }
            else
            {
                sqlcommand.Parameters.AddWithValue("folderID", track.FolderID);
            }
            sqlcommand.Parameters.AddWithValue("URL", track.sourceURL);
            sqlcommand.Parameters.AddWithValue("Title", track.Title);
            sqlcommand.Parameters.AddWithValue("Interpret", track.Interpret);
            sqlcommand.Parameters.AddWithValue("Album", track.Album);
            sqlcommand.Parameters.AddWithValue("ReleaseYear", track.Year);
            sqlcommand.Parameters.AddWithValue("Comm", track.Comm);
            sqlcommand.Parameters.AddWithValue("Genre", track.Genre);
            sqlcommand.Parameters.AddWithValue("Rating", track.Rating);

            OpenDBConnection();
            try
            {
                sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void AddAll(List<Track> tracklist)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            
            foreach (Track track in tracklist)
            {
                
                SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO tracks (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "title," +
                                                                                         "interpret," +
                                                                                         "album," +
                                                                                         "releaseyear," +
                                                                                         "comm," +
                                                                                         "genre," +
                                                                                         "rating)" +
                                                                                 "VALUES(@ID," +
                                                                                        "@folderID," +
                                                                                        "@URL," +
                                                                                        "@Title," +
                                                                                        "@Interpret," +
                                                                                        "@Album," +
                                                                                        "@ReleaseYear," +
                                                                                        "@Comm," +
                                                                                        "@Genre," +
                                                                                        "@Rating);",
                                                                                        dbConnection);
                sqlcommand.Parameters.AddWithValue("ID", track.ID);
                if (track.FolderID == 0)
                {
                    sqlcommand.Parameters.AddWithValue("folderID", null);
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("folderID", track.FolderID);
                }              
                sqlcommand.Parameters.AddWithValue("URL", track.sourceURL);
                sqlcommand.Parameters.AddWithValue("Title", track.Title);
                sqlcommand.Parameters.AddWithValue("Interpret", track.Interpret);
                sqlcommand.Parameters.AddWithValue("Album", track.Album);
                sqlcommand.Parameters.AddWithValue("ReleaseYear", track.Year);
                sqlcommand.Parameters.AddWithValue("Comm", track.Comm);
                sqlcommand.Parameters.AddWithValue("Genre", track.Genre);
                sqlcommand.Parameters.AddWithValue("Rating", track.Rating);
                commandlist.Add(sqlcommand);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
                {
                    command.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Tracks were added to DB");
        }

        public void AddAll(List<Folder> folders)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            foreach (Folder f in folders)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO folders (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "name) " +
                                                                                  "VALUES(@ID," +
                                                                                         "@folderID," +
                                                                                         "@URL," +
                                                                                         "@name);",
                                                                                         dbConnection);
                command.Parameters.AddWithValue("ID",f.ID);
                if (f.FolderID == 1)
                {
                    command.Parameters.AddWithValue("folderID",null);
                }
                else
                {
                    command.Parameters.AddWithValue("folderID",f.FolderID);
                }
                command.Parameters.AddWithValue("URL",f.URL);
                command.Parameters.AddWithValue("name",f.Name);
                commandlist.Add(command);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand com in commandlist)
                {
                    com.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Folders were added to DB");
        }

        public void AddPlaylist(Playlist playlist)
        {
            Logger.Log("Playlist: '" + playlist.Name + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO playlists (ID," +
                                                                                            "name) " +
                                                                                     "VALUES(@ID," +
                                                                                            "@name);",
                                                                                             dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", playlist.ID);
            sqlcommand.Parameters.AddWithValue("name", playlist.Name);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                sqlcommand.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Playlist was added to DB: " + playlist.Name);
        }

        public void AddPlaylistTrack(Track track,Playlist playlist)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO playlisttracks(trackID," +
                                                                                            "playlistID) " +
                                                                                     "VALUES(@trackID,"+
                                                                                            "@playlistID)",
                                                                                             dbConnection);
            command.Parameters.AddWithValue("trackID",track.ID);
            command.Parameters.AddWithValue("playlistID",playlist.ID);

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

        public void AddAllPlaylistTracks(Playlist playlist,List<Track> tracklist)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            foreach (Track track in tracklist)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO playlisttracks(trackID,"+
                                                                                               "playlistID) "+
                                                                                         "VALUES(@trackID,"+
                                                                                                "@playlistID)",
                                                                                                 dbConnection);
                command.Parameters.AddWithValue("trackID",track.ID);
                command.Parameters.AddWithValue("playlistID",playlist.ID);
                commandlist.Add(command);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
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

        public void UpdateOptions(long IDGenStand, Options options)
        {
            SQLiteCommand sqlClearCommand = new SQLiteCommand("DROP TABLE options", dbConnection);
            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand("CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL," +
                                                                                                        "volume INT UNSIGNED," +
                                                                                                        "shuffle INT UNSIGNED," +
                                                                                                        "repeatTrack INT UNSIGNED);",
                                                                                                         dbConnection);
            SQLiteCommand sqlAddCommand = new SQLiteCommand("INSERT INTO options (IDgen,volume,shuffle,repeatTrack) VALUES (@IDgen,@volume,@shuffle,@repeatTrack);", dbConnection);
            sqlAddCommand.Parameters.AddWithValue("IDgen", IDGenStand);
            sqlAddCommand.Parameters.AddWithValue("volume",options.Volume);
            int shuffle;
            if (options.Shuffle) shuffle = 1;
            else shuffle = 0;
            sqlAddCommand.Parameters.AddWithValue("shuffle",shuffle);
            sqlAddCommand.Parameters.AddWithValue("repeatTrack",options.RepeatTrack);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                sqlClearCommand.ExecuteNonQuery();
                sqlCreateOptionsTable.ExecuteNonQuery();
                sqlAddCommand.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

    }
}
