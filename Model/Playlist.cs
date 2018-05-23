using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist : INotifyPropertyChanged
    {
        private long id;
        private string name;
        public bool IsModified { get; set; }
        public ObservableCollection<Track> Tracklist { get; set; }

        //Constructor-------
        public Playlist(string name,long ID)
        {
            this.Name = name;
            this.id = ID;
            Tracklist = new ObservableCollection<Track>();
        }
        //Constructor-------
        public Playlist(string name,Track track,long ID)
        {
            this.Name = name;
            this.id = ID;
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
            this.id = ID;
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
            get { return this.name; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Name can't be null.");
                }
                else
                {
                    this.name = value;
                    RaisePropertyChanged("Name");
                    IsModified = true;
                }
             }
        }

        public long ID
        {
            get { return this.id; }
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
            RaisePropertyChanged("Tracklist");
        }

        /// <summary>
        /// Adds a track to the playlist. (no database entry)
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(Track track)
        {
            this.Tracklist.Add(track);
            RaisePropertyChanged("Tracklist");
        }



        #region propertychanged
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
