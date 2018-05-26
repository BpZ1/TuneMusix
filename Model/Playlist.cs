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
    public class Playlist : ItemContainer<Track>, INotifyPropertyChanged
    {
        private long id;
        public bool IsModified { get; set; }

        public Playlist(string name,long ID) : base(name)
        {
            this.id = ID;
        }

        public Playlist(string name,Track track,long ID) : base(name)
        {
            this.id = ID;
            if (track != null)
            {
                Itemlist.Add(track);
            }
        }

        public Playlist(string name,List<Track> tracks,long ID) : base(name)
        {
            this.id = ID;
            foreach (Track track in tracks)
            {
                if (track != null)
                {
                    Itemlist.Add(track);
                }
            }          
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                IsModified = true;
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
        public void Add(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {             
                Itemlist.Add(track);             
            }
            RaisePropertyChanged("Tracklist");
        }

    }
}
