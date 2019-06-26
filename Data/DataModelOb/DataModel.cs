using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;

namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {

        private readonly Database _database = Database.Instance;
        private SQLLoader _loader;

        public int QueueIndex { get; set; }
        public double CurrentPosition { get; set; }
        public Folder SelectedFolder { get; set; }
        private Playlist _currentPlaylist;
        private Track _currentTrack;
        private bool _trackQueueIsShuffled;
        private ObservableList<Playlist> _playlists;
        private ObservableList<Track> _tracklist;
        private ObservableList<Folder> _rootFolders;
        private ObservableList<Album> _albumlist;
        private ObservableList<Interpret> _interpretlist;
        private ObservableList<Track> _trackQueue = new ObservableList<Track>();

        #region constructor and instance accessor

        private static volatile DataModel _instance;
        private static object _lockObject = new Object();

        public static DataModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            SQLLoader loader = new SQLLoader();
                            _instance = loader.LoadFromDB();
                        }
                    }
                }
                return _instance;
            }
        }


        public DataModel(List<Track> tracklist, List<Playlist> playlists, List<Folder> rootfolders,
            List<Album> albumlist, List<Interpret> interpretlist)
        {
            this._tracklist = new ObservableList<Track>(tracklist);
            this._rootFolders = new ObservableList<Folder>(rootfolders);
            this._playlists = new ObservableList<Playlist>(playlists);
            this._albumlist = new ObservableList<Album>(albumlist);
            this._interpretlist = new ObservableList<Interpret>(interpretlist);

            QueueIndex = 0;
            this._tracklist.CollectionChanged += CallOnDataModelChanged;
            this._playlists.CollectionChanged += CallOnDataModelChanged;
            this._rootFolders.CollectionChanged += CallOnDataModelChanged;
        }
        #endregion



        #region events
        public delegate void DataModelChangedEventHandler(object source,object changedObject);     
        public event DataModelChangedEventHandler CurrentTrackChanged;
        public event DataModelChangedEventHandler CurrentPlaylistChanged;
        public event DataModelChangedEventHandler DataModelChanged;
        public event DataModelChangedEventHandler TrackQueueChanged;
        public event DataModelChangedEventHandler AlbumlistChanged;
        public event DataModelChangedEventHandler InterpretlistChanged;
        protected virtual void OnCurrentTrackChanged()
        {
            CurrentTrackChanged?.Invoke(this, CurrentTrack);
        }
        protected virtual void OnCurrentPlaylistChanged()
        {
            CurrentPlaylistChanged?.Invoke(this, CurrentPlaylist);
        }
        protected virtual void OnDataModelChanged()
        {
            DataModelChanged?.Invoke(this, TrackList);
        }
        protected virtual void OnTrackQueueChanged()
        {
            TrackQueueChanged?.Invoke(this, TrackQueue);
        }
        protected virtual void OnAlbumlistChanged()
        {
            AlbumlistChanged?.Invoke(this, _albumlist);
        }
        protected virtual void OnInterpretlistChanged()
        {
            InterpretlistChanged?.Invoke(this, _interpretlist);
        }



        #endregion
        /// <summary>
        /// Exists to call OnDataModelChanged() automatically when the lists change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallOnDataModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataModelChanged();
        }

    }
}
