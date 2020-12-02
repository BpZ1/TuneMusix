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
        private DatabaseLegacy _database = DatabaseLegacy.Instance;
        private IDGenerator _idGen = IDGenerator.Instance;

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

            Debug.WriteLine( "Loading started" );

            //Starting the loading bar
            LoadingBarManager loadingBar = LoadingBarManager.Instance;
            loadingBar.StartLoading( "Loading data..." );

            #region loading data         
            long idgen = _database.GetIDCounterStand();
            if ( idgen == 0 || idgen == 1 )
            {
                idgen = 2;
            }
            _idGen.Initialize( idgen );
            _database.LoadOptions();

            loadingBar.Progress = 5;
            List<Folder> FolderList = _database.GetFolders();
            loadingBar.Progress = 10;
            List<Track> tracklist = _database.GetTracks();
            loadingBar.Progress = 20;
            List<Playlist> playlists = _database.GetPlaylists();
            loadingBar.Progress = 25;

            #endregion


            #region sorting
            FolderSort( FolderList );
            loadingBar.Progress = 50;
            TrackSort( FolderList, tracklist );
            loadingBar.Progress = 80;
            List<Folder> rootList = new List<Folder>();

            foreach ( Folder folder in FolderList )
            {
                if ( folder.FolderId == "" )
                    rootList.Add( folder );
            }
            PlaylistSort( playlists, tracklist );
            loadingBar.Progress = 90;
            List<Album> albumList = CreateAlbums( tracklist );
            List<Interpret> interpretList = CreateInterprets( tracklist );
            loadingBar.Progress = 100;
            #endregion

            Debug.WriteLine( "Loading finished" );
            watch.Stop();
            loadingBar.Message = "Loading Finished";
            loadingBar.EndLoading();
            Debug.WriteLine( "Time: " + watch.ElapsedMilliseconds );

            DataModel model = new DataModel( tracklist, playlists, rootList, albumList, interpretList );
            return model;
        }

        /// <summary>
        /// Loads options and EffectQueue from the database.
        /// </summary>
        public void LoadOptions()
        {
            AudioControls audio = AudioControls.Instance;
            _database.LoadOptions();
            List<BaseEffect> effectlist = _database.GetEffects();
            audio.EffectQueue.Clear();
            foreach ( BaseEffect effect in effectlist )
            {
                audio.EffectQueue.Add( effect );
            }
            audio.EffectQueue.IsModified = false;
            Debug.WriteLine( "Options loaded" );
        }
        /// <summary>
        /// Creates a list of albums from a list of tracks.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <returns></returns>
        private List<Album> CreateAlbums( List<Track> tracklist )
        {
            Dictionary<string, List<Track>> albumDictionary = new Dictionary<string, List<Track>>();
            //Create an entry for every new string found (case insensitive).
            foreach ( Track track in tracklist )
            {
                //If there is already an album with that name add track to it.
                if ( albumDictionary.ContainsKey( track.Album.Value.ToLower() ) )
                {
                    var list = albumDictionary[track.Album.Value.ToLower()];
                    list.Add( track );
                }
                //else create a new entry.
                else
                {
                    albumDictionary.Add( track.Album.Value.ToLower(), new List<Track>() { track } );
                }
            }
            List<Album> albums = new List<Album>();
            foreach ( KeyValuePair<string, List<Track>> entry in albumDictionary )
            {
                //Take album name from track (value) because the key is lower case.
                Album album = new Album( entry.Value.First().Album.Value );
                foreach ( Track track in entry.Value )
                {
                    album.Add( track );
                    track.AlbumContainer = album;
                }
                albums.Add( album );
            }
            return albums;
        }
        /// <summary>
        /// Creates a list of interprets from a list of tracks.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <returns></returns>
        private List<Interpret> CreateInterprets( List<Track> tracklist )
        {
            Dictionary<string, List<Track>> interpretDictionary = new Dictionary<string, List<Track>>();
            //Create an entry for every new string found (case insensitive).
            foreach ( Track track in tracklist )
            {
                //If there is already an interpret with that name add track to it.
                if ( interpretDictionary.ContainsKey( track.Interpret.Value.ToLower() ) )
                {
                    var list = interpretDictionary[track.Interpret.Value.ToLower()];
                    list.Add( track );
                }
                //else create a new entry.
                else
                {
                    interpretDictionary.Add( track.Interpret.Value.ToLower(), new List<Track>() { track } );
                }
            }
            List<Interpret> interprets = new List<Interpret>();
            foreach ( KeyValuePair<string, List<Track>> entry in interpretDictionary )
            {
                //Take interpret name from track (value) because the key is lower case.
                Interpret interpret = new Interpret( entry.Value.First().Interpret.Value );
                foreach ( Track track in entry.Value )
                {
                    interpret.Add( track );
                    track.InterpretContainer = interpret;
                }
                interprets.Add( interpret );
            }
            return interprets;
        }

        /// <summary>
        /// Takes a list of folders and returns the rootfolder(s).
        /// </summary>
        /// <param name="folderlist">List of folders.</param>
        /// <returns>Rootfolder(s) of the given Folder</returns>
        private void FolderSort( List<Folder> folderlist )
        {
            foreach ( Folder a in folderlist )
            {
                foreach ( Folder b in folderlist )
                {
                    if ( a.Id == b.FolderId )
                    {
                        a.Add( b );
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
        private void TrackSort( List<Folder> FolderList, List<Track> tracklist )
        {
            List<Folder> tempFolderList = FolderList;
            foreach ( Track track in tracklist )
            {
                foreach ( Folder folder in FolderList )
                {
                    if ( track.FolderID.Value == folder.Id )
                    {
                        folder.Add( track );
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
        private void PlaylistSort( List<Playlist> playlists, List<Track> tracklist )
        {
            List<Playlist> sortedList = new List<Playlist>();
            foreach ( Playlist playlist in playlists )
            {
                List<PlaylistTrack> playlistTracks = _database.GetPlaylistTracks( playlist );
                foreach ( PlaylistTrack playlistTrack in playlistTracks )
                {
                    foreach ( Track track in tracklist )
                    {
                        if ( track.Id == playlistTrack.TrackID )
                        {
                            playlist.Add( track );
                        }
                    }
                }
                playlist.IsModified = false;
            }
        }
    }
}
