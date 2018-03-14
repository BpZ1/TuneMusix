using System.Collections.Concurrent;
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
            get { return this.tracklist; }
        }
        public Track CurrentTrack
        {
            get { return this.currentTrack; }
            set
            {
                this.currentTrack = value;
                OnCurrentTrackChanged();
            }
        }
        public ObservableCollection<Playlist> Playlists
        {
            get { return this.playlists; }
            set
            {
                this.playlists = value;
                OnDataModelChanged();
            }
        }
        public Playlist CurrentPlaylist
        {
            get { return this.currentPlaylist; }
            set
            {
                this.currentPlaylist = value;
                if(value != null)
                {
                    TrackQueue = value.Tracklist.ToList<Track>();
                }            
                OnCurrentPlaylistChanged();
            }
        }
        public ObservableCollection<Folder> RootFolders
        {
            get { return this.rootFolders; }
        }

        public List<Track> TrackQueue
        {
            get { return this.trackQueue; }
            set
            {
                this.trackQueue = value;
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
            get { return effectQueue; }
            set { EffectQueue = value; }
        }
    }
}
