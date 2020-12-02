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

        public virtual bool IsEmpty
        {
            get
            {
                return Itemlist.Count == 0;
            }
        }

        public ObservableList<T> Itemlist => _itemlist;

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

        public override bool Equals( object obj )
        {
            if ( !( obj is T objT ) )
            {
                return false;
            }
            if ( obj is ItemContainer<T> container )
            {
                return Name.Equals( container );
            }
            return base.Equals( obj );
        }

        public override int GetHashCode()
        {
            var hashCode = 1978926223;
            hashCode = hashCode * -1521134295 + Name.Value.GetHashCode(); ;
            hashCode = hashCode * -1521134295 + _itemlist.GetHashCode();
            return hashCode;
        }
    }
}
