using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Interface;
using TuneMusix.Helpers.MediaPlayer;
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
        private Database database = Database.Instance;
        private IDGenerator IDgen = IDGenerator.Instance;

        /// <summary>
        /// Loads all data from the database and 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public DataModel LoadFromDB()
        {
            //Starting the timer
            var watch = new Stopwatch();
            watch.Start();

            Debug.WriteLine("Loading started");

            //Starting the loading bar
            LoadingBarManager loadingBar = LoadingBarManager.Instance;
            loadingBar.StartLoading("Loading data...");

            #region loading data         
            long idgen = database.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            database.LoadOptions();

            loadingBar.Progress = 5;
            List<Folder> FolderList = database.GetFolders();
            loadingBar.Progress = 10;
            List<Track> tracklist = database.GetTracks();
            loadingBar.Progress = 20;
            List<Playlist> playlists = database.GetPlaylists();
            loadingBar.Progress = 25;

            #endregion


            #region sorting
            folderSort(FolderList);
            loadingBar.Progress = 50;
            trackSort(FolderList, tracklist);
            loadingBar.Progress = 80;
            List<Folder> rootList = new List<Folder>();

            foreach (Folder folder in FolderList)
            {
                if (folder.FolderID == 1)
                    rootList.Add(folder);
            }
            playlistSort(playlists, tracklist);
            loadingBar.Progress = 90;
            List<Album> albumList = createAlbums(tracklist);
            List<Interpret> interpretList = createInterprets(tracklist);
            loadingBar.Progress = 100;
            #endregion

            Debug.WriteLine("Loading finished");
            watch.Stop();
            loadingBar.Message = "Loading Finished";
            loadingBar.EndLoading();
            Debug.WriteLine("Time: "+ watch.ElapsedMilliseconds);

            DataModel model = new DataModel(tracklist, playlists, rootList, albumList, interpretList);
            return model;
        }

        /// <summary>
        /// Loads options and EffectQueue from the database.
        /// </summary>
        public void LoadOptions()
        {
            AudioControls audio = AudioControls.Instance;
            database.LoadOptions();
            List<BaseEffect> effectlist = database.GetEffects();
            audio.EffectQueue.Clear();
            foreach(BaseEffect effect in effectlist)
            {
                audio.EffectQueue.Add(effect);
            }
            audio.EffectQueue.IsModified = false;
            Debug.WriteLine("Options loaded");
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

        /// <summary>
        /// Takes a list of folders and returns the rootfolder(s).
        /// </summary>
        /// <param name="folderlist">List of folders.</param>
        /// <returns>Rootfolder(s) of the given Folder</returns>
        private void folderSort(List<Folder> folderlist)
        {
            foreach (Folder a in folderlist)
            {              
                foreach (Folder b in folderlist)
                {
                    if (a.ID == b.FolderID)
                    {
                        a.Add(b);
                        b.IsModified = false;
                    }
                }
                a.IsModified = false;
            }
        }
        /// <summary>
        /// Takes a list of folders and a list of tracks and 
        /// sets the folder for each track and sets the tracks for each folder.
        /// </summary>
        /// <param name="FolderList">List of folders.</param>
        /// <param name="tracklist">List of tracks.</param>
        /// <returns>List of sorted folders</returns>
        private void trackSort(List<Folder> FolderList, List<Track> tracklist)
        {
            List<Folder> tempFolderList = FolderList;
            foreach (Track track in tracklist)
            {
                foreach (Folder folder in FolderList)
                {
                    if (track.FolderID == folder.ID)
                    {
                        folder.Add(track);
                        track.IsModified = false;
                    }
                    folder.IsModified = false;
                }
            }
        }
     
        /// <summary>
        /// Matches each playlist with its corresponding tracks.
        /// </summary>
        /// <param name="playlists">List of playlists.</param>
        /// <param name="tracklist">List of all tracks.</param>
        private void playlistSort(List<Playlist> playlists, List<Track> tracklist)
        {
            List<Playlist> sortedList = new List<Playlist>();
            foreach(Playlist playlist in playlists)
            {
                List<PlaylistTrack> playlistTracks = database.GetPlaylistTracks(playlist);
                foreach(PlaylistTrack playlistTrack in playlistTracks)
                {
                    foreach(Track track in tracklist)
                    {
                        if(track.ID == playlistTrack.TrackID)
                        {
                            playlist.Add(track);
                        }
                    }
                }
                playlist.IsModified = false;
            }
        }
    }
}
