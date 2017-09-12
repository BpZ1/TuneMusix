using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;

//duplicate code!!!

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist : BaseModel
    {
        private string name;
        private ObservableCollection<Track> trackList;

        //Constructor-------
        public Playlist(string name)
        {
            trackList = new ObservableCollection<Track>();
            if (name == null)
            {
                throw new ArgumentNullException("Name can't be null.");
            }
            else if (name.Length <= 0)
            {
                throw new ArgumentException("Name has to contain more than 0 symbols");
            }
            else
            {
                this.name = name;
            }
        }
        //Constructor-------
        public Playlist(string name,Track track)
        {
            //maybe check for validation
            trackList = new ObservableCollection<Track>();
            if (name == null)
            {
                throw new ArgumentNullException("Name can't be null.");
            }
            else if(name.Length <= 0)
            {
                throw new ArgumentException("Name has to contain more than 0 symbols");
            }
            else
            {
                this.name = name;
                if(track != null)
                {
                    trackList.Add(track);
                }
                else
                {
                    throw new ArgumentNullException("Track can't be null.");

                }
            }
        }
        //Constructor-------
        public Playlist(string name,List<Track> tracks)
        {
            //maybe check for validation
            trackList = new ObservableCollection<Track>();
            if (name == null)
            {
                throw new ArgumentNullException("Name can't be null.");
            }
            else if (name.Length <= 0)
            {
                throw new ArgumentException("Name has to contain more than 0 symbols");
            }
            else
            {
                this.name = name;
                if (tracks != null)
                {
                    foreach(Track track in tracks)
                    {
                        trackList.Add(track);
                    }
                }
                else
                {
                    throw new ArgumentNullException("Track can't be null.");

                }
            }
        }

        /// <summary>
        /// Getter and setter for the name of the Playlist.
        /// </summary>
        public string Name
        {
            get { return this.name; }
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
                    this.name = value;
                    RaisePropertyChanged("name");
                }
             }
        }
        
        /// <summary>
        /// Adds one Element to the List.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool AddTrack(Track track)
        {
            ValidationUtil<Track> valiUtil = new ValidationUtil<Track>();
            if (valiUtil.insertValidation(track.tTitle,this.name,track,trackList))
            {
                trackList.Add(track);
                Logger.log("Track '" + track.tTitle + "' has been added to the playlist " + this.name);
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
                if(valiUtil.insertValidation(track.tTitle, this.name, track, trackList))
                {
                    trackList.Add(track);
                    Logger.log("Track '" + track.tTitle + "' has been added to the playlist " + this.name);
                    TracksAdded++;
                }
            }
            Logger.log(TracksAdded + " of " + tracks.Count + " have been added.");
            return TracksAdded;

        }

        


    }
}
