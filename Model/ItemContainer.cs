using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuneMusix.Helpers.Util;

namespace TuneMusix.Model
{
    /// <summary>
    /// Base class for Classes containing a list of tracks.
    /// </summary>
    public abstract class ItemContainer<T>
    {
        protected string _name;
        protected ObservableList<T> _itemlist;

        public ItemContainer(string name)
        {
            this._name = name;
            _itemlist = new ObservableList<T>();
        }

        public delegate void ContainerChangedEventHandler(object sender);
        public event ContainerChangedEventHandler ContainerChanged;

        protected virtual void OnContainerChanged()
        {
            ContainerChanged?.Invoke(this);
        }

        #region properties
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Container name can't be null or empty");
                }
                _name = value;
                RaisePropertyChanged("Name");
                OnContainerChanged();
            }

        }

        public virtual bool IsEmpty
        {
            get
            {
                if (Itemlist.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ObservableList<T> Itemlist => _itemlist;
        #endregion
        public virtual bool Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("You can't add Null to container");

            if (!_itemlist.Contains(item))
            {
                _itemlist.Add(item);
                RaisePropertyChanged("Itemlist");
                OnContainerChanged();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds all (NON DUPLICATE) items to the container.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>Number of items that were actually added.</returns>
        public virtual int AddRange(IEnumerable<T> items)
        {
            List<T> originalItems = new List<T>();
            foreach(T item in items)
            {
                if (!_itemlist.Contains(item))
                {
                    originalItems.Add(item);
                }
            }
            _itemlist.AddRange(originalItems);
            if(originalItems.Count > 0)
            {
                RaisePropertyChanged("Itemlist");
                OnContainerChanged();
            }
            return originalItems.Count;
        }
        public virtual bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException("You can't remove Null from container");

            if(_itemlist.Remove(item))
            {                
                RaisePropertyChanged("Itemlist");
                OnContainerChanged();
                return true;
            }           
            return false;
        }

        #region propertychanged
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
