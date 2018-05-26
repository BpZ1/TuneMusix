using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Interface;
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
            LoadingBarManager.Instance.StartLoading("Loading...");

            #region options       
            long idgen = database.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            LoadingBarManager.Instance.Message = "Loading Options...";
            Debug.WriteLine("Options loading...");
            database.LoadOptions();
            Debug.WriteLine("Options loaded!");
            LoadingBarManager.Instance.Progress = 10;
            #endregion

            #region folders
            LoadingBarManager.Instance.Message = "Loading Folders...";
            Debug.WriteLine("Folders loading...");
            List<Folder> FolderList = database.GetFolders();
            List<Folder> RootList = new List<Folder>();
            Debug.WriteLine("Folders loaded");
            LoadingBarManager.Instance.Progress = 20;
            #endregion

            #region tracks
            LoadingBarManager.Instance.Message = "Loading Tracks...";
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
            LoadingBarManager.Instance.Progress = 60;
            #endregion

            #region Playlists
            //load playlists
            LoadingBarManager.Instance.Message = "Loading Playlists...";
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
            LoadingBarManager.Instance.Message = "Loading Playlists...";
            LoadingBarManager.Instance.Progress = 70;
            Debug.WriteLine("Loading effects...");
            List<BaseEffect> effectlist = database.GetEffects();
            dataModel.AddEffectsToQueue_NoDatabase(effectlist);
            Debug.WriteLine(effectlist.Count +  " Effects loaded!");
            LoadingBarManager.Instance.Progress = 80;
            #endregion

            #region creating albums
            LoadingBarManager.Instance.Message = "Loading Albums...";
            dataModel.Albumlist = new ObservableCollection<Album>(createAlbums(tracklist));
            LoadingBarManager.Instance.Progress = 90;
            #endregion

            #region creating interprets
            LoadingBarManager.Instance.Message = "Loading Interprets...";
            dataModel.Interpretlist = new ObservableCollection<Interpret>(createInterprets(tracklist));
            LoadingBarManager.Instance.Progress = 100;
            #endregion

            Debug.WriteLine("Loading finished");
            watch.Stop();
            LoadingBarManager.Instance.Message = "Loading Finished";
            LoadingBarManager.Instance.EndLoading();
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
        /// <summary>
        /// Creates a list of albums from a list of tracks.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <returns></returns>
        private List<Album> createAlbums(List<Track> tracklist)
        {
            Dictionary<string, List<Track>> albumDictionary = new Dictionary<string, List<Track>>();
            //Create an entry for every new string found (case insensitive).
            foreach (Track track in tracklist)
            {
                //If there is already an album with that name add track to it.
                if (albumDictionary.ContainsKey(track.Album.ToLower())) 
                {
                    var list = albumDictionary[track.Album.ToLower()];
                    list.Add(track);
                }
                //else create a new entry.
                else
                {
                    albumDictionary.Add(track.Album.ToLower(),new List<Track>(){track});
                }
            }
            List<Album> albums = new List<Album>();
            foreach (KeyValuePair<string,List<Track>> entry in albumDictionary)
            {
                //Take album name from track (value) because the key is lower case.
                Album album = new Album(entry.Value.First().Album);
                foreach(Track track in entry.Value)
                {
                    album.Add(track);
                    track.albumContainer = album;
                }
                albums.Add(album);
            }
            return albums;
        }
        /// <summary>
        /// Creates a list of interprets from a list of tracks.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <returns></returns>
        private List<Interpret> createInterprets(List<Track> tracklist)
        {
            Dictionary<string, List<Track>> interpretDictionary = new Dictionary<string, List<Track>>();
            //Create an entry for every new string found (case insensitive).
            foreach (Track track in tracklist)
            {
                //If there is already an interpret with that name add track to it.
                if (interpretDictionary.ContainsKey(track.Interpret.ToLower()))
                {
                    var list = interpretDictionary[track.Interpret.ToLower()];
                    list.Add(track);
                }
                //else create a new entry.
                else
                {
                    interpretDictionary.Add(track.Interpret.ToLower(), new List<Track>() { track });
                }
            }
            List<Interpret> interprets = new List<Interpret>();
            foreach (KeyValuePair<string, List<Track>> entry in interpretDictionary)
            {
                //Take interpret name from track (value) because the key is lower case.
                Interpret interpret = new Interpret(entry.Value.First().Interpret);
                foreach (Track track in entry.Value)
                {
                    interpret.Add(track);
                    track.interpretContainer = interpret;
                }
                interprets.Add(interpret);
            }
            return interprets;
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
                        a.Add(b);
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
                        folder.Add(track);
                        track.IsModified = false;
                    }
                }
            }
            return tempFolderList;
        }
     
    }
}
