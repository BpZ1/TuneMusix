using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Model
{
    public class Track 
    {
        private long _id;
        private long _folderID;
        private string URL;
        private string _title;
        private string _interpret;
        private string _album;
        private string _year;
        private string _comm;
        private string _genre;
        private int _rating;

        /// <summary>
        /// Constructor for Track
        /// </summary>
        /// <param name="URL"></param> 
        public Track(string URL,long ID)
        {
            this._id = ID;
            this._folderID = -1;
            this.URL = URL;
            Rating = 0;
        }

        public Track(string URL, long ID,long folderID)
        {
            this._id = ID;
            this._folderID = folderID;
            this.URL = URL;
            Rating = 0;
        }


        //events
        public delegate void TrackChangedEventHandler(object source);

        public event TrackChangedEventHandler TrackChanged;

        protected virtual void OnTrackChanged()
        {
            if(TrackChanged != null)
            {
                TrackChanged(this);
            }
        }

        public void Dispose()
        {
            
        }

        //getter and setter
        public long ID
        {
            get { return this._id; }
        }
        public long FolderID
        {
            get { return this._folderID; }
            set
            {
                _folderID = value;
                OnTrackChanged();
            }
        }
        public string url
        {
            get
            {
                if (URL != null)
                {
                    return URL;
                }
                throw new NullReferenceException("URL");
            }
            set
            {
                if (value != null)
                {
                    URL = value;
                    OnTrackChanged();
                }
                throw new ArgumentNullException("URL can't be null");
            }
        }

        public string Title
        {
            get
            {
                if(this._title != null)
                {
                    return this._title;
                }
                return "";
            }
            set
            {
                this._title = value;
                OnTrackChanged();
            }
        }
        public string Interpret
        {
            get
            {
                if (this._interpret != null)
                {
                    return this._interpret;
                }
                return "";
            }
            set
            {
                this._interpret = value;
                OnTrackChanged();
            }
        }
        public string Album
        {
            get
            {
                if (this._album != null)
                {
                    return this._album;
                }
                return "";
            }
            set
            {
                this._album = value;
                OnTrackChanged();
            }
        }
        public string Year
        {
            get
            {
                if (this._year != null)
                {
                    return this._year;
                }
                return "";
            }
            set
            {
                this._year = value;
                OnTrackChanged();
            }
        }
        public string Comm
        {
            get
            {
                if (this._comm != null)
                {
                    return this._comm;
                }
                return "";
            }
            set
            {
                this._comm = value;
                OnTrackChanged();
            }
        }
        public string Genre
        {
            get
            {
                if (this._genre != null)
                {
                    return this._genre;
                }
                return "";
            }
            set
            {

                this._genre = value;
                OnTrackChanged();
            }
        }
        public int Rating
        {
            get
            {
                return this._rating;
            }
            set
            {
                this._rating = value;
                OnTrackChanged();
            }
        }
        public string Name
        {
            get
            {
                if(this.Interpret == null)
                {
                    return this.Title;
                }
                return this.Interpret + " - " + this.Title;
            }
        }
    }
}
