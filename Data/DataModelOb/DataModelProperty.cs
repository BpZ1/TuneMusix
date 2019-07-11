using TuneMusix.Helpers.Util;
using TuneMusix.Model;


namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {
        //list containing all tracks
        public ObservableList<Track> TrackList => this._tracklist;
 
        //list containing all playlists
        public ObservableList<Playlist> Playlists
        {
            get { return this._playlists; }
            set
            {
                this._playlists = value;
                OnDataModelChanged();
            }
        }
        //currently loaded playlist
        public Playlist CurrentPlaylist
        {
            get { return this._currentPlaylist; }
            set
            {
                this._currentPlaylist = value;
                if(value != null)
                {
                    TrackQueue.Queue = value.Itemlist;
                }            
                OnCurrentPlaylistChanged();
            }
        }
        //Returns true if the current trackqueue is shuffled
        public bool TrackQueueIsShuffled
        {
            get { return _trackQueueIsShuffled; }
        }
        //list of all root folders
        public ObservableList<Folder> RootFolders
        {
            get { return this._rootFolders; }
        }

        public ObservableList<Album> Albumlist
        {
            get { return _albumlist; }
            set
            {
                _albumlist = value;
                OnAlbumlistChanged();
            }
        }
        public ObservableList<Interpret> Interpretlist
        {
            get { return _interpretlist; }
            set
            {
                _interpretlist = value;
                OnInterpretlistChanged();
            }
        }
    }
}
