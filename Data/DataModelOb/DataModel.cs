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
        private Playlist _currentPlaylist = null;
        private Track _CurrentTrack = null;
        private ObservableCollection<Playlist> _playlists = new ObservableCollection<Playlist>();
        private ObservableCollection<Track> _tracklist = new ObservableCollection<Track>();
        private ObservableCollection<Track> _selectedTracks = new ObservableCollection<Track>();
        private ObservableCollection<Folder> _rootFolders = new ObservableCollection<Folder>();
        private List<Track> _trackQueue = new List<Track>();
        private ObservableCollection<BaseEffect> _effectQueue = new ObservableCollection<BaseEffect>();
        
   

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
