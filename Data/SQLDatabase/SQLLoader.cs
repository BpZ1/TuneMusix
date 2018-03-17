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
    public class SQLLoader
    {
        private DataModel dataModel = DataModel.Instance;
        private Options options = Options.Instance;
        private SQLManager DBmanager = new SQLManager();
        private IDGenerator IDgen = IDGenerator.Instance;

        /// <summary>
        /// Loads all data from the database and 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadFromDB(object sender, DoWorkEventArgs e)
        {
            var watch = new Stopwatch();
            watch.Start();

            //Load options         
            long idgen = DBmanager.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            Debug.WriteLine("Options loading...");
            DBmanager.LoadOptions();
            Debug.WriteLine("Options loaded!");
          
            //Load folders
            Debug.WriteLine("Folders loading...");
            List<Folder> FolderList = DBmanager.GetFolders();
            List<Folder> RootList = new List<Folder>();
            Debug.WriteLine("Folders loaded");

            //load tracks
            Debug.WriteLine("Loading Tracks...");
            List<Track> tracklist = DBmanager.GetTracks();
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
            dataModel.AddPlaylists_NoDatabase(DBmanager.GetPlaylists());          
            foreach (Playlist playlist in dataModel.Playlists)
            {
                List<PlaylistTrack> playlistTracks = DBmanager.GetPlaylistTracks(playlist);
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
            List<BaseEffect> effectlist = DBmanager.GetEffectQueue();
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
            DBmanager.LoadOptions();
            List<BaseEffect> effectlist = DBmanager.GetEffectQueue();
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
     
        public void CreateDatabase()
        {
            DBmanager.CreateDatabase();
        }
    }
}
