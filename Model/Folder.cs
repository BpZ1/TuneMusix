﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Folder
    {
        private long _id;
        private long _folderID;
        private string _name;
        private string _url;
        private ObservableCollection<Folder> _folderlist;
        private ObservableCollection<Track> _tracklist;

        public Folder(string name, string url, long ID)
        {
            this.Name = name;
            this.URL = url;
            this._id = ID;
            _folderlist = new ObservableCollection<Folder>();
            _tracklist = new ObservableCollection<Track>();
        }

        public Folder(string name, string url, long ID,long folderID)
        {
            this.Name = name;
            this.URL = url;
            this._id = ID;
            this._folderID = folderID;
            _folderlist = new ObservableCollection<Folder>();
            _tracklist = new ObservableCollection<Track>();
        }
        //Events

        public delegate void FolderChangedEventHandler(object source);

        public event FolderChangedEventHandler FolderChanged;

        protected virtual void OnFolderChanged()
        {
            if(FolderChanged != null)
            {
                FolderChanged(this);
            }
        }


        /// <summary>
        /// Method for Adding a container to a container.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool AddFolder(Folder folder)
        {
            ValidationUtil<Folder> valiUtil = new ValidationUtil<Folder>();
            if(folder != null)
            {
                if (valiUtil.insertValidation(folder._name, this._name, folder, _folderlist))
                {
                    _folderlist.Add(folder);
                    folder.FolderID = this.ID;
                    OnFolderChanged();
                    return true;
                }
                else { return false; }
            }
            return false;          
        }

        public bool AddTrack(Track track)
        {
            ValidationUtil<Track> valiUtil = new ValidationUtil<Track>();
            if(track != null)
            {
                if (valiUtil.insertValidation(track.Title, this._name, track, _tracklist))
                {
                    _tracklist.Add(track);
                    track.FolderID = this.ID;
                    OnFolderChanged();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }



        // Setter and Getter for the name of the Container
        public string Name
        {
            get { return this._name; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    this._name = value;
                    OnFolderChanged();
                }
                else
                {
                    //---Need new Exception or maybe no exception
                    throw new InvalidOperationException("Name of a Folder has to be longer then 0");
                }
            }
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
                OnFolderChanged();
            }
        }
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
                OnFolderChanged();
            }
        }

        public ObservableCollection<Track> Tracklist
        {
            get
            {
                return this._tracklist;
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
                new CollectionContainer() { Collection = Tracklist },
                new CollectionContainer() { Collection = Folderlist }
            };
            }
        }


    }
}
