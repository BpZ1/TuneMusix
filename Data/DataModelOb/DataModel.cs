using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.MediaPlayer.Effects;
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
        private ObservableCollection<Playlist> playlists;
        private ObservableCollection<Track> tracklist;
        private ObservableCollection<Folder> rootFolders;
        private ObservableCollection<Album> albumlist;
        private ObservableCollection<Interpret> interpretlist;
        private ObservableCollection<Track> selectedTracks = new ObservableCollection<Track>();
        private ObservableCollection<Track> trackQueue = new ObservableCollection<Track>();

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
            this.tracklist = new ObservableCollection<Track>(tracklist);
            this.rootFolders = new ObservableCollection<Folder>(rootfolders);
            this.playlists = new ObservableCollection<Playlist>(playlists);
            this.albumlist = new ObservableCollection<Album>(albumlist);
            this.interpretlist = new ObservableCollection<Interpret>(interpretlist);

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
            if (CurrentTrackChanged != null)
                CurrentTrackChanged(this, CurrentTrack);
        }
        protected virtual void OnCurrentPlaylistChanged()
        {
            if (CurrentPlaylistChanged != null)
                CurrentPlaylistChanged(this, CurrentPlaylist);
        }
        protected virtual void OnDataModelChanged()
        {
            if (DataModelChanged != null)
                DataModelChanged(this, TrackList);
        }
        protected virtual void OnTrackQueueChanged()
        {
            if (TrackQueueChanged != null)
                TrackQueueChanged(this, TrackQueue);
        }
        protected virtual void OnAlbumlistChanged()
        {
            if (AlbumlistChanged != null)
                AlbumlistChanged(this, albumlist);
        }
        protected virtual void OnInterpretlistChanged()
        {
            if (InterpretlistChanged != null)
                InterpretlistChanged(this, interpretlist);
        }



        #endregion

        private void dataModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnDataModelChanged();
        }

    }
}
