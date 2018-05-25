using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    public sealed partial class Database : IDatabase
    {

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
                    string url = dbReader.GetString(2);
                    long id = dbReader.GetInt32(0);
                    long folderId = folderID;
                    string title = dbReader.GetString(3);
                    string interpret = dbReader.GetString(4);
                    string album = dbReader.GetString(5);
                    int year = dbReader.GetInt32(6);
                    string comm = dbReader.GetString(7);
                    string genre = dbReader.GetString(8);
                    int rating = dbReader.GetInt32(9);
                    string duration = dbReader.GetString(10);
                    Track track = new Track(url, id, folderId, title, interpret, album,year, comm, genre, rating, duration);
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

        public List<Folder> GetFolders()
        {
            List<Folder> folderlist = new List<Folder>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM folders", dbConnection);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
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

        public void LoadOptions()
        {
            Options options = Options.Instance;

            SQLiteCommand command = new SQLiteCommand("SELECT volume,shuffle,repeatTrack,primaryColor,accentColor,theme,askConfirmation FROM options;", dbConnection);

            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                int volume = 50;
                bool shuffle = false;
                int repeatTrack = 0;
                int primaryColor = 16; //Default = BlueGrey
                int accentColor = 4; //Default = Teal
                bool theme = false; //Default = Light
                bool askConfirmation = true;

                while (dbReader.Read())
                {
                    if (!dbReader.IsDBNull(0))
                        volume = dbReader.GetInt32(0);

                    if (!dbReader.IsDBNull(1))
                        shuffle = Converter.IntToBoolConverter(dbReader.GetInt32(1));                      

                    if (!dbReader.IsDBNull(2))
                        repeatTrack = dbReader.GetInt32(2);
                    
                    if (!dbReader.IsDBNull(3))       
                        primaryColor = dbReader.GetInt32(3);

                    if (!dbReader.IsDBNull(4))
                        accentColor = dbReader.GetInt32(4);

                    if (!dbReader.IsDBNull(5))
                        theme = Converter.IntToBoolConverter(dbReader.GetInt32(5));
                 
                    if (!dbReader.IsDBNull(6))
                        askConfirmation = Converter.IntToBoolConverter(dbReader.GetInt32(6));

                }
                Options.Instance.SetOptions(volume, shuffle, repeatTrack, primaryColor, accentColor, theme, askConfirmation);

            }                    
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
        }

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

        public List<Playlist> GetPlaylists()
        {
            List<Playlist> playlistList = new List<Playlist>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM playlists;",dbConnection);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                while (dbReader.Read())
                {
                    Playlist playlist = new Playlist(dbReader.GetString(1), dbReader.GetInt32(0));
                    playlistList.Add(playlist);
                }
            }
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
            return playlistList;
        }

        public List<PlaylistTrack> GetPlaylistTracks(Playlist playlist)
        {
            List<PlaylistTrack> playlistTrackList = new List<PlaylistTrack>();
            SQLiteCommand command = new SQLiteCommand("SELECT * "+
                                                      "FROM playlisttracks "+
                                                      "WHERE playlistID=@ID;"
                                                      ,dbConnection);
            command.Parameters.AddWithValue("ID",playlist.ID);
            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                while (dbReader.Read())
                {
                    PlaylistTrack playlistTrack = new PlaylistTrack(dbReader.GetInt32(0), dbReader.GetInt32(1));
                    playlistTrackList.Add(playlistTrack);
                }
            }
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
            return playlistTrackList;
        }

        public List<BaseEffect> GetEffects()
        {
            List<BaseEffect> effectQueue = new List<BaseEffect>();          
            SQLiteCommand command = new SQLiteCommand("SELECT * " +
                                                    "FROM effectsqueue;"
                                                    ,dbConnection);

            OpenDBConnection();
            dbReader = command.ExecuteReader();
            try
            {
                while (dbReader.Read())
                {
                    string type = dbReader.GetString(1);
                    bool isActive;
                    float value0 = 0;
                    float value1 = 0;
                    float value2 = 0;
                    float value3 = 0;
                    float value4 = 0;
                    float value5 = 0;
                    float value6 = 0;
                    float value7 = 0;
                    float value8 = 0;
                    float value9 = 0;
                    int value10 = 0;
                    int value11 = 0;
                    if (dbReader.GetInt32(2) == 1) isActive = true;
                    else isActive = false;
                    if (!dbReader.IsDBNull(3))
                    {
                        value0 = dbReader.GetFloat(3);
                    }
                    if (!dbReader.IsDBNull(4))
                    {
                        value1 = dbReader.GetFloat(4);
                    }
                    if (!dbReader.IsDBNull(5))
                    {
                        value2 = dbReader.GetFloat(5);
                    }
                    if (!dbReader.IsDBNull(6))
                    {
                        value3 = dbReader.GetFloat(6);
                    }
                    if (!dbReader.IsDBNull(7))
                    {
                        value4 = dbReader.GetFloat(7);
                    }
                    if (!dbReader.IsDBNull(8))
                    {
                        value5 = dbReader.GetFloat(8);
                    }
                    if (!dbReader.IsDBNull(9))
                    {
                        value6 = dbReader.GetFloat(9);
                    }
                    if (!dbReader.IsDBNull(10))
                    {
                        value7 = dbReader.GetFloat(10);
                    }
                    if (!dbReader.IsDBNull(11))
                    {
                        value8 = dbReader.GetFloat(11);
                    }
                    if (!dbReader.IsDBNull(12))
                    {
                        value9 = dbReader.GetFloat(12);
                    }
                    if (!dbReader.IsDBNull(13))
                    {
                        value10 = dbReader.GetInt32(13);
                    }
                    if (!dbReader.IsDBNull(14))
                    {
                        value11 = dbReader.GetInt32(14);
                    }

                    if (type.Equals("distortion"))
                    {
                        DistortionEffect effect = new DistortionEffect();
                        effect.IsActive = isActive;
                        effect.Edge = value0;
                        effect.Gain = value1;
                        effect.PostEQBandwidth = value2;
                        effect.PostEQCenterFrequency = value3;
                        effect.PreLowPassCutoff = value4;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("echo"))
                    {
                        EchoEffect effect = new EchoEffect();
                        effect.IsActive = isActive;
                        effect.Feedback = value0;
                        effect.LeftDelay = value1;
                        effect.RightDelay = value2;
                        effect.WetDryMix = value3;
                        if (value10 == 1) effect.PanDelay = true;
                        else effect.PanDelay = false;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("reverb"))
                    {
                        ReverbEffect effect = new ReverbEffect();
                        effect.IsActive = isActive;
                        effect.HighFrequencyRTRatio = value0;
                        effect.InGain = value1;
                        effect.ReverbMix = value2;
                        effect.ReverbTime = value3;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("equalizer"))
                    {
                        EqualizerEffect effect = new EqualizerEffect();
                        effect.IsActive = isActive;
                        effect.ChannelFilter0 = value0;
                        effect.ChannelFilter1 = value1;
                        effect.ChannelFilter2 = value2;
                        effect.ChannelFilter3 = value3;
                        effect.ChannelFilter4 = value4;
                        effect.ChannelFilter5 = value5;
                        effect.ChannelFilter6 = value6;
                        effect.ChannelFilter7 = value7;
                        effect.ChannelFilter8 = value8;
                        effect.ChannelFilter9 = value9;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("gargle"))
                    {
                        GargleEffect effect = new GargleEffect();
                        effect.IsActive = isActive;
                        effect.Rate = value10;
                        effect.WaveShape = value11;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("compressor"))
                    {
                        CompressorEffect effect = new CompressorEffect();
                        effect.IsActive = isActive;
                        effect.Attack = value0;
                        effect.Gain = value1;
                        effect.Predelay = value2;
                        effect.Ratio = value3;
                        effect.Release = value4;
                        effect.Treshold = value5;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("flanger"))
                    {
                        FlangerEffect effect = new FlangerEffect();
                        effect.IsActive = isActive;
                        effect.Delay = value0;
                        effect.Depth = value1;
                        effect.Feedback = value2;
                        effect.Frequency = value3;
                        effect.Wet_DryMix = value4;
                        effect.WaveForm = value10;
                        effectQueue.Add(effect);
                    }
                    if (type.Equals("chorus"))
                    {
                        ChorusEffect effect = new ChorusEffect();
                        effect.IsActive = isActive;                    
                        effect.Delay = value0;
                        effect.Depth = value1;
                        effect.Feedback = value2;
                        effect.Frequency = value3;
                        effect.Phase = value10;
                        effect.WaveForm = value11;
                        effectQueue.Add(effect);
                    }

                }
            }
            finally
            {
                CloseDBConnection();
                dbConnection.Close();
            }
            return effectQueue;
        }
    }
}
