using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data
{
    public class DataModel
    {

        private static DataModel instance;

        private DataModel() { }

        public static DataModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataModel();
                }
                return instance;
            }
        }

        //currently active playlist
        private Playlist _currentPlaylist = null;
        //List of all Playlists
        private ObservableCollection<Playlist> _Playlists = new ObservableCollection<Playlist>();
        //List of all tracks
        private ObservableCollection<Track> _TrackList = new ObservableCollection<Track>();
        //Currently playing track
        private Track _CurrentTrack = null;
        //The currently selected tracks
        private ObservableCollection<Track> _SelectedTracks = new ObservableCollection<Track>();
        //The currently selected playlist
        private Playlist _SelectedPlaylist = null;

        public double currentPosition { get; set; }

        //events
        public delegate void DataModelChangedEventHandler(object source,object changedObject);

        public event DataModelChangedEventHandler CurrentTrackChanged;

        public event DataModelChangedEventHandler TrackListChanged;

        public event DataModelChangedEventHandler CurrentPlaylistChanged;

        public event DataModelChangedEventHandler PlaylistsChanged;

        protected virtual void OnCurrentTrackChanged()
        {
            if(CurrentTrackChanged != null)
            {
                CurrentTrackChanged(this,CurrentTrackDM);
            }
        }

        protected virtual void OnTrackListChanged()
        {
            if (TrackListChanged != null)
            {
                TrackListChanged(this,TrackList);
            }
        }

        protected virtual void OnCurrentPlaylistChanged()
        {
            if (CurrentPlaylistChanged != null)
            {
                CurrentPlaylistChanged(this,CurrentPlaylist);
            }
        }
        protected virtual void OnPlaylistsChanged()
        {
            if (PlaylistsChanged != null)
            {
                PlaylistsChanged(this,Playlists);
            }
        }
        
        //Getter and setter
        public ObservableCollection<Track> TrackList
        {
            get { return this._TrackList; }
        }
        public Track CurrentTrackDM
        {
            get { return this._CurrentTrack; }
            set
            {
                if(this._CurrentTrack != value)
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
                OnPlaylistsChanged();
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

        public bool AddTrack(Track track)
        {
            bool contained = false;
            foreach (Track t in TrackList)
            {
                if (track.url.Equals(t.url))
                {
                    contained = true;
                }
            }
            if(contained == false)
            {
                TrackList.Add(track);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a Track is already in the List and then adds it.
        /// </summary>
        /// <param name="URL"></param>
        public bool AddTrackURL(string URL)
        {
            if(URL == null) { return false; }
            bool contained = false;
            foreach (Track track in TrackList)
            {
                if (track.url.Equals(URL))
                {
                    contained = true;
                }
            }
            if (contained == false)
            {
                FileParser fileParser = new FileParser();
                TrackList.Add(fileParser.GetAudioData(URL));
                
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Deletes a track from the tracklist if contained.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True if track was found and deleted, otherwise false.</returns>
        public bool DeleteTrackFromList(Track track)
        {
            Track removeObj = null;
            if (track == null)
            {
                Logger.Log("Could not remove track from Tracklist because object was null.");
                return false;
            }
            if (track.url == null)
            {
                Logger.Log("Could not remove track from Tracklist because url was null.");
                return false;
            }
            foreach (Track t in TrackList)
            {
                if (t.url.Equals(track.url))
                {
                    removeObj = t;
                }
            }
            if(removeObj == null)
            {
                Logger.Log("Could not remove track from Tracklist because no object was found.");
                return false;
            }
            else
            {
                Logger.Log("Track '" + track.url + "' was deleted from the Tracklist.");
                TrackList.Remove(removeObj);
                OnTrackListChanged();
                return true;
            }
        }

        public void AddFolder(string url)
        {
            FileParser fileParser = new FileParser();
            Folder folder = fileParser.GetFolderData(url);
            foreach (Track t in tList)
            {
                AddTrack(t);
            }
        }

    }
}
