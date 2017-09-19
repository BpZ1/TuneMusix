using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Model;


namespace TuneMusix.Data
{
    public partial class DataModel
    {

        public ObservableCollection<Track> TrackList
        {
            get { return this._TrackList; }
        }
        public Track CurrentTrack
        {
            get { return this._CurrentTrack; }
            set
            {
                if (this._CurrentTrack != value)
                {
                    this._CurrentTrack = value;
                    OnCurrentTrackChanged();
                }
            }
        }
        public ObservableCollection<Playlist> Playlists
        {
            get { return this._Playlists; }
            set
            {
                this._Playlists = value;
                OnDataModelChanged();
            }
        }
        public Playlist CurrentPlaylist
        {
            get { return this._currentPlaylist; }
            set
            {
                this._currentPlaylist = value;
                OnCurrentPlaylistChanged();
            }
        }
        public ObservableCollection<Track> SelectedTracks
        {
            get { return this._SelectedTracks; }
            set
            {
                this._SelectedTracks = value;
            }
        }
        public Playlist SelectedPlaylist
        {
            get { return this._SelectedPlaylist; }
            set
            {
                this._SelectedPlaylist = value;
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
                CurrentTrack = value.First();
                OnTrackQueueChanged();
            }
        }
    }
}
