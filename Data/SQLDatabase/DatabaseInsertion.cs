using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    /// <summary>
    /// This class contains methods for inserting data into the database.
    /// </summary>
    public sealed partial class Database : IDatabase
    {
        
        public void Insert(Track track)
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

        public void Insert(List<Track> tracklist)
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
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Tracks were added to DB");
        }
      
        public void Insert(List<Folder> folders)
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
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand com in commandlist)
                {
                    com.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Folders were added to DB");
        }
       
        public void Insert(Playlist playlist)
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
                beginCommand.ExecuteNonQuery();
                sqlcommand.ExecuteNonQuery();
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
            Debug.WriteLine("Playlist was added to DB: " + playlist.Name);
        }
        
        public void Insert(Playlist playlist, List<Track> tracks)
        {
            if (playlist == null || tracks == null)
                throw new ArgumentNullException();

            List<PlaylistTrack> playlistTracks = new List<PlaylistTrack>();
            foreach (Track track in tracks)
            {
                playlistTracks.Add(new PlaylistTrack(track.ID, playlist.ID));
            }
            this.insert(playlistTracks);
        }

        private void insert(List<PlaylistTrack> playlistTracks)
        {
            List<SQLiteCommand> commandlist = new List<SQLiteCommand>();
            foreach (PlaylistTrack track in playlistTracks)
            {
                SQLiteCommand command = new SQLiteCommand("INSERT OR REPLACE INTO playlisttracks(trackID,"+
                                                                                               "playlistID) "+
                                                                                         "VALUES(@trackID,"+
                                                                                                "@playlistID)",
                                                                                                 dbConnection);
                command.Parameters.AddWithValue("trackID",track.TrackID);
                command.Parameters.AddWithValue("playlistID",track.PlaylistID);
                commandlist.Add(command);
            }
            OpenDBConnection();
            try
            {
                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in commandlist)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
        /// <summary>
        /// Updates the options table in the database.
        /// Updated values include:
        /// -stand of idgenerator
        /// -volume
        /// -shuffle
        /// -repeatTrack
        /// -primary color
        /// -accent color
        /// -theme
        /// </summary>
        /// <param name="IDGenStand"></param>
        /// <param name="options"></param>
        public void UpdateOptions(long IDGenStand, Options options)
        {
            SQLiteCommand sqlClearCommand = new SQLiteCommand("DELETE FROM options",dbConnection);
            SQLiteCommand sqlVacuum = new SQLiteCommand("VACUUM",dbConnection);
            //Query
            SQLiteCommand sqlAddCommand = new SQLiteCommand("INSERT INTO options (IDgen,volume,shuffle,repeatTrack,primaryColor,accentColor,theme,askConfirmation)"+
                " VALUES (@IDgen,@volume,@shuffle,@repeatTrack,@primaryColor,@accentColor,@theme,@askConfirmation);", dbConnection);

            //Set values
            sqlAddCommand.Parameters.AddWithValue("IDgen", IDGenStand);
            sqlAddCommand.Parameters.AddWithValue("volume",options.Volume);
            sqlAddCommand.Parameters.AddWithValue("shuffle",Converter.BoolToIntConverter(options.Shuffle));
            sqlAddCommand.Parameters.AddWithValue("repeatTrack",options.RepeatTrack);
            sqlAddCommand.Parameters.AddWithValue("primaryColor", options.PrimaryColorIndex);
            sqlAddCommand.Parameters.AddWithValue("accentColor", options.AccentColorIndex);
            sqlAddCommand.Parameters.AddWithValue("theme", Converter.BoolToIntConverter(options.Theme));
            sqlAddCommand.Parameters.AddWithValue("askConfirmation",Converter.BoolToIntConverter(options.AskConfirmation));

            OpenDBConnection();
            try
            {
                sqlClearCommand.ExecuteNonQuery();
                sqlVacuum.ExecuteNonQuery();

                beginCommand.ExecuteNonQuery();             
                sqlAddCommand.ExecuteNonQuery();
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }
  
        public void UpdateEffectQueue(List<BaseEffect> effectQueue)
        {

            SQLiteCommand sqlClearCommand = new SQLiteCommand("DELETE FROM effectsqueue",dbConnection);
            SQLiteCommand sqlVacuum = new SQLiteCommand("VACUUM",dbConnection);
            List<SQLiteCommand> effectInsertCommands = new List<SQLiteCommand>();
            int i = 1;
            foreach (BaseEffect effect in effectQueue)
            {
                //choose which kind of effect has to be inserted.                 
                #region effects
                if (effect.GetType() == typeof(ChorusEffect))
                {
                    ChorusEffect currentEffect = effect as ChorusEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive,"+
                                                                                        "value0,"+
                                                                                        "value1,"+
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," + 
                                                                                        "value10," +
                                                                                        "value11) "+
                                                                                "VALUES(@Index,"+
                                                                                       "@Effecttype,"+
                                                                                       "@Isactive,"+
                                                                                       "@Delay,"+
                                                                                       "@Depth,"+
                                                                                       "@Feedback," +
                                                                                       "@Frequency," +                                                                                       
                                                                                       "@Wetdrymix,"+
                                                                                       "@Value5," +                                                                         
                                                                                       "@Value6," +
                                                                                       "@Value7," +
                                                                                       "@Value8," +
                                                                                       "@Value9," +
                                                                                       "@Phase," +
                                                                                       "@waveform);"
                                                                                       ,dbConnection);
                    command.Parameters.AddWithValue("Index",i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype","chorus");
                    command.Parameters.AddWithValue("Delay", currentEffect.Delay);
                    command.Parameters.AddWithValue("Depth", currentEffect.Depth);
                    command.Parameters.AddWithValue("Feedback",currentEffect.Feedback);
                    command.Parameters.AddWithValue("Frequency", currentEffect.Frequency);          
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.Wet_DryMix);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    command.Parameters.AddWithValue("Phase", currentEffect.Phase);
                    command.Parameters.AddWithValue("waveform", currentEffect.WaveForm);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(CompressorEffect))
                {
                    CompressorEffect currentEffect = effect as CompressorEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0," +
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                                    "VALUES(@Index," +
                                                                                           "@Effecttype," +
                                                                                           "@Isactive," +
                                                                                           "@Attack," +
                                                                                           "@Gain," +
                                                                                           "@Predelay," +
                                                                                           "@Ratio," +
                                                                                           "@Release," +
                                                                                           "@Treshold," +
                                                                                           "@Value6," +
                                                                                           "@Value7," +
                                                                                           "@Value8," +
                                                                                           "@Value9," +
                                                                                           "@Value10," +
                                                                                           "@Value11);"
                                                                                           ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else  command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "compressor");
                    command.Parameters.AddWithValue("Attack", currentEffect.Attack);
                    command.Parameters.AddWithValue("Gain", currentEffect.Gain);
                    command.Parameters.AddWithValue("Predelay", currentEffect.Predelay);
                    command.Parameters.AddWithValue("Ratio", currentEffect.Ratio);
                    command.Parameters.AddWithValue("Release", currentEffect.Release);
                    command.Parameters.AddWithValue("Treshold", currentEffect.Treshold);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    command.Parameters.AddWithValue("Value10", null);
                    command.Parameters.AddWithValue("Value11", null);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(EchoEffect))
                {
                    EchoEffect currentEffect = effect as EchoEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0," +
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                              "VALUES(@Index," +
                                                                                     "@Effecttype," +
                                                                                     "@Isactive," +
                                                                                     "@Feedback," +
                                                                                     "@Leftdelay," +
                                                                                     "@Rightdelay," +
                                                                                     "@Wetdrymix," +
                                                                                     "@Value4," +
                                                                                     "@Value5," +
                                                                                     "@Value6," +
                                                                                     "@Value7," +
                                                                                     "@Value8," +
                                                                                     "@Value9," +
                                                                                     "@Pandelay,"+
                                                                                     "@Value11);"
                                                                                     ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "echo");
                    command.Parameters.AddWithValue("Feedback", currentEffect.Feedback);
                    command.Parameters.AddWithValue("Leftdelay", currentEffect.LeftDelay);
                    command.Parameters.AddWithValue("Rightdelay", currentEffect.RightDelay);
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.WetDryMix);
                    command.Parameters.AddWithValue("Value4", null);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    if (currentEffect.PanDelay) command.Parameters.AddWithValue("Pandelay", 1);
                    else command.Parameters.AddWithValue("Pandelay", 0);
                    command.Parameters.AddWithValue("Value11", null);                         
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(FlangerEffect))
                {
                    FlangerEffect currentEffect = effect as FlangerEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0," +
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                                 "VALUES(@Index," +
                                                                                        "@Effecttype," +
                                                                                        "@Isactive," +
                                                                                        "@Delay," +
                                                                                        "@Depth," +
                                                                                        "@Feedback," +
                                                                                        "@Frequency," +
                                                                                        "@Wetdrymix," +
                                                                                        "@Value5," +
                                                                                        "@Value6," +
                                                                                        "@Value7," +
                                                                                        "@Value8," +
                                                                                        "@Value9," +
                                                                                        "@Waveform,"+
                                                                                        "@Value11);"
                                                                                        ,dbConnection);

                   command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "flanger");
                    command.Parameters.AddWithValue("Delay", currentEffect.Delay);
                    command.Parameters.AddWithValue("Depth", currentEffect.Depth);
                    command.Parameters.AddWithValue("Feedback", currentEffect.Feedback);
                    command.Parameters.AddWithValue("Frequency", currentEffect.Frequency);
                    command.Parameters.AddWithValue("Wetdrymix", currentEffect.Wet_DryMix);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    command.Parameters.AddWithValue("Waveform", currentEffect.WaveForm);
                    command.Parameters.AddWithValue("Value11", null);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(DistortionEffect))
                {
                    DistortionEffect currentEffect = effect as DistortionEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0," +
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                                    "VALUES(@Index," +
                                                                                           "@Effecttype," +
                                                                                           "@Isactive," +
                                                                                           "@Edge," +
                                                                                           "@Gain," +
                                                                                           "@Posteqbandwidth," +
                                                                                           "@Posteqcenter," +
                                                                                           "@Prelowpasscutoff,"+
                                                                                           "@Value5," +
                                                                                           "@Value6," +
                                                                                           "@Value7," +
                                                                                           "@Value8," +
                                                                                           "@Value9," +
                                                                                           "@Value10," +
                                                                                           "@Value11);"
                                                                                           , dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "distortion");
                    command.Parameters.AddWithValue("Edge", currentEffect.Edge);
                    command.Parameters.AddWithValue("Gain", currentEffect.Gain);
                    command.Parameters.AddWithValue("Posteqbandwidth", currentEffect.PostEQBandwidth);
                    command.Parameters.AddWithValue("Posteqcenter", currentEffect.PostEQCenterFrequency);
                    command.Parameters.AddWithValue("Prelowpasscutoff", currentEffect.PreLowPassCutoff);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    command.Parameters.AddWithValue("Value10", null);
                    command.Parameters.AddWithValue("Value11", null);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(ReverbEffect))
                {
                    ReverbEffect currentEffect = effect as ReverbEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0," +
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                                "VALUES(@Index," +
                                                                                       "@Effecttype," +
                                                                                       "@Isactive," +
                                                                                       "@Highfrequencyrtratio," +
                                                                                       "@Ingain," +
                                                                                       "@Reverbmix," +
                                                                                       "@Reverbtime," +
                                                                                       "@Value4," +
                                                                                       "@Value5," +
                                                                                       "@Value6," +
                                                                                       "@Value7," +
                                                                                       "@Value8," +
                                                                                       "@Value9," +
                                                                                       "@Value10," +
                                                                                       "@Value11);"
                                                                                       ,dbConnection);

                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "reverb");
                    command.Parameters.AddWithValue("Highfrequencyrtratio", currentEffect.HighFrequencyRTRatio);
                    command.Parameters.AddWithValue("Ingain", currentEffect.InGain);
                    command.Parameters.AddWithValue("Reverbmix", currentEffect.ReverbMix);
                    command.Parameters.AddWithValue("Reverbtime", currentEffect.ReverbTime);
                    command.Parameters.AddWithValue("Value4", null);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
                    command.Parameters.AddWithValue("Value10", null);
                    command.Parameters.AddWithValue("Value11", null);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(EqualizerEffect))
                {
                    EqualizerEffect currentEffect = effect as EqualizerEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                           "effecttype," +
                                                                                           "isactive," +
                                                                                           "value0," +
                                                                                           "value1," +
                                                                                           "value2," +
                                                                                           "value3," +
                                                                                           "value4," +
                                                                                           "value5," +
                                                                                           "value6," +
                                                                                           "value7," +
                                                                                           "value8," +
                                                                                           "value9," +
                                                                                           "value10," +
                                                                                           "value11) " +
                                                                                   "VALUES(@Index," +
                                                                                          "@Effecttype," +
                                                                                          "@Isactive," +
                                                                                          "@Filter1," +
                                                                                          "@Filter2," +
                                                                                          "@Filter3," +
                                                                                          "@Filter4," +
                                                                                          "@Filter5," +
                                                                                          "@Filter6," +
                                                                                          "@Filter7," +
                                                                                          "@Filter8," +
                                                                                          "@Filter9," +
                                                                                          "@Filter10," +
                                                                                          "@Value10," +
                                                                                          "@Value11);"
                                                                                          ,dbConnection);
                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "equalizer");
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
                    command.Parameters.AddWithValue("Value10",null);
                    command.Parameters.AddWithValue("Value11",null);
                    effectInsertCommands.Add(command);
                }
                if (effect.GetType() == typeof(GargleEffect))
                {
                    GargleEffect currentEffect = effect as GargleEffect;
                    SQLiteCommand command = new SQLiteCommand("INSERT INTO effectsqueue (queueindex," +
                                                                                        "effecttype," +
                                                                                        "isactive," +
                                                                                        "value0,"+
                                                                                        "value1," +
                                                                                        "value2," +
                                                                                        "value3," +
                                                                                        "value4," +
                                                                                        "value5," +
                                                                                        "value6," +
                                                                                        "value7," +
                                                                                        "value8," +
                                                                                        "value9," +
                                                                                        "value10," +
                                                                                        "value11) " +
                                                                                "VALUES(@Index," +                                                                                     
                                                                                       "@Effecttype," +
                                                                                       "@Isactive," +
                                                                                       "@Value0," +
                                                                                       "@Value1," +
                                                                                       "@Value2," +
                                                                                       "@Value3," +
                                                                                       "@Value4," +
                                                                                       "@Value5," +
                                                                                       "@Value6," +
                                                                                       "@Value7," +
                                                                                       "@Value8," +
                                                                                       "@Value9," +
                                                                                       "@Rate," +
                                                                                       "@Waveshape);"
                                                                                       ,dbConnection);

                    command.Parameters.AddWithValue("Index", i);
                    if (currentEffect.IsActive) command.Parameters.AddWithValue("Isactive", 1);
                    else command.Parameters.AddWithValue("Isactive", 0);
                    command.Parameters.AddWithValue("Effecttype", "gargle");
                    command.Parameters.AddWithValue("Value0",null);
                    command.Parameters.AddWithValue("Value1", null);
                    command.Parameters.AddWithValue("Value2", null);
                    command.Parameters.AddWithValue("Value3", null);
                    command.Parameters.AddWithValue("Value4", null);
                    command.Parameters.AddWithValue("Value5", null);
                    command.Parameters.AddWithValue("Value6", null);
                    command.Parameters.AddWithValue("Value7", null);
                    command.Parameters.AddWithValue("Value8", null);
                    command.Parameters.AddWithValue("Value9", null);
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

                beginCommand.ExecuteNonQuery();
                foreach (SQLiteCommand command in effectInsertCommands)
                {
                    command.ExecuteNonQuery();
                }
                endCommand.ExecuteNonQuery();
            }
            finally
            {
                CloseDBConnection();
            }
        }

 
    }
}
