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
        private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
        private ObservableCollection<Track> tracklist = new ObservableCollection<Track>();
        private ObservableCollection<Track> selectedTracks = new ObservableCollection<Track>();
        private ObservableCollection<Folder> rootFolders = new ObservableCollection<Folder>();
        private ObservableCollection<Track> trackQueue = new ObservableCollection<Track>();
        private ObservableCollection<BaseEffect> effectQueue = new ObservableCollection<BaseEffect>();
        private ObservableCollection<Album> albumlist = new ObservableCollection<Album>();
        private ObservableCollection<Interpret> interpretlist = new ObservableCollection<Interpret>();

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
                            instance = new DataModel();
                        }
                    }
                }
                return instance;
            }
        }


        private DataModel()
        {
            QueueIndex = 0;
            tracklist.CollectionChanged += dataModelChanged;
            playlists.CollectionChanged += dataModelChanged;
            rootFolders.CollectionChanged += dataModelChanged;
            effectQueue.CollectionChanged += dataModelChanged;
        }
        #endregion



        #region events
        public delegate void DataModelChangedEventHandler(object source,object changedObject);     
        public event DataModelChangedEventHandler CurrentTrackChanged;
        public event DataModelChangedEventHandler CurrentPlaylistChanged;
        public event DataModelChangedEventHandler DataModelChanged;
        public event DataModelChangedEventHandler TrackQueueChanged;
        public event DataModelChangedEventHandler EffectQueueChanged;
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
        protected virtual void OnEffectQueueChanged()
        {
            if (EffectQueueChanged != null)
                EffectQueueChanged(this, EffectQueue);
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
