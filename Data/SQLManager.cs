using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TuneMusix.Model;
using TuneMusix.Helpers;

namespace TuneMusix.Data
{
    class SQLManager
    {

        SQLiteConnection dbConnection;
        SQLiteDataReader dbReader;

       public SQLManager()
        {
            SQLiteConnection.CreateFile("db_Sqlite_Musix.sqlite");
            dbConnection = new SQLiteConnection("Data Source=db_Sqlite_Musix.db;Version=3;");
            SQLiteCommand sqlCreateTrackTable = new SQLiteCommand("CREATE TABLE if not exists tracks (URL VARCHAR (100),title VARCHAR(30),interpret VARCHAR(30),album VARCHAR(30),releaseyear VARCHAR(10),comm VARCHAR(50),genre VARCHAR(20),rating INT NOT NULL);", dbConnection);
            SQLiteCommand sqlCreateFolderTable = new SQLiteCommand("CREATE TABLE if not exists folders (URL VARCHAR (100),name VARCHAR(30))", dbConnection);
            dbConnection.Open();
            sqlCreateTrackTable.ExecuteNonQuery();
            sqlCreateFolderTable.ExecuteNonQuery();
            dbConnection.Close();
        }

        public void AddTrack(Track track)
        {        
            Logger.Log("Track: '" + track.url + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT INTO tracks (URL,title,interpret,album,releaseyear,comm,genre,rating) VALUES(@URL,@Title,@Interpret,@Album,@ReleaseYear,@Comm,@Genre,@Rating);", dbConnection);
            sqlcommand.Parameters.AddWithValue("URL",track.url);
            sqlcommand.Parameters.AddWithValue("Title", track.Title);
            sqlcommand.Parameters.AddWithValue("Interpret", track.Interpret);
            sqlcommand.Parameters.AddWithValue("Album", track.Album);
            sqlcommand.Parameters.AddWithValue("ReleaseYear", track.Year);
            sqlcommand.Parameters.AddWithValue("Comm", track.Comm);
            sqlcommand.Parameters.AddWithValue("Genre", track.Genre);
            sqlcommand.Parameters.AddWithValue("Rating", track.Rating);
            dbConnection.Open();
            try
            {
                sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }     
        }

        public List<Track> GetTracks()
        {

            List<Track> tracklist = new List<Track>();
            string dbCommand = "SELECT * FROM tracks";
            SQLiteCommand command = new SQLiteCommand(dbCommand, dbConnection);
            dbConnection.Open();
            dbReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (dbReader.Read())
                {
                    Track track = new Track(dbReader.GetString(0));
                    track.Title = dbReader.GetString(1);
                    track.Interpret = dbReader.GetString(2);
                    track.Album = dbReader.GetString(3);
                    track.Year = dbReader.GetString(4);
                    track.Comm = dbReader.GetString(5);
                    track.Genre = dbReader.GetString(6);
                    track.Rating = dbReader.GetInt32(7);
                    
                    tracklist.Add(track);
  
                }

            }
            finally
            {
                dbReader.Close();
                dbConnection.Close();
            }
            return tracklist;
        }

        public void AddFolderToDB(Folder folder)
        {
           //Implement
        }

        public void AddPlaylist(Playlist playlist)
        {
            //Implement
        }
    }
}
