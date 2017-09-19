using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Attributes;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Model;

namespace TuneMusix.Data
{
    public partial class DataModel
    {

        private static DataModel _instance;
        SQLManager DBManager;
        private DataModel()
        {
            DBManager = new SQLManager();
            QueueIndex = 0;
        }

        public static DataModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataModel();
                }
                return _instance;
            }
        }

        public int QueueIndex { get; set; }
        public double CurrentPosition { get; set; }
        public Folder SelectedFolder { get; set; }
        private Playlist _currentPlaylist = null;
        private ObservableCollection<Playlist> _playlists = new ObservableCollection<Playlist>();
        private ObservableCollection<Track> _tracklist = new ObservableCollection<Track>();
        private Track _CurrentTrack = null;
        private ObservableCollection<Track> _selectedTracks = new ObservableCollection<Track>();
        private Playlist _selectedPlaylist = null;       
        private ObservableCollection<Folder> _rootFolders = new ObservableCollection<Folder>();
        private List<Track> _trackQueue;
        
   

        //events/////////////////////////////////////////////////////////////////////////////////
        public delegate void DataModelChangedEventHandler(object source,object changedObject);

        public event DataModelChangedEventHandler CurrentTrackChanged;
        public event DataModelChangedEventHandler CurrentPlaylistChanged;
        public event DataModelChangedEventHandler DataModelChanged;
        public event DataModelChangedEventHandler TrackQueueChanged;

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
        protected virtual void OnTrackQueueChanged()
        {
            if (TrackQueueChanged != null)
            {
                TrackQueueChanged(this,TrackQueue);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Deletes a track from the folder, tracklist and database
        /// </summary>
        /// <param name="track"></param>
        public void Delete(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {
                if(TrackQueue!= null)
                {
                    TrackQueue.Remove(track);
                }              
                TrackList.Remove(track);              
                foreach (Playlist playlist in Playlists)
                {
                    playlist.Tracklist.Remove(track);
                    //database removal-------------------------------------------------------------------------------------------
                }
                track.Dispose();
                OnDataModelChanged();
            }
            DBManager.Delete(tracks);         
        }


        /// <summary>
        ///  Deletes a folder and all of its content from the datamodel/database
        /// </summary>
        /// <param name="folder"></param>
        public void Delete(Folder folder)
        {
            DBManager.Delete(folder);
            foreach (Track track in folder.Tracklist)
            {
                TrackList.Remove(track);
                TrackQueue.Remove(track);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.Delete(f);
            }
            //Delete reference from container
            if (folder.Container != null)
            {
                folder.Container.Folderlist.Remove(folder);
            }
            if(folder.Container == null)
            {
                RootFolders.Remove(folder);
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
                if (track.sourceURL.Equals(t.sourceURL))
                {
                    contained = true;
                }
            }
            if(!contained)
            {
                TrackList.Add(track);
                OnDataModelChanged();
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// Checks if a folder is already contained in the list, or if it
        /// is a parent/child of an existing folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void AddRootFolder(Folder folder)
        {
            if (!RootFolders.Contains(folder))
            {
                RootFolders.Add(folder);
                
                TempFolderList = new List<Folder>();
                TempTrackList = new List<Track>();
                TempFolderList.Add(folder);
                AddFolderContent(folder);
                OnDataModelChanged();
                DBManager.AddAll(TempFolderList);
                DBManager.AddAll(TempTrackList);
                TempFolderList = null;
                TempTrackList = null;
            }
        }

        private List<Folder> TempFolderList;
        private List<Track> TempTrackList;

        private void AddFolderContent(Folder folder)
        {
            foreach (Track t in folder.Tracklist)
            {
                this.AddTrack(t);
                this.TempTrackList.Add(t);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.AddFolderContent(f);
                this.TempFolderList.Add(f);
            }
        }

    

    }
}
