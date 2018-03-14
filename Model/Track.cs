using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TuneMusix.Model
{
    public class Track :IDisposable, INotifyPropertyChanged
    {
        private long _id;
        private long _folderID;
        private string URL;
        private string _title;
        private string _interpret;
        private string _album;
        private int _year;
        private string _comm;
        private string _genre;
        private int _rating;
        public bool IsModified { get; set; }
        public Folder Container { get; set; }

        /// <summary>
        /// Constructor for Track
        /// </summary>
        /// <param name="URL"></param> 
        public Track(string URL,long ID)
        {
            this._id = ID;
            this._folderID = 0;
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


        /// <summary>
        /// Removes the reference to the track from the folder it is in.
        /// </summary>
        public void Dispose()
        {
            if(this.Container != null)
            {
                this.Container.Tracklist.Remove(this);
            }
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
                IsModified = true;
                RaisePropertyChanged("FolderID");
                OnTrackChanged();
            }
        }
        public string sourceURL
        {
            get
            {
                if (URL != null)
                {
                    return URL;
                }
                throw new ArgumentNullException("URL");
            }
            set
            {
                if (value != null)
                {
                    URL = value;
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
        public string Name
        {
            get
            {
                if(this.Interpret == null||this.Interpret.Count() == 0)
                {
                    return this.Title;
                }
                return this.Interpret + " - " + this.Title;
            }
        }



        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }
    }
}
