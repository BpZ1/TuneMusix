using System;
using System.ComponentModel;

namespace TuneMusix.Model
{
    public class Track : IDisposable, INotifyPropertyChanged
    {
        #region saved values
        private long id;
        private long folderId;
        private string url;
        private string title;
        private string interpret;
        private string album;
        private int year;
        private string comm;
        private string genre;
        private int rating;
        private string duration;
        #endregion

        private bool isValid; //Checked on update
        private bool isCurrentTrack;

        public Interpret interpretContainer { get; set; }
        public Album albumContainer { get; set; }

        public bool IsModified { get; set; }
        public Folder Container { get; set; }

        private int index; //original position in the trackqueue

        public Track(string url, long id, string title,
            string interpret, string album, int year, string comm, string genre, string duration)
        {
            this.url = url;
            this.id = id;
            this.title = title;
            this.interpret = interpret;
            this.album = album;
            this.year = year;
            this.comm = comm;
            this.genre = genre;
            this.rating = 0;
            this.duration = duration;
        }

        public Track(string url, long id, long folderId, string title, 
            string interpret, string album, int year, string comm, string genre, int rating, string duration)
        {
            this.url = url;
            this.id = id;
            this.folderId = folderId;
            this.title = title;
            this.interpret = interpret;
            this.album = album;
            this.year = year;
            this.comm = comm;
            this.genre = genre;
            this.rating = rating;
            this.duration = duration;
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


        /// <summary>
        /// Removes the reference to the track from the folder it is in.
        /// </summary>
        public void Dispose()
        {
            if(this.Container != null)
            {
                this.Container.Remove(this);
                this.albumContainer.Remove(this);
                this.interpretContainer.Remove(this);
            }
        }

        //getter and setter
        public long ID
        {
            get { return this.id; }
        }
        public long FolderID
        {
            get { return this.folderId; }
            set
            {
                folderId = value;
                IsModified = true;
                RaisePropertyChanged("FolderID");
                OnTrackChanged();
            }
        }
        public string SourceURL
        {
            get
            {
                if (url != null)
                {
                    return url;
                }
                throw new ArgumentNullException("URL");
            }
            set
            {
                if (value != null)
                {
                    url = value;
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
                if(this.title != null)
                {
                    return this.title;
                }
                return "";
            }
            set
            {
                this.title = value;
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
                if (this.interpret != null)
                {
                    return this.interpret;
                }
                return "";
            }
            set
            {
                this.interpret = value;
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
                if (this.album != null)
                {
                    return this.album;
                }
                return "";
            }
            set
            {
                this.album = value;
                IsModified = true;
                RaisePropertyChanged("Album");
                OnTrackChanged();
            }
        }
        public int Year
        {
            get
            {  
                return this.year;             
            }
            set
            {
                this.year = value;
                IsModified = true;
                RaisePropertyChanged("Album");
                OnTrackChanged();
            }
        }
        public string Comm
        {
            get
            {
                if (this.comm != null)
                {
                    return this.comm;
                }
                return "";
            }
            set
            {
                this.comm = value;
                IsModified = true;
                RaisePropertyChanged("Comm");
                OnTrackChanged();
            }
        }
        public string Genre
        {
            get
            {
                if (this.genre != null)
                {
                    return this.genre;
                }
                return "";
            }
            set
            {

                this.genre = value;
                IsModified = true;
                RaisePropertyChanged("Genre");
                OnTrackChanged();
            }
        }
        public int Rating
        {
            get
            {
                return this.rating;
            }
            set
            {
                this.rating = value;
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
        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }
        public bool IsCurrentTrack
        {
            get { return isCurrentTrack; }
            set
            {
                isCurrentTrack = value;
                RaisePropertyChanged("IsCurrentTrack");
            }
        }
        public string Duration
        {
            get { return duration; }
        }
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        #region propertychanged
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public bool Contains(string value)
        {
            if (value == null)
                throw new ArgumentException("Value can't be null.");

            if (value == null)
                throw new ArgumentNullException();

            if (Title.ToLower().Contains(value.ToLower()))
                return true;

            if (Interpret.ToLower().Contains(value.ToLower()))
                return true;

            if (Album.ToLower().Contains(value.ToLower()))
                return true;

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
