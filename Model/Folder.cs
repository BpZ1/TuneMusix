using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using TuneMusix.Helpers.Util;

namespace TuneMusix.Model
{
    public class Folder : ItemContainer<Track>, INotifyPropertyChanged
    {
        public readonly long Id;
        private long _folderId;
        private string _url;
        public bool IsModified { get; set; }
        public Folder Container { get; set; }

        public Folder(string name, string url, long ID) : base(name)
        {
            URL = url;
            Id = ID;
            Folderlist = new ObservableList<Folder>();
        }

        public Folder(string name, string url, long ID,long folderID) : this(name, url, ID)
        {
            _folderId = folderID;
            Folderlist = new ObservableList<Folder>();
        }


        public bool Add(Folder folder)
        {
            if(folder == null)
            {
                throw new ArgumentNullException("You can't add Null to container");
            }

            if (!Folderlist.Contains(folder))
            {
                Folderlist.Add(folder);
                folder.FolderId = this.Id;
                folder.Container = this;
                NotifyPropertyChanged("Folderlist");
                OnContainerChanged();
                return true;
            }
            return false;          
        }

        public override bool Add( Track track )
        {
            if ( base.Add( track ) )
            {
                track.FolderID.Value = Id;
                track.Container = this;
                return true;
            }
            return false;
        }

        public override bool IsEmpty
        {
            get
            {
                if( base.IsEmpty && Folderlist.Count == 0 )
                {
                    return true;
                }
                else
                {
                    //Check if the subfolders are empty
                    bool foldersEmpty = true;
                    foreach( Folder folder in Folderlist )
                    {
                        if ( !folder.IsEmpty )
                        {
                            foldersEmpty = false;
                        }
                    }
                    return foldersEmpty;
                }
            }
        }

        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("URL mustn't be null.");
                }
                this._url = value;
                NotifyPropertyChanged("URL");
                IsModified = true;
                OnContainerChanged();
            }
        }

        public long FolderId
        {
            get { return this._folderId; }
            set
            {
                _folderId = value;
                IsModified = true;
                NotifyPropertyChanged("FolderID");
                OnContainerChanged();
            }
        }

        public ObservableList<Folder> Folderlist { get; private set; }
        
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
