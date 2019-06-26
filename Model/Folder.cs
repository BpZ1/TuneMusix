using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace TuneMusix.Model
{
    public class Folder : ItemContainer<Track>, INotifyPropertyChanged
    {
        public readonly long ID;
        private long _folderID;
        private string _url;
        public bool IsModified { get; set; }
        public Folder Container { get; set; }
        private ObservableCollection<Folder> _folderlist;

        public Folder(string name, string url, long ID) : base(name)
        {
            this.URL = url;
            this.ID = ID;
            _folderlist = new ObservableCollection<Folder>();
        }

        public Folder(string name, string url, long ID,long folderID) : base(name)
        {
            this.URL = url;
            this.ID = ID;
            this._folderID = folderID;
            _folderlist = new ObservableCollection<Folder>();
        }


        public bool Add(Folder folder)
        {
            if(folder == null)
                throw new ArgumentNullException("You can't add Null to container");

            if (!_folderlist.Contains(folder))
            {
                _folderlist.Add(folder);
                folder.FolderID = this.ID;
                folder.Container = this;
                RaisePropertyChanged("Folderlist");
                OnContainerChanged();
                return true;
            }
            return false;          
        }

        public override bool Add(Track track)
        {
            if (base.Add(track))
            {
                track.FolderID = this.ID;
                track.Container = this;
                return true;
            }
            return false;
        }

        public string URL
        {
            get
            {
                return this._url;
            }
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("URL mustn't be null.");
                }
                this._url = value;
                RaisePropertyChanged("URL");
                IsModified = true;
                OnContainerChanged();
            }
        }

        public long FolderID
        {
            get { return this._folderID; }
            set
            {
                _folderID = value;
                IsModified = true;
                RaisePropertyChanged("FolderID");
                OnContainerChanged();
            }
        }

        public ObservableCollection<Folder> Folderlist
        {
            get
            {
                return this._folderlist;
            }
        }

        CompositeCollection cc = new CompositeCollection();
        
        /// <summary>
        /// Returns a IList for the Treeview
        /// </summary>
        public IList Children
        {
            get
            {
                return new CompositeCollection()
            {
                new CollectionContainer() { Collection = Itemlist },
                new CollectionContainer() { Collection = Folderlist }
            };
            }
        }
    }
}
