using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Attributes;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.DataModelOb
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
        private Playlist currentPlaylist = null;
        private Track currentTrack = null;
        private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
        private ObservableCollection<Track> tracklist = new ObservableCollection<Track>();
        private ObservableCollection<Track> selectedTracks = new ObservableCollection<Track>();
        private ObservableCollection<Folder> rootFolders = new ObservableCollection<Folder>();
        private List<Track> trackQueue = new List<Track>();
        private ObservableCollection<BaseEffect> effectQueue = new ObservableCollection<BaseEffect>();
        
  

        //events/////////////////////////////////////////////////////////////////////////////////
        public delegate void DataModelChangedEventHandler(object source,object changedObject);

        public event DataModelChangedEventHandler CurrentTrackChanged;
        public event DataModelChangedEventHandler CurrentPlaylistChanged;
        public event DataModelChangedEventHandler DataModelChanged;
        public event DataModelChangedEventHandler TrackQueueChanged;
        public event DataModelChangedEventHandler EffectQueueChanged;

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
        protected virtual void OnEffectQueueChanged()
        {
            if(EffectQueueChanged != null)
            {
                EffectQueueChanged(this,EffectQueue);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////





    }
}
