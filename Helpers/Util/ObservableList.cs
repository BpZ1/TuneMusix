using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TuneMusix.Helpers.Util
{
    /// <summary>
    /// Extention of <see cref="ObservableCollection{T}"/> that adds methods
    /// for adding and removing ranges of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableList<T> : ObservableCollection<T>
    {

        public ObservableList() : base() { }

        public ObservableList(IEnumerable<T> items) : base(items) { }


        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Items.Add(item);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Items.Remove(item);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
