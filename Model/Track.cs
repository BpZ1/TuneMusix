using System;
using System.ComponentModel;

namespace TuneMusix.Model
{
    public class Track : IDisposable, INotifyPropertyChanged
    {
        #region saved values
        private readonly long _id;
        private long _folderId;
        private string _url;
        private string _title;
        private string _interpret;
        private string _album;
        private int _year;
        private string _comm;
        private string _genre;
        private int _rating;
        private readonly string _duration;
        #endregion

        private bool _isValid; //Checked on update
        private bool _isCurrentTrack;

        public Interpret InterpretContainer { get; set; }
        public Album AlbumContainer { get; set; }
        public bool IsModified { get; set; }
        public Folder Container { get; set; }


        public Track(string url, long id, string title,
            string interpret, string album, int year, string comm, string genre, string duration)
        {
            this._url = url;
            this._id = id;
            this._title = title;
            this._interpret = interpret;
            this._album = album;
            this._year = year;
            this._comm = comm;
            this._genre = genre;
            this._rating = 0;
            this._duration = duration;
        }

        public Track(string url, long id, long folderId, string title, 
            string interpret, string album, int year, string comm, string genre, int rating, string duration)
        {
            this._url = url;
            this._id = id;
            this._folderId = folderId;
            this._title = title;
            this._interpret = interpret;
            this._album = album;
            this._year = year;
            this._comm = comm;
            this._genre = genre;
            this._rating = rating;
            this._duration = duration;
        }

        //events
        public delegate void TrackChangedEventHandler(object source);

        public event TrackChangedEventHandler TrackChanged;

        protected virtual void OnTrackChanged()
        {
            TrackChanged?.Invoke(this);
        }


        /// <summary>
        /// Removes the reference to the track from the folder it is in.
        /// </summary>
        public void Dispose()
        {
            if(this.Container != null)
            {
                this.Container.Remove(this);
                this.AlbumContainer.Remove(this);
                this.InterpretContainer.Remove(this);
            }
        }

        //getter and setter
        public long ID
        {
            get { return this._id; }
        }
        public long FolderID
        {
            get { return this._folderId; }
            set
            {
                _folderId = value;
                IsModified = true;
                RaisePropertyChanged("FolderID");
                OnTrackChanged();
            }
        }
        public string SourceURL
        {
            get
            {
                if (_url != null)
                {
                    return _url;
                }
                throw new ArgumentNullException("URL");
            }
            set
            {
                if (value != null)
                {
                    _url = value;
                    IsModified = true;
                    RaisePropertyChanged("sourceURL");
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
                IsModified = true;
                OnTrackChanged();
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Name");
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
                IsModified = true;
                RaisePropertyChanged("Interpret");
                RaisePropertyChanged("Name");
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
                IsModified = true;
                RaisePropertyChanged("Album");
                OnTrackChanged();
            }
        }
        public int Year
        {
            get
            {  
                return this._year;             
            }
            set
            {
                this._year = value;
                IsModified = true;
                RaisePropertyChanged("Album");
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
                IsModified = true;
                RaisePropertyChanged("Comm");
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
                IsModified = true;
                RaisePropertyChanged("Genre");
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
                IsModified = true;
                RaisePropertyChanged("Rating");
                OnTrackChanged();
            }
        }
        /// <summary>
        /// Combination of Title and Interpret of the track.
        /// Form: "Title - Interpret"
        /// If one of them is empty "..." will be 
        /// </summary>
        public string Name
        {
            get
            {
                if(this.Interpret == null)
                    return this.Title;

                if (this.Interpret.Equals(""))
                    return this.Title;

                return this.Title + " - " + this.Interpret;
            }
        }
        /// <summary>
        /// Index used to save potition for unshuffling.
        /// </summary>
        public int Index { get; set; }
        public bool IsCurrentTrack
        {
            get { return _isCurrentTrack; }
            set
            {
                _isCurrentTrack = value;
                RaisePropertyChanged("IsCurrentTrack");
            }
        }
        public string Duration
        {
            get { return _duration; }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        public bool Contains(string value)
        {
            if (value == null)
                throw new ArgumentException("Value can't be null.");

            if (Title.ToLower().Contains(value.ToLower()))
                return true;

            if (Interpret.ToLower().Contains(value.ToLower()))
                return true;

            if (Album.ToLower().Contains(value.ToLower()))
                return true;

            return false;
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
