using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using TuneMusix.Helpers.Util;

namespace TuneMusix.Model
{
    public class Folder : ItemContainer<Track>, INotifyPropertyChanged
    {
        public readonly string Id;
        private string _folderId;
        private string _url;
        public bool IsModified { get; set; }
        public Folder Container { get; set; }

        public Folder( string name, string url, string id ) : base( name )
        {
            URL = url;
            Id = id;
            Folderlist = new ObservableList<Folder>();
        }

        public Folder( string name, string url, string id, string folderId ) : this( name, url, id )
        {
            _folderId = folderId;
            Folderlist = new ObservableList<Folder>();
        }

        public bool Add( Folder folder )
        {
            if ( folder == null )
            {
                throw new ArgumentNullException( "You can't add Null to container" );
            }

            if ( !Folderlist.Contains( folder ) )
            {
                Folderlist.Add( folder );
                folder.FolderId = this.Id;
                folder.Container = this;
                NotifyPropertyChanged( nameof( Folderlist ) );
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
                if ( !base.IsEmpty || !( Folderlist.Count == 0 ) )
                {
                    return false;
                }
                else
                {
                    //Check if the subfolders are empty
                    bool foldersEmpty = true;
                    foreach ( Folder folder in Folderlist )
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
                if ( value == null )
                {
                    throw new ArgumentNullException( "URL mustn't be null." );
                }
                this._url = value;
                NotifyPropertyChanged();
                IsModified = true;
                OnContainerChanged();
            }
        }

        public string FolderId
        {
            get { return this._folderId; }
            set
            {
                _folderId = value;
                IsModified = true;
                NotifyPropertyChanged();
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

        public override bool Equals( object obj )
        {
            if ( !( obj is Folder otherFolder ) )
            {
                return false;
            }
            return _url.Equals( otherFolder.URL );
        }

        public override int GetHashCode()
        {
            return _url.GetHashCode();
        }
    }
}
