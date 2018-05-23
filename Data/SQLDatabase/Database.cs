using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TuneMusix.Exceptions;
using TuneMusix.Model;
using TuneMusix.Helpers;
using System.Diagnostics;

namespace TuneMusix.Data.SQLDatabase
{
    /// <summary>
    /// This class contains methods for interaction with the database.
    /// </summary>
    public sealed partial class Database
    {

        SQLiteConnection dbConnection;
        SQLiteDataReader dbReader;
        SQLiteCommand beginCommand;
        SQLiteCommand endCommand;

        private static volatile Database instance;
        private static object lockObject = new Object();

        public static Database Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (lockObject)
                    {
                        if(instance == null)
                        {
                            instance = new Database();
                        }
                    }                  
                }
                return instance;
            }
        }

        private Database()
        {
            dbConnection = new SQLiteConnection("Data Source=musixDB.db;Version=3;");
            beginCommand = new SQLiteCommand("begin", dbConnection);
            endCommand = new SQLiteCommand("end", dbConnection);

            createDatabase();
        }

        private void createDatabase()
        {
            #region sqlcommands
            //Initial SQL Querys
            SQLiteCommand sqlCreateTrackTable = new SQLiteCommand("CREATE TABLE if not exists tracks (ID INT UNSIGNED UNIQUE NOT NULL," +
                                                                                                     "folderID INT UNSIGNED," +
                                                                                                     "URL VARCHAR (100)," +
                                                                                                     "title VARCHAR(30)," +
                                                                                                     "interpret VARCHAR(30)," +
                                                                                                     "album VARCHAR(30)," +
                                                                                                     "releaseyear INT UNSIGNED," +
                                                                                                     "comm VARCHAR(50)," +
                                                                                                     "genre VARCHAR(20)," +
                                                                                                     "rating INT NOT NULL,PRIMARY KEY(ID)," +
                                                                                                     "FOREIGN KEY (folderID) " +
                                                                                                     "REFERENCES folders(ID) " +
                                                                                                     "ON DELETE CASCADE);",
                                                                                                     dbConnection);
            SQLiteCommand sqlCreateFolderTable = new SQLiteCommand("CREATE TABLE if not exists folders (ID INT UNSIGNED NOT NULL," +
                                                                                                       "folderID INT," +
                                                                                                       "URL VARCHAR (100)," +
                                                                                                       "name VARCHAR(30)," +
                                                                                                       "PRIMARY KEY(ID), " +
                                                                                                       "FOREIGN KEY (folderID) " +
                                                                                                       "REFERENCES folders(ID) " +
                                                                                                       "ON DELETE CASCADE);"
                                                                                                       ,dbConnection);
            SQLiteCommand sqlCreatePlaylistTable = new SQLiteCommand("CREATE TABLE if not exists playlists (ID INT NOT NULL," +
                                                                                                           "name VARCHAR(30)," +
                                                                                                           "PRIMARY KEY(ID))",
                                                                                                           dbConnection);
            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand("CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL," +
                                                                                                        "volume INT UNSIGNED," +
                                                                                                        "shuffle INT UNSIGNED," +
                                                                                                        "repeatTrack INT UNSIGNED," +
                                                                                                        "primaryColor INT UNSIGNED," +
                                                                                                        "accentColor INT UNSIGNED," + 
                                                                                                        "theme INT UNSIGNED," +
                                                                                                        "askConfirmation INT UNSIGNED);",
                                                                                                         dbConnection);
            SQLiteCommand sqlCreateTrackPlaylisttable = new SQLiteCommand("CREATE TABLE if not exists playlisttracks(trackID INT UNSIGNED NOT NULL, " +
                                                                                                                   "playlistID INT UNSIGNED NOT NULL, " +
                                                                                                                   "FOREIGN KEY(trackID) " +
                                                                                                                   "REFERENCES tracks(ID) " +
                                                                                                                   "ON DELETE CASCADE, " +
                                                                                                                   "FOREIGN KEY(playlistID) " +
                                                                                                                   "REFERENCES playlists(ID) " +
                                                                                                                   "ON DELETE CASCADE);"
                                                                                                                   ,dbConnection);
            SQLiteCommand sqlCreateEffectsQueueTable = new SQLiteCommand("CREATE TABLE if not exists effectsqueue (queueindex INT UNSIGNED NOT NULL," +
                                                                                                                  "effecttype VARCHAR(20),"+
                                                                                                                  "isactive INT,"+
                                                                                                                  "value0 REAL,"+
                                                                                                                  "value1 REAL," +
                                                                                                                  "value2 REAL," +
                                                                                                                  "value3 REAL," +
                                                                                                                  "value4 REAL," +
                                                                                                                  "value5 REAL," +
                                                                                                                  "value6 REAL," +
                                                                                                                  "value7 REAL," +
                                                                                                                  "value8 REAL," +
                                                                                                                  "value9 REAL," +
                                                                                                                  "value10 int," +
                                                                                                                  "value11 int);",
                                                                                                                  dbConnection);

            #endregion

            OpenDBConnection();
            try
            {
                sqlCreateEffectsQueueTable.ExecuteNonQuery();
                sqlCreateTrackTable.ExecuteNonQuery();
                sqlCreateFolderTable.ExecuteNonQuery();
                sqlCreatePlaylistTable.ExecuteNonQuery();
                sqlCreateOptionsTable.ExecuteNonQuery();
                sqlCreateTrackPlaylisttable.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Database tables were created.");
        }
     
        /// <summary>
        /// Opens the connection to the database
        /// </summary>
        private void OpenDBConnection()
        {
            if(dbConnection != null)
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                {
                    dbConnection.Open();
                }
            }
            else
            {
                throw new ConnectionNotSetException("Connection is null");
            }    
        }
        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        private void CloseDBConnection()
        {
            if (dbConnection != null)
            {
                if (dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    dbConnection.Close();
                }
            }
            else
            {
                throw new ConnectionNotSetException("Connection is null");
            }
        }    
    }
}
