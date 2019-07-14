using TuneMusix.Helpers.Util;
using TuneMusix.Model;


namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {
        //list containing all tracks
        public ObservableList<Track> TrackList => _tracklist;

        //list containing all playlists
        public ObservableList<Playlist> Playlists => _playlists;

        //list of all root folders
        public ObservableList<Folder> RootFolders => _rootFolders;

        public ObservableList<Album> Albumlist => _albumlist;

        public ObservableList<Interpret> Interpretlist => _interpretlist;

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

    }
}
