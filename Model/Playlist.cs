using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist : BaseModel
    {
        private long _id;
        private string _name;
        private bool _modified;
        private ObservableCollection<Track> _trackList;

        //Constructor-------
        public Playlist(string name,long ID)
        {
            this._modified = false;
            this.Name = name;
            this._id = ID;
        }
        //Constructor-------
        public Playlist(string name,Track track,long ID)
        {

            this.Name = name;
            this._id = ID;
            this._modified = false;
            if (track != null)
            {
                Tracklist.Add(track);
            }
            
        }
        //Constructor-------
        public Playlist(string name,List<Track> tracks,long ID)
        {
            this.Name = name;
            this._id = ID;
            this._modified = false;
            foreach (Track track in tracks)
            {
                if (track != null)
                {
                    Tracklist.Add(track);
                }
            }
        }

        //Getter and setter
        public string Name
        {
            get { return this._name; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Name can't be null.");
                }
                else if (value.Length <= 0)
                {
                    throw new ArgumentException("Name has to contain more than 0 symbols");
                }
                else
                {
                    this._name = value;
                    this.Modified = true;
                    RaisePropertyChanged("name");
                }
             }
        }
        public bool Modified
        {
            get { return this._modified; }
            set { this._modified = value; }
        }
        public ObservableCollection<Track> Tracklist
        {
            get
            {
                return this._trackList;
            }
            set
            {
                this._trackList = value;
                this.Modified = true;
            }
        }

        public long ID
        {
            get { return this._id; }
        }
        
        /// <summary>
        /// Adds one Element to the List.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool AddTrack(Track track)
        {
            ValidationUtil<Track> valiUtil = new ValidationUtil<Track>();
            if (valiUtil.insertValidation(track.Title,this._name,track,_trackList))
            {
                _trackList.Add(track);
                this.Modified = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds all Elements that are not Null or already contained to the List.
        /// </summary>
        /// <param name="tracks">Tracks to be added to the List.</param>
        /// <returns>Number of Elements added.</returns>
        public int AddAllTracks(List<Track> tracks)
        {
            int TracksAdded = 0;
            ValidationUtil<Track> valiUtil = new ValidationUtil<Track>();
            foreach (Track track in tracks)
            {
                if(valiUtil.insertValidation(track.Title, this._name, track, _trackList))
                {
                    _trackList.Add(track);
                    TracksAdded++;
                }
            }
            this.Modified = true;
            return TracksAdded;
        }

        


    }
}
