using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;


namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {

        public ObservableCollection<Track> TrackList
        {
            get { return this._tracklist; }
        }
        public Track CurrentTrack
        {
            get { return this._CurrentTrack; }
            set
            {
                this._CurrentTrack = value;
                OnCurrentTrackChanged();
            }
        }
        public ObservableCollection<Playlist> Playlists
        {
            get { return this._playlists; }
            set
            {
                this._playlists = value;
                OnDataModelChanged();
            }
        }
        public Playlist CurrentPlaylist
        {
            get { return this._currentPlaylist; }
            set
            {
                this._currentPlaylist = value;
                if(value != null)
                {
                    TrackQueue = value.Tracklist.ToList<Track>();
                }            
                OnCurrentPlaylistChanged();
            }
        }
        public ObservableCollection<Folder> RootFolders
        {
            get { return this._rootFolders; }
        }

        public List<Track> TrackQueue
        {
            get { return this._trackQueue; }
            set
            {
                this._trackQueue = value;
                this.QueueIndex = 0;
                if (value.Count > 0)
                {
                    CurrentTrack = value.First();
                }
                else
                {
                    CurrentTrack = null;
                }
                OnTrackQueueChanged();
            }
        }

        public ObservableCollection<BaseEffect> EffectQueue
        {
            get { return _effectQueue; }
        }
    }
}
