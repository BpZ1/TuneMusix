using System;
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
    public class Folder : BaseModel
    {
        private long ID;
        private string name;
        private string _url;
        private ObservableCollection<Folder> _folderlist;
        private ObservableCollection<Track> _tracklist;
       

        public Folder(string name,string url,long ID)
        {
            this.Name = name;
            this.URL = url;
            this.ID = ID;

             _folderlist = new ObservableCollection<Folder>();
            _tracklist = new ObservableCollection<Track>();
                    
        }

        /// <summary>
        /// Method for Adding a container to a container.
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns></returns>
        public bool AddFolder(Folder Folder)
        {
            ValidationUtil<Folder> valiUtil = new ValidationUtil<Folder>();
            if(Folder != null)
            {
                if (valiUtil.insertValidation(Folder.name, this.name, Folder, _folderlist))
                {
                    _folderlist.Add(Folder);
                    RaisePropertyChanged("containerList");
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
                if (valiUtil.insertValidation(track.Title, this.name, track, _tracklist))
                {
                    _tracklist.Add(track);
                    Logger.Log(track.Title + " has been added to " + this.name + ".");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Setter and Getter for the name of the Container
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    this.name = value;
                    RaisePropertyChanged("Name");
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
            }
        }
        public long GetID
        {
            get { return this.ID; }
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
