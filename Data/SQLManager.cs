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
            //Initial SQL Querys
            SQLiteCommand sqlCreateTrackTable = new SQLiteCommand("CREATE TABLE if not exists tracks (ID INT UNSIGNED UNIQUE NOT NULL,folderID INT,URL VARCHAR (100),title VARCHAR(30),interpret VARCHAR(30),album VARCHAR(30),releaseyear VARCHAR(10),comm VARCHAR(50),genre VARCHAR(20),rating INT NOT NULL,PRIMARY KEY(ID));", dbConnection);
            SQLiteCommand sqlCreateFolderTable = new SQLiteCommand("CREATE TABLE if not exists folders (ID INT NOT NULL,folderID INT, URL VARCHAR (100),name VARCHAR(30),PRIMARY KEY(ID))", dbConnection);
            SQLiteCommand sqlCreatePlaylistTable = new SQLiteCommand("CREATE TABLE if not exists playlists (ID INT NOT NULL, name VARCHAR (30),PRIMARY KEY(ID))", dbConnection);

            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand("CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL)",dbConnection);
            //Only needed if foreign key exists
            //SQLiteCommand sqlFolderInit = new SQLiteCommand("INSERT INTO folders(ID, folderID, URL, name) VALUES(-1, -1,'Nothing','Nothing'",dbConnection);
            dbConnection.Open();
            try
            {
                sqlCreateTrackTable.ExecuteNonQuery();
                sqlCreateFolderTable.ExecuteNonQuery();
                sqlCreatePlaylistTable.ExecuteNonQuery();
                sqlCreateFolderTable.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }      
        }


        //Insertion methods

        /// <summary>
        /// Adds a given Track to the Database.
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(Track track)
        {        
            Logger.Log("Track: '" + track.url + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT INTO tracks (ID,folderID,URL,title,interpret,album,releaseyear,comm,genre,rating) VALUES(@ID,@folderID,@URL,@Title,@Interpret,@Album,@ReleaseYear,@Comm,@Genre,@Rating);", dbConnection);
            sqlcommand.Parameters.AddWithValue("ID",track.ID);
            sqlcommand.Parameters.AddWithValue("folderID",track.FolderID);
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
        /// <summary>
        /// Adds a Folder and all of its content to the Database.
        /// </summary>
        /// <param name="folder"></param>
        public void AddFolder(Folder folder)
        {
            Logger.Log("Folder: '" + folder.URL + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT INTO folders (ID,folderID,URL,name) VALUES(@ID,@folderID,@URL,@name);", dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", folder.ID);
            sqlcommand.Parameters.AddWithValue("folderID", folder.FolderID);
            sqlcommand.Parameters.AddWithValue("URL", folder.URL);
            sqlcommand.Parameters.AddWithValue("name", folder.Name);
            dbConnection.Open();
            try
            {
                sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }
            foreach (Track track in folder.Tracklist)
            {
                this.AddTrack(track);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.AddFolder(f);
            }
        }

        public void AddPlaylist(Playlist playlist)
        {
            Logger.Log("Playlist: '" + playlist.Name + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT INTO playlists (ID,name) VALUES(@ID,@name);", dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", playlist.ID);
            sqlcommand.Parameters.AddWithValue("name",playlist.Name);
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

        public void UpdateOptions(long IDGenStand,Options options)
        {
            SQLiteCommand sqlClearCommand = new SQLiteCommand("TRUNCATE TABLE options", dbConnection);
            SQLiteCommand sqlAddCommand = new SQLiteCommand("INSERT INTO options (ID) VALUES (@ID);");
            sqlAddCommand.Parameters.AddWithValue("ID",IDGenStand);
            dbConnection.Open();
            try
            {
                sqlClearCommand.ExecuteNonQuery();
                sqlAddCommand.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Clone();
            }
        }



        //Selection methods

        /// <summary>
        /// Returns all Tracks contained in the Database.
        /// </summary>
        /// <returns></returns>
        public List<Track> GetTracks()
        {
            List<Track> tracklist = new List<Track>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM tracks", dbConnection);
            dbConnection.Open();
            dbReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (dbReader.Read())
                {
                    Track track = new Track(dbReader.GetString(2),dbReader.GetInt32(0),dbReader.GetInt64(1));
                    track.Title = dbReader.GetString(3);
                    track.Interpret = dbReader.GetString(4);
                    track.Album = dbReader.GetString(5);
                    track.Year = dbReader.GetString(6);
                    track.Comm = dbReader.GetString(7);
                    track.Genre = dbReader.GetString(8);
                    track.Rating = dbReader.GetInt32(9);                
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

        public List<Folder> GetFolders()
        {
            List<Folder> folderlist = new List<Folder>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM tracks", dbConnection);
            dbConnection.Open();
            dbReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (dbReader.Read())
                {
                    Folder folder = new Folder(dbReader.GetString(3),dbReader.GetString(2), dbReader.GetInt32(0));
                    folder.FolderID = dbReader.GetInt64(1);
                    folderlist.Add(folder);
                }
            }
            finally
            {
                dbReader.Close();
                dbConnection.Close();
            }
            return folderlist;
        }

        public Options GetOptions()
        {
            Options options = Options.Instance;

            //Implement

            return options;
        }

        public long GetIDCounterStand()
        {
            long IDCounter;
            SQLiteCommand command = new SQLiteCommand("SELECT ID FROM options",dbConnection);
            dbConnection.Open();
            dbReader = command.ExecuteReader();
            try
            {
                IDCounter = dbReader.GetInt64(0);
            }
            finally
            {
                dbReader.Close();
                dbConnection.Close();
            }
            return IDCounter;
        }

        


        
    }
}
