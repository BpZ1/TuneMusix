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
        private long id;
        private long folderID;
        private string url;
        private string title;
        private string interpret;
        private string album;
        private int year;
        private string comm;
        private string genre;
        private int rating;
        public bool IsModified { get; set; }
        public Folder Container { get; set; }

        private int index; //original position in the trackqueue

        /// <summary>
        /// Constructor for Track
        /// </summary>
        /// <param name="URL"></param> 
        public Track(string URL,long ID)
        {
            this.id = ID;
            this.folderID = 0;
            this.url = URL;
            Rating = 0;
        }

        public Track(string URL, long ID,long folderID)
        {
            this.id = ID;
            this.folderID = folderID;
            this.url = URL;
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
            get { return this.id; }
        }
        public long FolderID
        {
            get { return this.folderID; }
            set
            {
                folderID = value;
                IsModified = true;
                RaisePropertyChanged("FolderID");
                OnTrackChanged();
            }
        }
        public string sourceURL
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

        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
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
