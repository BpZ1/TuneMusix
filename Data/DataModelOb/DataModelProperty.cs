using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;


namespace TuneMusix.Data.DataModelOb
{
    public partial class DataModel
    {
        //Progress for the loading of tracks
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnProgressChanged();
            }
        }
        //list containing all tracks
        public ObservableCollection<Track> TrackList
        {
            get { return this.tracklist; }
        }
        //currently loaded track
        public Track CurrentTrack
        {
            get { return this.currentTrack; }
            set
            {
                this.currentTrack = value;
                OnCurrentTrackChanged();
            }
        }
        //list containing all playlists
        public ObservableCollection<Playlist> Playlists
        {
            get { return this.playlists; }
            set
            {
                this.playlists = value;
                OnDataModelChanged();
            }
        }
        //currently loaded playlist
        public Playlist CurrentPlaylist
        {
            get { return this.currentPlaylist; }
            set
            {
                this.currentPlaylist = value;
                if(value != null)
                {
                    TrackQueue = value.Tracklist;
                }            
                OnCurrentPlaylistChanged();
            }
        }
        //Returns true if the current trackqueue is shuffled
        public bool TrackQueueIsShuffled
        {
            get { return trackQueueIsShuffled; }
        }
        //list of all root folders
        public ObservableCollection<Folder> RootFolders
        {
            get { return this.rootFolders; }
        }
        //list of the tracks that are in the playing queue
        public ObservableCollection<Track> TrackQueue
        {
            get { return this.trackQueue; }
            set
            {
                if(value != null)
                {
                    if(trackQueue != null)
                    {
                        if (!ListUtil.UnorderedEqual<Track>(value, trackQueue))
                            trackQueueIsShuffled = false;
                    }
                       
                    //Shuffle track queue if shuffle is activated
                    if (Options.Instance.Shuffle && !trackQueueIsShuffled)
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
                 
                this.trackQueue = value;
                this.QueueIndex = 0;
               
               
                OnTrackQueueChanged();
            }
        }
        //list containing all loaded effects
        public ObservableCollection<BaseEffect> EffectQueue
        {
            get { return effectQueue; }
            set { EffectQueue = value; }
        }
    }
}
