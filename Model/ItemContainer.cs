using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    /// <summary>
    /// Base class for Classes containing a list of tracks.
    /// </summary>
    public abstract class ItemContainer<T>
    {
        protected string name;
        protected ObservableCollection<T> itemlist;

        public ItemContainer(string name)
        {
            this.name = name;
            itemlist = new ObservableCollection<T>();
        }

        public delegate void ContainerChangedEventHandler(object source);

        public event ContainerChangedEventHandler ContainerChanged;

        protected virtual void OnContainerChanged()
        {
            if (ContainerChanged != null)
            {
                ContainerChanged(this);
            }
        }


        #region properties
        public virtual string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Container name can't be null or empty");
                }
                name = value;
                RaisePropertyChanged("Name");
                OnContainerChanged();
            }

        }
        public ObservableCollection<T> Itemlist
        {
            get { return itemlist; }
        }
        #endregion
        public virtual bool Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("You can't add Null to container");

            if (!itemlist.Contains(item))
            {
                itemlist.Add(item);
                RaisePropertyChanged("Tracklist");
                OnContainerChanged();
                return true;
            }
            return false;
        }
        public virtual bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException("You can't remove Null from container");

            if(itemlist.Remove(item))
            {                
                RaisePropertyChanged("Tracklist");
                OnContainerChanged();
            }           
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
