using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data
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

        public Options GetOptions()
        {
            Options options = Options.Instance;

            //Implement

            return options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetIDCounterStand()
        {
            long IDCounter = 0;
            SQLiteCommand command = new SQLiteCommand("SELECT IDgen FROM options", dbConnection);
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
