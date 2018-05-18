using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    /// <summary>
    /// This class contains methods for loading all data from the database on the 
    /// start of the program.
    /// </summary>
    public class SQLLoader
    {
        private DataModel dataModel = DataModel.Instance;
        private Database database = Database.Instance;
        private IDGenerator IDgen = IDGenerator.Instance;

        /// <summary>
        /// Loads all data from the database and 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadFromDB()
        {
            var watch = new Stopwatch();
            watch.Start();

            //Load options         
            long idgen = database.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            Debug.WriteLine("Options loading...");
            database.LoadOptions();
            Debug.WriteLine("Options loaded!");
          
            //Load folders
            Debug.WriteLine("Folders loading...");
            List<Folder> FolderList = database.GetFolders();
            List<Folder> RootList = new List<Folder>();
            Debug.WriteLine("Folders loaded");

            //load tracks
            Debug.WriteLine("Loading Tracks...");
            List<Track> tracklist = database.GetTracks();
            dataModel.AddTracks_NoDatabase(tracklist, false);
            Debug.WriteLine("Tracks loaded!");
            FolderSort(FolderList);
            TrackSort(FolderList);
            foreach (Folder folder in FolderList)
            {
                if (folder.FolderID == 1)
                {
                    RootList.Add(folder);
                }
            }
            dataModel.AddRootFolders_NoDatabase(RootList);

            //load playlists
            Debug.WriteLine("Loading Playlists...");
            dataModel.AddPlaylists_NoDatabase(database.GetPlaylists());          
            foreach (Playlist playlist in dataModel.Playlists)
            {
                List<PlaylistTrack> playlistTracks = database.GetPlaylistTracks(playlist);
                foreach (PlaylistTrack pt in playlistTracks)
                {
                    foreach (Track track in dataModel.TrackList)
                    {
                        if (track.ID == pt.TrackID)
                        {
                            dataModel.AddTrackToPlaylist_NoDatabase(track,playlist);
                        }
                    }
                }
            }
            Debug.WriteLine("Playlists loaded!");
            Debug.WriteLine("Loading effects...");
            List<BaseEffect> effectlist = database.GetEffects();
            dataModel.AddEffectsToQueue_NoDatabase(effectlist);

            Debug.WriteLine(effectlist.Count +  " Effects loaded!");
            Debug.WriteLine("Loading finished");

            watch.Stop();
            Debug.WriteLine("Time: "+ watch.ElapsedMilliseconds);

        }

        /// <summary>
        /// Loads options and EffectQueue from the database.
        /// </summary>
        public void LoadOptions()
        {
            database.LoadOptions();
            List<BaseEffect> effectlist = database.GetEffects();
            dataModel.EffectQueue.Clear();
            dataModel.AddEffectsToQueue_NoDatabase(effectlist);
        }

        private List<Folder> FolderSort(List<Folder> FolderList)
        {
            List<Folder> templist = FolderList;
            foreach (Folder a in templist)
            {
                a.IsModified = false;
                foreach (Folder b in templist)
                {
                    if (a.ID == b.FolderID)
                    {
                        a.InsertFolder(b);
                    }
                }
            }
            return templist;
        }
        private List<Folder> TrackSort(List<Folder> FolderList)
        {
            List<Folder> tempFolderList = FolderList;
            foreach (Track track in dataModel.TrackList)
            {
                foreach (Folder folder in FolderList)
                {
                    if (track.FolderID == folder.ID)
                    {
                        folder.AddTrack(track);
                        track.IsModified = false;
                    }
                }
            }
            return tempFolderList;
        }
     
    }
}
