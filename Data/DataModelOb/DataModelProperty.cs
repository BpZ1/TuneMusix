using System.Collections.ObjectModel;
using System.Linq;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;


namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {
        //list containing all tracks
        public ObservableList<Track> TrackList => this._tracklist;
        //currently loaded track
        public Track CurrentTrack
        {
            get { return this._currentTrack; }
            set
            {
                if(_currentTrack != null)
                    this._currentTrack.IsCurrentTrack = false;

                //Set new track to current track
                if(value != null)
                {
                    value.IsCurrentTrack = true;
                    //Update the track and check if it is valid.
                    FileParser parser = new FileParser();
                    if (parser.UpdateTrack(value))
                    {
                        value.IsValid = true;
                    }
                    else
                    {
                        value.IsValid = false;
                    }
                    if (value.IsModified)
                    {
                        SaveTrack(value);
                    }
                }
                this._currentTrack = value;              
                OnCurrentTrackChanged();
            }
        }
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
                    TrackQueue = value.Itemlist;
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
        //list of the tracks that are in the playing queue
        public ObservableList<Track> TrackQueue
        {
            get { return this._trackQueue; }
            set
            {
                if(value != null)
                {
                    if(_trackQueue != null)
                    {
                        if (!ListUtil.UnorderedEqual<Track>(value, _trackQueue))
                            _trackQueueIsShuffled = false;
                    }
                       
                    //Shuffle track queue if shuffle is activated
                    if (Options.Instance.Shuffle && !_trackQueueIsShuffled)
                        ShuffleTrackQueue();

                    if (value.Count > 0)
                    {
                        CurrentTrack = value.First();
                    }
                    else
                    {
                        CurrentTrack = null;
                    }
                }
                 
                this._trackQueue = value;
                this.QueueIndex = 0;
               
               
                OnTrackQueueChanged();
            }
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
