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
    public partial class DataModel
    {

        private static DataModel instance;
        SQLManager DBManager;
        private DataModel()
        {
            DBManager = new SQLManager();
        }

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
   

        //events/////////////////////////////////////////////////////////////////////////////////
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
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Deletes a track from the folder, tracklist and database
        /// </summary>
        /// <param name="track"></param>
        public void Delete(Track track)
        {
            //TODO remove track from db here
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
            //TODO remove folder from db here
            foreach (Track track in folder.Tracklist)
            {
                //TODO remove track from db here
                TrackList.Remove(track);
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

        public void AddTracks(List<Track> tracks)
        {

        }

       
        /// <summary>
        /// Checks if a folder is already contained in the list, or if it
        /// is a parent/child of an existing folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool AddRootFolder(Folder folder)
        {
            if (!RootFolders.Contains(folder))
            {
                RootFolders.Add(folder);
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

    

    }
}
