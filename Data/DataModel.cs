using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data
{
    public class DataModel
    {

        private static DataModel instance;

        private DataModel() { }

        public static DataModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataModel();
                }
                return instance;
            }
        }

        //currently active playlist
        private Playlist _currentPlaylist = null;
        //List of all Playlists
        private ObservableCollection<Playlist> _Playlists = new ObservableCollection<Playlist>();
        //List of all tracks
        private ObservableCollection<Track> _TrackList = new ObservableCollection<Track>();
        //Currently playing track
        private Track _CurrentTrack = null;
        //The currently selected tracks
        private ObservableCollection<Track> _SelectedTracks = new ObservableCollection<Track>();
        //The currently selected playlist
        private Playlist _SelectedPlaylist = null;
        //List of rootfolders
        private ObservableCollection<Folder> _rootFolders = new ObservableCollection<Folder>();

        public double currentPosition { get; set; }
   

        //events
        public delegate void DataModelChangedEventHandler(object source,object changedObject);

        public event DataModelChangedEventHandler CurrentTrackChanged;

        public event DataModelChangedEventHandler CurrentPlaylistChanged;

        public event DataModelChangedEventHandler DataModelChanged;

        protected virtual void OnCurrentTrackChanged()
        {
            if(CurrentTrackChanged != null)
            {
                CurrentTrackChanged(this,CurrentTrack);
            }
        }
        protected virtual void OnCurrentPlaylistChanged()
        {
            if (CurrentPlaylistChanged != null)
            {
                CurrentPlaylistChanged(this, CurrentPlaylist);
            }
        }

        protected virtual void OnDataModelChanged()
        {
            if (DataModelChanged != null)
            {
                DataModelChanged(this,TrackList);
            }
        }
        /// <summary>
        /// Deletes a track from the folder, tracklist and database
        /// </summary>
        /// <param name="track"></param>
        public void Delete(Track track)
        {
            TrackList.Remove(track);
            track.Dispose();
            OnDataModelChanged();
        }
        /// <summary>
        ///  Deletes a folder and all of its content from the datamodel/database
        /// </summary>
        /// <param name="folder"></param>
        public void Delete(Folder folder)
        {
            foreach (Track track in folder.Tracklist)
            {
                TrackList.Remove(track);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.Delete(f);
            }
            if (folder.Container != null)
            {
                folder.Container.Folderlist.Remove(folder);
            }
            OnDataModelChanged();
        }
       

        //////////////////////////database methods///////////////////////////////////
        /// <summary>
        /// Method for adding tracks.
        /// Should only be used for loading from the database as it avoids all checks
        /// </summary>
        /// <param name="tracks"></param>
        /// 
        [DatabaseMethod]
        public void AddTracksDB(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {
                TrackList.Add(track);
            }
            OnDataModelChanged();
        }
        [DatabaseMethod]
        public void AddRootFoldersDB(List<Folder> folders)
        {
            foreach (Folder folder in folders)
            {
                RootFolders.Add(folder);
            }
            OnDataModelChanged();
        }
        [DatabaseMethod]
        public void AddPlaylistsDB(List<Playlist> playlists)
        {
            foreach (Playlist playlist in playlists)
            {
                Playlists.Add(playlist);
            }
            OnDataModelChanged();
        }
        //////////////////////////////////////////////////////////////////////////////

        //Getter and setter
        public ObservableCollection<Track> TrackList
        {
            get { return this._TrackList; }
        }
        public Track CurrentTrack
        {
            get { return this._CurrentTrack; }
            set
            {
                if(this._CurrentTrack != value)
                {
                    this._CurrentTrack = value;
                    OnCurrentTrackChanged();
                }          
            }
        }
        public ObservableCollection<Playlist> Playlists
        {
            get { return this._Playlists; }
            set
            {
                this._Playlists = value;
                OnDataModelChanged();
            }
        }
        public Playlist CurrentPlaylist
        {
            get { return this._currentPlaylist; }
            set
            {
                this._currentPlaylist = value;
                OnCurrentPlaylistChanged();
            }
        }
        public ObservableCollection<Track> SelectedTracks
        {
            get { return this._SelectedTracks; }
            set
            {
                this._SelectedTracks = value;
            }
        }
        public Playlist SelectedPlaylist
        {
            get { return this._SelectedPlaylist; }
            set
            {
                this._SelectedPlaylist = value;  
                    
            }
        }
        public ObservableCollection<Folder> RootFolders
        {
            get { return this._rootFolders; }
        }

        /// <summary>
        /// Adds a Track to the Tracklist after checking if it is already contained.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool AddTrack(Track track)
        {
            bool contained = false;
            foreach (Track t in TrackList)
            {
                if (track.url.Equals(t.url))
                {
                    contained = true;
                }
            }
            if(contained == false)
            {
                TrackList.Add(track);
                OnDataModelChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a Track is already in the List and then adds it.
        /// </summary>
        /// <param name="URL"></param>
        public bool AddTrackURL(string URL)
        {
            if(URL == null) { return false; }
            bool contained = false;
            foreach (Track track in TrackList)
            {
                if (track.url.Equals(URL))
                {
                    contained = true;
                }
            }
            if (contained == false)
            {
                FileParser fileParser = new FileParser();
                Track mp3 = fileParser.GetAudioData(URL);
                if(mp3 != null)
                {
                    TrackList.Add(mp3);
                    OnDataModelChanged();
                    return true;
                }           
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if a folder is already contained in the list, or if it
        /// is a parent/child of an existing folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool AddRootFolder(Folder folder)
        {
            if (RootFolders.Contains(folder))
            {
                return false;
            }
            bool contained = false;
            foreach (Folder f in RootFolders)
            {
                if (folder.URL.Contains(f.URL) || f.URL.Contains(folder.URL))
                {
                    contained = true;
                }
            }
            if (contained == false)
            {
                RootFolders.Add(folder);
              //  AddFolderContent(folder);
                OnDataModelChanged(); 
                return true;
            }
            return false;       
        }

        private void AddFolderContent(Folder folder)
        {
            foreach (Track t in folder.Tracklist)
            {
                this.AddTrack(t);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.AddFolderContent(f);
            }
            OnDataModelChanged();
        }

        /// <summary>
        /// Deletes a track from the tracklist if contained.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True if track was found and deleted, otherwise false.</returns>
        public bool DeleteTrackFromList(Track track)
        {
            Track removeObj = null;
            if (track == null)
            {
                return false;
            }
            if (track.url == null)
            {
                return false;
            }
            foreach (Track t in TrackList)
            {
                if (t.url.Equals(track.url))
                {
                    removeObj = t;
                }
            }
            if(removeObj == null)
            {
                return false;
            }
            else
            {
                TrackList.Remove(removeObj);
                OnDataModelChanged();
                return true;
            }
        }
        /// <summary>
        /// Adds a root folder and all of its content to the model/database
        /// </summary>
        /// <param name="url"></param>
        public void AddRootFolder(string url)
        {
            FileParser fileParser = new FileParser();
            Folder folder = fileParser.GetFolderData(url);
            folder.FolderID = 1;
            AddRootFolder(folder);
            AddFolderContent(folder);

        }
    

    }
}
