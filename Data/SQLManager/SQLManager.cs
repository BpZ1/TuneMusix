using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TuneMusix.Exceptions;
using TuneMusix.Model;
using TuneMusix.Helpers;

namespace TuneMusix.Data
{
    partial class SQLManager
    {

        SQLiteConnection dbConnection;
        SQLiteDataReader dbReader;
        SQLiteCommand BeginCommand;
        SQLiteCommand EndCommand;

        public SQLManager()
        {
            SQLiteConnection.CreateFile("db_Sqlite_Musix.sqlite");
            dbConnection = new SQLiteConnection("Data Source=db_Sqlite_Musix.db;Version=3;");
            //Initial SQL Querys
            SQLiteCommand sqlCreateTrackTable = new SQLiteCommand("CREATE TABLE if not exists tracks (ID INT UNSIGNED UNIQUE NOT NULL,"+
                                                                                                     "folderID INT UNSIGNED,"+
                                                                                                     "URL VARCHAR (100),"+
                                                                                                     "title VARCHAR(30),"+
                                                                                                     "interpret VARCHAR(30),"+
                                                                                                     "album VARCHAR(30),"+
                                                                                                     "releaseyear VARCHAR(10),"+
                                                                                                     "comm VARCHAR(50),"+
                                                                                                     "genre VARCHAR(20),"+
                                                                                                     "rating INT NOT NULL,PRIMARY KEY(ID),"+
                                                                                                     "FOREIGN KEY (folderID) "+
                                                                                                     "REFERENCES folders(ID) "+
                                                                                                     "ON DELETE CASCADE);",
                                                                                                     dbConnection);
            SQLiteCommand sqlCreateFolderTable = new SQLiteCommand("CREATE TABLE if not exists folders (ID INT UNSIGNED NOT NULL,"+
                                                                                                       "folderID INT,"+
                                                                                                       "URL VARCHAR (100),"+
                                                                                                       "name VARCHAR(30),"+
                                                                                                       "PRIMARY KEY(ID), "+
                                                                                                       "FOREIGN KEY (folderID) "+
                                                                                                       "REFERENCES folders(ID) "+
                                                                                                       "ON DELETE CASCADE);",
                                                                                                       dbConnection);
            SQLiteCommand sqlCreatePlaylistTable = new SQLiteCommand("CREATE TABLE if not exists playlists (ID INT NOT NULL,"+
                                                                                                           "name VARCHAR (30),"+
                                                                                                           "PRIMARY KEY(ID))",
                                                                                                           dbConnection);
            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand("CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL);",
                                                                                                         dbConnection);

            //Only needed if foreign key exists
            //SQLiteCommand sqlFolderInit = new SQLiteCommand("INSERT INTO folders(ID, folderID, URL, name) VALUES(-1, -1,'Nothing','Nothing'",dbConnection);
            dbConnection.Open();
            try
            {
                sqlCreateTrackTable.ExecuteNonQuery();
                sqlCreateFolderTable.ExecuteNonQuery();
                sqlCreatePlaylistTable.ExecuteNonQuery();
                sqlCreateOptionsTable.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }


            BeginCommand = new SQLiteCommand("begin", dbConnection);
            EndCommand = new SQLiteCommand("end", dbConnection);
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
                throw new ConnectionNotSetException("Connection is not null");
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
                throw new ConnectionNotSetException("Connection is not null");
            }
        }    
    }
}
