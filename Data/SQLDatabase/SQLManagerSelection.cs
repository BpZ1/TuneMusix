using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    partial class SQLManager
    {

        /// <summary>
        /// Returns all Tracks contained in the Database.
        /// </summary>
        /// <returns></returns>
        public List<Track> GetTracks()
        {
            List<Track> tracklist = new List<Track>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM tracks", dbConnection);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (dbReader.Read())
                {
                    long folderID;
                    if (dbReader.IsDBNull(1))
                    {
                        folderID = 0;
                    }
                    else
                    {
                        folderID = dbReader.GetInt64(1);
                    }
                    Track track = new Track(dbReader.GetString(2), dbReader.GetInt32(0), folderID);
                    track.Title = dbReader.GetString(3);
                    track.Interpret = dbReader.GetString(4);
                    track.Album = dbReader.GetString(5);
                    track.Year = dbReader.GetInt32(6);
                    track.Comm = dbReader.GetString(7);
                    track.Genre = dbReader.GetString(8);
                    track.Rating = dbReader.GetInt32(9);
                    tracklist.Add(track);
                }
            }
            finally
            {
                dbReader.Close();
                CloseDBConnection();
            }
            return tracklist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Folder> GetFolders()
        {
            List<Folder> folderlist = new List<Folder>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM folders", dbConnection);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                // Always call Read before accessing data.
                while (dbReader.Read())
                {
                    string name = dbReader.GetString(3);
                    long id = dbReader.GetInt32(0);
                    long folderid;
                    if (dbReader.IsDBNull(1))
                    {
                        folderid = 1;
                    }
                    else
                    {
                        folderid = dbReader.GetInt64(1);
                    }
                    string url = dbReader.GetString(2);
                    Folder folder = new Folder(name,url,id,folderid);
                    folderlist.Add(folder);
                }
            }
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
            return folderlist;
        }

        public void GetOptions()
        {
            Options options = Options.Instance;

            SQLiteCommand command = new SQLiteCommand("SELECT volume,shuffle,repeatTrack FROM options;",dbConnection);


            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                int i = 0;
                while (dbReader.Read())
                {
                    i++;
                    if (!dbReader.IsDBNull(0))
                    {
                        options.Volume = dbReader.GetInt32(0);
                    }
                    else
                    {
                        options.Volume = 50;
                    }
                    if (!dbReader.IsDBNull(1))
                    {
                        if (dbReader.GetInt32(1) == 1)
                        {
                            options.Shuffle = true;
                        }
                        else
                        {
                            options.Shuffle = false;
                        }
                    }
                    else
                    {
                        options.Shuffle = false;
                    }
                    if (!dbReader.IsDBNull(2))
                    {
                        options.RepeatTrack = dbReader.GetInt32(2);
                    }
                    else
                    {
                        options.RepeatTrack = 0;
                    }
                }
                if (i == 0)
                {
                    options.Volume = 50;
                    options.Shuffle = false;
                    options.RepeatTrack = 0;
                }

            }                    
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetIDCounterStand()
        {
            long IDCounter = 0;
            SQLiteCommand command = new SQLiteCommand("SELECT IDgen FROM options;", dbConnection);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                while (dbReader.Read())
                {
                    if (dbReader.IsDBNull(0))
                    {
                        IDCounter = 0;
                    }
                    else
                    {
                        IDCounter = dbReader.GetInt64(0);
                    }
                }
            }
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
            return IDCounter;
        }
    }
}
