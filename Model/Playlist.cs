using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using WMPLib;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist
    {
        private long _id;
        private string _name;
        public ObservableCollection<Track> Tracklist { get; set; }

        //Constructor-------
        public Playlist(string name,long ID)
        {
            this.Name = name;
            this._id = ID;
            Tracklist = new ObservableCollection<Track>();
        }
        //Constructor-------
        public Playlist(string name,Track track,long ID)
        {
            this.Name = name;
            this._id = ID;
            Tracklist = new ObservableCollection<Track>();
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
            Tracklist = new ObservableCollection<Track>();
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
                else
                {
                    this._name = value;

                }
             }
        }

        public long ID
        {
            get { return this._id; }
        }

        /// <summary>
        /// Adds all Elements that are not Null or already contained to the List.
        /// </summary>
        /// <param name="tracks">Tracks to be added to the List.</param>
        /// <returns>Number of Elements added.</returns>
        public void AddTracks(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {             
                Tracklist.Add(track);             
            }
    
        }

        /// <summary>
        /// Adds a track to the playlist. (no database entry)
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(Track track)
        {
            this.Tracklist.Add(track);
        }
   
    }
}
