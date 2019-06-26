using System;
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

        public delegate void ContainerChangedEventHandler(object source);

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
        public ObservableList<T> Itemlist
        {
            get { return _itemlist; }
        }
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
