using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Util;

namespace TuneMusix.Model
{
    /// <summary>
    /// Base class for Classes containing a list of tracks.
    /// </summary>
    public abstract class ItemContainer<T> : NotifyPropertyChangedBase
    {
        public ObservableValue<string> Name = new ObservableValue<string>( string.Empty );
        protected ObservableList<T> _itemlist;

        public ItemContainer( string name )
        {
            Name.Value = name;
            _itemlist = new ObservableList<T>();
        }

        public delegate void ContainerChangedEventHandler( object sender );
        public event ContainerChangedEventHandler ContainerChanged;

        protected virtual void OnContainerChanged()
        {
            ContainerChanged?.Invoke( this );
        }

        #region properties

        public virtual bool IsEmpty
        {
            get
            {
                if ( Itemlist.Count == 0 )
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
        public virtual bool Add( T item )
        {
            if ( item == null )
                throw new ArgumentNullException( "You can't add Null to container" );

            if ( !_itemlist.Contains( item ) )
            {
                _itemlist.Add( item );
                NotifyPropertyChanged( "Itemlist" );
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
        public virtual int AddRange( IEnumerable<T> items )
        {
            List<T> originalItems = new List<T>();
            foreach ( T item in items )
            {
                if ( !_itemlist.Contains( item ) )
                {
                    originalItems.Add( item );
                }
            }
            _itemlist.AddRange( originalItems );
            if ( originalItems.Count > 0 )
            {
                NotifyPropertyChanged( "Itemlist" );
                OnContainerChanged();
            }
            return originalItems.Count;
        }
        public virtual bool Remove( T item )
        {
            if ( item == null )
                throw new ArgumentNullException( "You can't remove Null from container" );

            if ( _itemlist.Remove( item ) )
            {
                NotifyPropertyChanged( "Itemlist" );
                OnContainerChanged();
                return true;
            }
            return false;
        }
    }
}
