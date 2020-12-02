using System;
using System.Data.SQLite;
using System.Diagnostics;
using TuneMusix.Exceptions;

namespace TuneMusix.Data.SQLDatabase
{
    /// <summary>
    /// This class contains methods for interaction with the database.
    /// </summary>
    public sealed partial class DatabaseLegacy
    {

        private SQLiteConnection _dbConnection;
        private SQLiteDataReader _dbReader;
        private SQLiteCommand _beginCommand;
        private SQLiteCommand _endCommand;

        private static volatile DatabaseLegacy _instance;
        private static object _lockObject = new Object();

        public static DatabaseLegacy Instance
        {
            get
            {
                if ( _instance == null )
                {
                    lock ( _lockObject )
                    {
                        if ( _instance == null )
                        {
                            _instance = new DatabaseLegacy();
                        }
                    }
                }
                return _instance;
            }
        }

        private DatabaseLegacy()
        {
            _dbConnection = new SQLiteConnection( "Data Source=musixDB.db;Version=3;" );
            _beginCommand = new SQLiteCommand( "begin", _dbConnection );
            _endCommand = new SQLiteCommand( "end", _dbConnection );

            CreateDatabase();
        }

        private void CreateDatabase()
        {
            #region sqlcommands
            //Initial SQL Querys
            SQLiteCommand sqlCreateTrackTable = new SQLiteCommand( "CREATE TABLE if not exists tracks (ID INT UNSIGNED UNIQUE NOT NULL," +
                                                                                                     "folderID INT UNSIGNED," +
                                                                                                     "URL VARCHAR (100)," +
                                                                                                     "title VARCHAR(30)," +
                                                                                                     "interpret VARCHAR(30)," +
                                                                                                     "album VARCHAR(30)," +
                                                                                                     "releaseyear INT UNSIGNED," +
                                                                                                     "comm VARCHAR(50)," +
                                                                                                     "genre VARCHAR(20)," +
                                                                                                     "rating INT NOT NULL," +
                                                                                                     "duration VARCHAR(20)," +
                                                                                                     "PRIMARY KEY(ID)," +
                                                                                                     "FOREIGN KEY (folderID) " +
                                                                                                     "REFERENCES folders(ID) " +
                                                                                                     "ON DELETE CASCADE);",
                                                                                                     _dbConnection );
            SQLiteCommand sqlCreateFolderTable = new SQLiteCommand( "CREATE TABLE if not exists folders (ID INT UNSIGNED NOT NULL," +
                                                                                                       "folderID INT," +
                                                                                                       "URL VARCHAR (100)," +
                                                                                                       "name VARCHAR(30)," +
                                                                                                       "PRIMARY KEY(ID), " +
                                                                                                       "FOREIGN KEY (folderID) " +
                                                                                                       "REFERENCES folders(ID) " +
                                                                                                       "ON DELETE CASCADE);"
                                                                                                       , _dbConnection );
            SQLiteCommand sqlCreatePlaylistTable = new SQLiteCommand( "CREATE TABLE if not exists playlists (ID INT NOT NULL," +
                                                                                                           "name VARCHAR(30)," +
                                                                                                           "PRIMARY KEY(ID))",
                                                                                                           _dbConnection );
            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand( "CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL," +
                                                                                                        "volume INT UNSIGNED," +
                                                                                                        "shuffle INT UNSIGNED," +
                                                                                                        "repeatTrack INT UNSIGNED," +
                                                                                                        "primaryColor INT UNSIGNED," +
                                                                                                        "accentColor INT UNSIGNED," +
                                                                                                        "theme INT UNSIGNED," +
                                                                                                        "askConfirmation INT UNSIGNED);",
                                                                                                         _dbConnection );
            SQLiteCommand sqlCreateTrackPlaylisttable = new SQLiteCommand( "CREATE TABLE if not exists playlisttracks(trackID INT UNSIGNED NOT NULL, " +
                                                                                                                   "playlistID INT UNSIGNED NOT NULL, " +
                                                                                                                   "FOREIGN KEY(trackID) " +
                                                                                                                   "REFERENCES tracks(ID) " +
                                                                                                                   "ON DELETE CASCADE, " +
                                                                                                                   "PRIMARY KEY(trackID)," +
                                                                                                                   "FOREIGN KEY(playlistID) " +
                                                                                                                   "REFERENCES playlists(ID) " +
                                                                                                                   "ON DELETE CASCADE);"
                                                                                                                   , _dbConnection );
            SQLiteCommand sqlCreateEffectsQueueTable = new SQLiteCommand( "CREATE TABLE if not exists effectsqueue (queueindex INT UNSIGNED NOT NULL," +
                                                                                                                  "effecttype VARCHAR(20)," +
                                                                                                                  "isactive INT," +
                                                                                                                  "value0 REAL," +
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
                                                                                                                  _dbConnection );

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
            Debug.WriteLine( "Database tables were created." );
        }

        /// <summary>
        /// Opens the connection to the database
        /// </summary>
        private void OpenDBConnection()
        {
            if ( _dbConnection != null )
            {
                if ( _dbConnection.State != System.Data.ConnectionState.Open )
                {
                    _dbConnection.Open();
                }
            }
            else
            {
                throw new ConnectionNotSetException( "Connection is null" );
            }
        }
        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        private void CloseDBConnection()
        {
            if ( _dbConnection != null )
            {
                if ( _dbConnection.State != System.Data.ConnectionState.Closed )
                {
                    _dbConnection.Close();
                }
            }
            else
            {
                throw new ConnectionNotSetException( "Connection is null" );
            }
        }
    }
}
