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

        public void Delete(List<Playlist> playlists)
        {

        }

        public void Delete(Playlist playlist)
        {

        }



    }
}
