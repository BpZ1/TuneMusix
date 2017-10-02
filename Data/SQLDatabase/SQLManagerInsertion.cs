using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    partial class SQLManager
    {

        /// <summary>
        /// Adds a given Track to the Database.
        /// </summary>
        /// <param name="track"></param>
        private void AddTrack(Track track)
        {
            Logger.Log("Track: '" + track.sourceURL + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO tracks (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "title," +
                                                                                         "interpret," +
                                                                                         "album," +
                                                                                         "releaseyear," +
                                                                                         "comm," +
                                                                                         "genre," +
                                                                                         "rating)" +
                                                                                 "VALUES(@ID," +
                                                                                        "@folderID," +
                                                                                        "@URL," +
                                                                                        "@Title," +
                                                                                        "@Interpret," +
                                                                                        "@Album," +
                                                                                        "@ReleaseYear," +
                                                                                        "@Comm," +
                                                                                        "@Genre," +
                                                                                        "@Rating);",                                                                                   
                                                                                        dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", track.ID);
            if (track.FolderID == 0)
            {
                sqlcommand.Parameters.AddWithValue("folderID", null);
            }
            else
            {
                sqlcommand.Parameters.AddWithValue("folderID", track.FolderID);
            }
            sqlcommand.Parameters.AddWithValue("URL", track.sourceURL);
            sqlcommand.Parameters.AddWithValue("Title", track.Title);
            sqlcommand.Parameters.AddWithValue("Interpret", track.Interpret);
            sqlcommand.Parameters.AddWithValue("Album", track.Album);
            sqlcommand.Parameters.AddWithValue("ReleaseYear", track.Year);
            sqlcommand.Parameters.AddWithValue("Comm", track.Comm);
            sqlcommand.Parameters.AddWithValue("Genre", track.Genre);
            sqlcommand.Parameters.AddWithValue("Rating", track.Rating);

            OpenDBConnection();
            try
            {
                sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void AddAll(List<Track> tracklist)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            
            foreach (Track track in tracklist)
            {
                
                SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO tracks (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "title," +
                                                                                         "interpret," +
                                                                                         "album," +
                                                                                         "releaseyear," +
                                                                                         "comm," +
                                                                                         "genre," +
                                                                                         "rating)" +
                                                                                 "VALUES(@ID," +
                                                                                        "@folderID," +
                                                                                        "@URL," +
                                                                                        "@Title," +
                                                                                        "@Interpret," +
                                                                                        "@Album," +
                                                                                        "@ReleaseYear," +
                                                                                        "@Comm," +
                                                                                        "@Genre," +
                                                                                        "@Rating);",
                                                                                        dbConnection);
                sqlcommand.Parameters.AddWithValue("ID", track.ID);
                if (track.FolderID == 0)
                {
                    sqlcommand.Parameters.AddWithValue("folderID", null);
                }
                else
                {
                    sqlcommand.Parameters.AddWithValue("folderID", track.FolderID);
                }              
                sqlcommand.Parameters.AddWithValue("URL", track.sourceURL);
                sqlcommand.Parameters.AddWithValue("Title", track.Title);
                sqlcommand.Parameters.AddWithValue("Interpret", track.Interpret);
                sqlcommand.Parameters.AddWithValue("Album", track.Album);
                sqlcommand.Parameters.AddWithValue("ReleaseYear", track.Year);
                sqlcommand.Parameters.AddWithValue("Comm", track.Comm);
                sqlcommand.Parameters.AddWithValue("Genre", track.Genre);
                sqlcommand.Parameters.AddWithValue("Rating", track.Rating);
                commandlist.Add(sqlcommand);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
                {
                    command.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Tracks were added to DB");
        }

        public void AddAll(List<Folder> folders)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            foreach (Folder f in folders)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO folders (ID," +
                                                                                         "folderID," +
                                                                                         "URL," +
                                                                                         "name) " +
                                                                                  "VALUES(@ID," +
                                                                                         "@folderID," +
                                                                                         "@URL," +
                                                                                         "@name);",
                                                                                         dbConnection);
                command.Parameters.AddWithValue("ID",f.ID);
                if (f.FolderID == 1)
                {
                    command.Parameters.AddWithValue("folderID",null);
                }
                else
                {
                    command.Parameters.AddWithValue("folderID",f.FolderID);
                }
                command.Parameters.AddWithValue("URL",f.URL);
                command.Parameters.AddWithValue("name",f.Name);
                commandlist.Add(command);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand com in commandlist)
                {
                    com.ExecuteNonQuery();
                }
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Folders were added to DB");
        }
        /// <summary>
        /// Adds a playlist to the database.
        /// </summary>
        /// <param name="playlist"></param>
        public void AddPlaylist(Playlist playlist)
        {
            Logger.Log("Playlist: '" + playlist.Name + "' added to database");
            SQLiteCommand sqlcommand = new SQLiteCommand("INSERT OR REPLACE INTO playlists (ID," +
                                                                                            "name) " +
                                                                                     "VALUES(@ID," +
                                                                                            "@name);",
                                                                                             dbConnection);
            sqlcommand.Parameters.AddWithValue("ID", playlist.ID);
            sqlcommand.Parameters.AddWithValue("name", playlist.Name);
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                sqlcommand.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Playlist was added to DB: " + playlist.Name);
        }
        /// <summary>
        /// Adds a track to a given playlist via the connecting PlaylistTrack table.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="playlist"></param>
        public void AddPlaylistTrack(Track track,Playlist playlist)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO playlisttracks(trackID," +
                                                                                            "playlistID) " +
                                                                                     "VALUES(@trackID,"+
                                                                                            "@playlistID)",
                                                                                             dbConnection);
            command.Parameters.AddWithValue("trackID",track.ID);
            command.Parameters.AddWithValue("playlistID",playlist.ID);

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

        public void AddAllPlaylistTracks(Playlist playlist,List<Track> tracklist)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            foreach (Track track in tracklist)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO playlisttracks(trackID,"+
                                                                                               "playlistID) "+
                                                                                         "VALUES(@trackID,"+
                                                                                                "@playlistID)",
                                                                                                 dbConnection);
                command.Parameters.AddWithValue("trackID",track.ID);
                command.Parameters.AddWithValue("playlistID",playlist.ID);
                commandlist.Add(command);
            }
            OpenDBConnection();
            try
            {
                BeginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
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
        /// <summary>
        /// Clears the options table and updates it with new values.
        /// </summary>
        /// <param name="IDGenStand"></param>
        /// <param name="options"></param>
        public void UpdateOptions(long IDGenStand, Options options)
        {
            SQLiteCommand sqlClearCommand = new SQLiteCommand("DELETE FROM options",dbConnection);
            SQLiteCommand sqlVacuum = new SQLiteCommand("VACUUM",dbConnection);
            SQLiteCommand sqlAddCommand = new SQLiteCommand("INSERT INTO options (IDgen,volume,shuffle,repeatTrack) VALUES (@IDgen,@volume,@shuffle,@repeatTrack);", dbConnection);
            sqlAddCommand.Parameters.AddWithValue("IDgen", IDGenStand);
            sqlAddCommand.Parameters.AddWithValue("volume",options.Volume);
            int shuffle;
            if (options.Shuffle) shuffle = 1;
            else shuffle = 0;
            sqlAddCommand.Parameters.AddWithValue("shuffle",shuffle);
            sqlAddCommand.Parameters.AddWithValue("repeatTrack",options.RepeatTrack);
            OpenDBConnection();
            try
            {
                sqlClearCommand.ExecuteNonQuery();
                sqlVacuum.ExecuteNonQuery();

                BeginCommand.ExecuteNonQuery();             
                sqlAddCommand.ExecuteNonQuery();
                EndCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        /// <summary>
        /// Clears the effect queue table and updates it with new values.
        /// </summary>
        /// <param name="effectQueue"></param>
        public void UpdateEffectQueue(List<BaseEffect> effectQueue)
        {

            SQLiteCommand sqlClearCommand = new SQLiteCommand("DELETE FROM effectsqueue",dbConnection);
            SQLiteCommand sqlVacuum = new SQLiteCommand("VACUUM",dbConnection);
            List<SQLiteCommand> effectInsertCommands = new List<SQLiteCommand>();
            List<SQLiteCommand> effectQueueInsertCommands = new List<SQLiteCommand>();
            foreach (BaseEffect effect in effectQueue)
            {
                //choose which kind of effect has to be inserted.
                int i = 1;
                SQLiteCommand queueCommand = new SQLiteCommand("INSERT INTO effectsqueue (queueindex) VALUES(@Index)", dbConnection);
                queueCommand.Parameters.AddWithValue("Index",i);
                effectQueueInsertCommands.Add(queueCommand);
                #region effects
                if (effect.GetType() == typeof(ChorusEffect))
                {
                    ChorusEffect currentEffect = effect as ChorusEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO choruseffect (queueindex," +
                                                                                        "isactive,"+
                                                                                        "delay,"+
                                                                                        "depth,"+
                                                                                        "frequency,"+
                                                                                        "phase"+
                                                                                        "wetdrymix,"+
                                                                                        "waveform) "+
                                                                                "VALUES(@Index,"+
                                                                                       "@Isactive"+
                                                                                       "@Delay,"+
                                                                                       "@Depth,"+
                                                                                       "@Frequency,"+
                                                                                       "@Phase"+
                                                                                       "@Wetdrymix,"+
                                                                                       "@waveform);"
                                                                                       ,dbConnection);
                    command.Parameters.AddWithValue("Index",i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Delay", currentEffect.Delay);
                    command.Parameters.AddWithValue("Depth", currentEffect.Depth);
                    command.Parameters.AddWithValue("Frequency", currentEffect.Frequency);
                    command.Parameters.AddWithValue("Phase", currentEffect.Phase);
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.Wet_DryMix);
                    command.Parameters.AddWithValue("waveform", currentEffect.WaveForm);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(CompressorEffect))
                {
                    CompressorEffect currentEffect = effect as CompressorEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO compressoreffect (queueindex," +
                                                                                            "isactive,"+
                                                                                            "attack," +
                                                                                            "gain," +
                                                                                            "predelay," +
                                                                                            "ratio" +
                                                                                            "release," +
                                                                                            "treshold) " +
                                                                                    "VALUES(@Index," +
                                                                                           "@Isactive" +
                                                                                           "@Attack," +
                                                                                           "@Gain," +
                                                                                           "@Predelay," +
                                                                                           "@Ratio" +
                                                                                           "@Release," +
                                                                                           "@Treshold);"
                                                                                           ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else  command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Attack", currentEffect.Attack);
                    command.Parameters.AddWithValue("Gain", currentEffect.Gain);
                    command.Parameters.AddWithValue("Predelay", currentEffect.Predelay);
                    command.Parameters.AddWithValue("Ratio", currentEffect.Ratio);
                    command.Parameters.AddWithValue("Release", currentEffect.Release);
                    command.Parameters.AddWithValue("Treshold", currentEffect.Treshold);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(EchoEffect))
                {
                    EchoEffect currentEffect = effect as EchoEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO echoeffect (queueindex," +
                                                                                      "isactive," +
                                                                                      "feedback," +
                                                                                      "leftdelay," +
                                                                                      "rightdelay," +
                                                                                      "wetdrymix" +
                                                                                      "pandelay," +
                                                                              "VALUES(@Index," +
                                                                                     "@Isactive," +
                                                                                     "@Feedback," +
                                                                                     "@Leftdelay," +
                                                                                     "@Rightdelay," +
                                                                                     "@Wetdrymix" +
                                                                                     "@Pandelay);"
                                                                                     ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Feedback", currentEffect.Feedback);
                    command.Parameters.AddWithValue("Leftdelay", currentEffect.LeftDelay);
                    command.Parameters.AddWithValue("Rightdelay", currentEffect.RightDelay);
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.WetDryMix);
                    if (currentEffect.PanDelay) command.Parameters.AddWithValue("Pandelay", 1);
                    else command.Parameters.AddWithValue("Pandelay", 0);                  
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(FlangerEffect))
                {
                    FlangerEffect currentEffect = effect as FlangerEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO flangereffect (queueindex," +
                                                                                         "isactive," +
                                                                                         "delay," +
                                                                                         "depth," +
                                                                                         "feedback," +
                                                                                         "frequency," +
                                                                                         "wetdrymix" +
                                                                                         "waveform)" +
                                                                                 "VALUES(@Index," +
                                                                                        "@Isactive" +
                                                                                        "@Delay," +
                                                                                        "@Depth," +
                                                                                        "@Feedback," +
                                                                                        "@Frequency" +
                                                                                        "@Wetdrymix," +
                                                                                        "@Waveform);"
                                                                                        ,dbConnection);

                   command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Delay", currentEffect.Delay);
                    command.Parameters.AddWithValue("Depth", currentEffect.Depth);
                    command.Parameters.AddWithValue("Feedback", currentEffect.Feedback);
                    command.Parameters.AddWithValue("Frequency", currentEffect.Frequency);
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.Wet_DryMix);
                    command.Parameters.AddWithValue("Waveform", currentEffect.WaveForm);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(DistortionEffect))
                {
                    DistortionEffect currentEffect = effect as DistortionEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO distortioneffect (queueindex," +
                                                                                            "isactive," +
                                                                                            "edge," +
                                                                                            "gain," +
                                                                                            "posteqbandwidth," +
                                                                                            "posteqcenter" +
                                                                                            "prelowpasscutoff)" +
                                                                                    "VALUES(@Index," +
                                                                                           "@Isactive" +
                                                                                           "@Edge," +
                                                                                           "@Gain," +
                                                                                           "@Posteqbandwidth," +
                                                                                           "@Posteqcenter" +
                                                                                           "@Prelowpasscutoff);"
                                                                                           ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Edge", currentEffect.Edge);
                    command.Parameters.AddWithValue("Gain", currentEffect.Gain);
                    command.Parameters.AddWithValue("Posteqbandwidth", currentEffect.PostEQBandwidth);
                    command.Parameters.AddWithValue("Posteqcenter", currentEffect.PostEQCenterFrequency);
                    command.Parameters.AddWithValue("Prelowpasscutoff", currentEffect.PreLowPassCutoff);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(ReverbEffect))
                {
                    ReverbEffect currentEffect = effect as ReverbEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO reverbeffect (queueindex," +
                                                                                        "isactive," +
                                                                                        "highfrequencyrtratio," +
                                                                                        "ingain," +
                                                                                        "reverbmix," +
                                                                                        "reverbtime)" +
                                                                                "VALUES(@Index," +
                                                                                       "@Isactive" +
                                                                                       "@Highfrequencyrtratio," +
                                                                                       "@Ingain," +
                                                                                       "@Reverbmix," +
                                                                                       "@Reverbtime);"
                                                                                       ,dbConnection);

                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Highfrequencyrtratio", currentEffect.HighFrequencyRTRatio);
                    command.Parameters.AddWithValue("Ingain", currentEffect.InGain);
                    command.Parameters.AddWithValue("Reverbmix", currentEffect.ReverbMix);
                    command.Parameters.AddWithValue("Reverbtime", currentEffect.ReverbTime);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(EqualizerEffect))
                {
                    EqualizerEffect currentEffect = effect as EqualizerEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO equalizereffect (queueindex," +
                                                                                           "filter1," +
                                                                                           "filter2," +
                                                                                           "filter3," +
                                                                                           "filter4," +
                                                                                           "filter5," +
                                                                                           "filter6," +
                                                                                           "filter7"  +
                                                                                           "filter8," +
                                                                                           "filter9," +
                                                                                           "filter10)" +
                                                                                   "VALUES(@Index," +
                                                                                          "@Filter1" +
                                                                                          "@Filter2" +
                                                                                          "@Filter3" +
                                                                                          "@Filter4" +
                                                                                          "@Filter5" +
                                                                                          "@Filter6" +
                                                                                          "@Filter7" +
                                                                                          "@Filter8" +
                                                                                          "@Filter9" +
                                                                                          "@Filter10);"
                                                                                          ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Filter1", currentEffect.ChannelFilter0);
                    command.Parameters.AddWithValue("Filter2", currentEffect.ChannelFilter1);
                    command.Parameters.AddWithValue("Filter3", currentEffect.ChannelFilter2);
                    command.Parameters.AddWithValue("Filter4", currentEffect.ChannelFilter3);
                    command.Parameters.AddWithValue("Filter5", currentEffect.ChannelFilter4);
                    command.Parameters.AddWithValue("Filter6", currentEffect.ChannelFilter5);
                    command.Parameters.AddWithValue("Filter7", currentEffect.ChannelFilter6);
                    command.Parameters.AddWithValue("Filter8", currentEffect.ChannelFilter7);
                    command.Parameters.AddWithValue("Filter9", currentEffect.ChannelFilter8);
                    command.Parameters.AddWithValue("Filter10", currentEffect.ChannelFilter9);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(GargleEffect))
                {
                    GargleEffect currentEffect = effect as GargleEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO gargleeffect (queueindex," +
                                                                                        "isactive," +
                                                                                        "rate," +
                                                                                        "waveshape)" +
                                                                                "VALUES(@Index," +
                                                                                       "@Isactive" +
                                                                                       "@Rate," +
                                                                                       "@Waveshape);"
                                                                                       ,dbConnection);

                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Rate", currentEffect.Rate);
                    command.Parameters.AddWithValue("Waveshape", currentEffect.WaveShape);
                    effectInsertCommands.Add(command);
                }
#endregion
                i++;
            }

            OpenDBConnection();
            try
            {            
                sqlClearCommand.ExecuteNonQuery();
                sqlVacuum.ExecuteNonQuery();

                BeginCommand.ExecuteNonQuery();
                foreach(SQLiteCommand command in effectQueueInsertCommands)
                {
                    command.ExecuteNonQuery();
                }
                foreach (SQLiteCommand command in effectInsertCommands)
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

    }
}
