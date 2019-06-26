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

        private Database database = Database.Instance;
        private SQLLoader loader;

        public int QueueIndex { get; set; }
        public double CurrentPosition { get; set; }
        public Folder SelectedFolder { get; set; }
        private Playlist currentPlaylist;
        private Track currentTrack;
        private bool trackQueueIsShuffled;
        private ObservableList<Playlist> playlists;
        private ObservableList<Track> tracklist;
        private ObservableList<Folder> rootFolders;
        private ObservableList<Album> albumlist;
        private ObservableList<Interpret> interpretlist;
        private ObservableList<Track> selectedTracks = new ObservableList<Track>();
        private ObservableList<Track> trackQueue = new ObservableList<Track>();

        #region constructor and instance accessor

        private static volatile DataModel instance;

        private static object lockObject = new Object();

        public static DataModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            SQLLoader loader = new SQLLoader();
                            instance = loader.LoadFromDB();
                        }
                    }
                }
                return instance;
            }
        }


        public DataModel(List<Track> tracklist, List<Playlist> playlists, List<Folder> rootfolders,
            List<Album> albumlist, List<Interpret> interpretlist)
        {
            this.tracklist = new ObservableList<Track>(tracklist);
            this.rootFolders = new ObservableList<Folder>(rootfolders);
            this.playlists = new ObservableList<Playlist>(playlists);
            this.albumlist = new ObservableList<Album>(albumlist);
            this.interpretlist = new ObservableList<Interpret>(interpretlist);

            QueueIndex = 0;
            this.tracklist.CollectionChanged += dataModelChanged;
            this.playlists.CollectionChanged += dataModelChanged;
            this.rootFolders.CollectionChanged += dataModelChanged;
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
            AlbumlistChanged?.Invoke(this, albumlist);
        }
        protected virtual void OnInterpretlistChanged()
        {
            InterpretlistChanged?.Invoke(this, interpretlist);
        }



        #endregion

        private void dataModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataModelChanged();
        }

    }
}
