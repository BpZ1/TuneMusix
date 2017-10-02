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
    partial class SQLManager
    {

        SQLiteConnection dbConnection;
        SQLiteDataReader dbReader;
        SQLiteCommand BeginCommand;
        SQLiteCommand EndCommand;

        public SQLManager()
        {
            //SQLiteConnection.CreateFile("musixDB.db");
            dbConnection = new SQLiteConnection("Data Source=musixDB.db;Version=3;");           
            BeginCommand = new SQLiteCommand("begin",dbConnection);
            EndCommand = new SQLiteCommand("end",dbConnection);
        }

        public void CreateDatabase()
        {
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
                                                                                                           "name VARCHAR (30)," +
                                                                                                           "PRIMARY KEY(ID))",
                                                                                                           dbConnection);
            SQLiteCommand sqlCreateOptionsTable = new SQLiteCommand("CREATE TABLE if not exists options (IDgen INT UNSIGNED NOT NULL," +
                                                                                                        "volume INT UNSIGNED," +
                                                                                                        "shuffle INT UNSIGNED," +
                                                                                                        "repeatTrack INT UNSIGNED);",
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
            SQLiteCommand sqlCreateEffectsQueueTable = new SQLiteCommand("CREATE TABLE if not exists effectsqueue (queueindex INT UNSIGNED NOT NULL);",
                                                                                                                  dbConnection);
            SQLiteCommand sqlCreateFlangerTable = new SQLiteCommand("CREATE TABLE if not exists flangereffect (queueindex INT," +
                                                                                                            "isactive INT," +
                                                                                                            "delay REAL," +
                                                                                                            "depth REAL," +
                                                                                                            "feedback REAL," +
                                                                                                            "frequency REAL," +
                                                                                                            "wetdrymix REAL," +
                                                                                                            "waveform INT," +
                                                                                                            "FOREIGN KEY(queueindex) " +
                                                                                                            "REFERENCES effectsqueue(queueindex) " +
                                                                                                            "ON DELETE CASCADE);"
                                                                                                            ,dbConnection);
            SQLiteCommand sqlCreateChorusTable = new SQLiteCommand("CREATE TABLE if not exists choruseffect (queueindex INT," +
                                                                                                          "isactive INT," +
                                                                                                          "delay REAL," +
                                                                                                          "depth REAL," +
                                                                                                          "feedback REAL," +
                                                                                                          "frequency REAL," +
                                                                                                          "phase INT," +
                                                                                                          "wetdrymix REAL,"+
                                                                                                          "waveform INT," +
                                                                                                          "FOREIGN KEY(queueindex) " +
                                                                                                          "REFERENCES effectsqueue(queueindex) " +
                                                                                                          "ON DELETE CASCADE);"
                                                                                                           ,dbConnection);
            SQLiteCommand sqlCreateCompressorTable = new SQLiteCommand("CREATE TABLE if not exists compressoreffect (queueindex INT," +
                                                                                                                  "isactive INT," +
                                                                                                                  "attack REAL," +
                                                                                                                  "gain REAL," +
                                                                                                                  "predelay REAL," +
                                                                                                                  "ratio REAL," +
                                                                                                                  "release REAL," +
                                                                                                                  "treshold REAL," +
                                                                                                                  "FOREIGN KEY(queueindex) " +
                                                                                                                  "REFERENCES effectsqueue(queueindex) " +
                                                                                                                  "ON DELETE CASCADE);"
                                                                                                                  ,dbConnection);
            SQLiteCommand sqlCreateDistortionTable = new SQLiteCommand("CREATE TABLE if not exists distortioneffect (queueindex INT," +
                                                                                                                  "isactive INT," +
                                                                                                                  "edge REAL," +
                                                                                                                  "gain REAL," +
                                                                                                                  "posteqbandwidth REAL," +
                                                                                                                  "posteqcenter REAL," +
                                                                                                                  "prelowpasscutoff INT," +
                                                                                                                  "FOREIGN KEY(queueindex) " +
                                                                                                                  "REFERENCES effectsqueue(queueindex) " +
                                                                                                                  "ON DELETE CASCADE);"
                                                                                                                  ,dbConnection);
            SQLiteCommand sqlCreateEchoTable = new SQLiteCommand("CREATE TABLE if not exists echoeffect (queueindex INT," +
                                                                                                      "isactive INT," +
                                                                                                      "feedback REAL," +
                                                                                                      "leftdelay REAL," +
                                                                                                      "rightdelay REAL," +
                                                                                                      "wetdrymix REAL," +
                                                                                                      "pandelay INT," +
                                                                                                      "FOREIGN KEY(queueindex) " +
                                                                                                      "REFERENCES effectsqueue(queueindex) " +
                                                                                                      "ON DELETE CASCADE);"
                                                                                                      ,dbConnection);
            SQLiteCommand sqlCreateEqualizerTable = new SQLiteCommand("CREATE TABLE if not exists equalizereffect (queueindex INT," +
                                                                                                                "isactive INT," +
                                                                                                                "filter1 REAL," +
                                                                                                                "filter2 REAL," +
                                                                                                                "filter3 REAL," +
                                                                                                                "filter4 REAL," +
                                                                                                                "filter5 REAL," +
                                                                                                                "filter6 REAL," +
                                                                                                                "filter7 REAL," +
                                                                                                                "filter8 REAL," +
                                                                                                                "filter9 REAL," +
                                                                                                                "filter10 REAL," +
                                                                                                                "FOREIGN KEY(queueindex) " +
                                                                                                                "REFERENCES effectsqueue(queueindex) " +
                                                                                                                "ON DELETE CASCADE);"
                                                                                                                ,dbConnection);
            SQLiteCommand sqlCreateGargleTable = new SQLiteCommand("CREATE TABLE if not exists gargleeffect (queueindex INT," +
                                                                                                          "isactive INT," +
                                                                                                          "rate REAL," +
                                                                                                          "waveshape INT," +
                                                                                                          "FOREIGN KEY(queueindex) " +
                                                                                                          "REFERENCES effectsqueue(queueindex) " +
                                                                                                          "ON DELETE CASCADE);"
                                                                                                          ,dbConnection);
            SQLiteCommand sqlCreateReverbTable = new SQLiteCommand("CREATE TABLE if not exists reverbeffect (queueindex INT," +
                                                                                                         "isactive INT," +
                                                                                                         "highfrequencyrtratio REAL," +
                                                                                                         "ingain REAL," +
                                                                                                         "reverbmix REAL," +
                                                                                                         "reverbtime REAL," +
                                                                                                         "FOREIGN KEY(queueindex) " +
                                                                                                         "REFERENCES effectsqueue(queueindex) " +
                                                                                                         "ON DELETE CASCADE);"
                                                                                                         ,dbConnection);
            dbConnection.Open();
            try
            {
                sqlCreateEffectsQueueTable.ExecuteNonQuery();
                sqlCreateChorusTable.ExecuteNonQuery();         
                sqlCreateCompressorTable.ExecuteNonQuery();
                sqlCreateDistortionTable.ExecuteNonQuery();
                sqlCreateEchoTable.ExecuteNonQuery();
                sqlCreateEqualizerTable.ExecuteNonQuery();
                sqlCreateFlangerTable.ExecuteNonQuery();
                sqlCreateGargleTable.ExecuteNonQuery();
                sqlCreateReverbTable.ExecuteNonQuery();
                sqlCreateTrackTable.ExecuteNonQuery();
                sqlCreateFolderTable.ExecuteNonQuery();
                sqlCreatePlaylistTable.ExecuteNonQuery();
                sqlCreateOptionsTable.ExecuteNonQuery();
                sqlCreateTrackPlaylisttable.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
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
