
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace TuneMusix.Helpers.Util
{
    class ObservableHashSet<T> : HashSet<T>, INotifyPropertyChanged, INotifyCollectionChanged, ISet<T>
    {

        public ObservableHashSet() : base()
        {
        }

        public ObservableHashSet(IEnumerable<T> collection) : base(collection)
        {
        }

        public ObservableHashSet(IEqualityComparer<T> comparer) : base(comparer)
        {
        }

        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : base(collection, comparer)
        {
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionAdd()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        private void OnCollectionRemove()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public new bool Add(T item)
        {
            Monitor.Enter(this);
            if (base.Add(item))
            {
                OnCollectionAdd();
                OnPropertyChanged("Count");
                Monitor.Exit(this);
                return true;
            }
            Monitor.Exit(this);
            return false;
        }

        public int AddRange(IEnumerable<T> items)
        {
            int counter = 0;
            Monitor.Enter(this);
            foreach(T item in items)
            {
                if (base.Add(item))
                {
                    counter++;
                }
            }
            if(counter > 0)
            {
                OnCollectionAdd();
                OnPropertyChanged("Count");
            }
            Monitor.Exit(this);
            return counter;
        }

        public new bool Remove(T item)
        {
            Monitor.Enter(this);
            if (base.Remove(item))
            {
                OnCollectionRemove();
                OnPropertyChanged("Count");
                Monitor.Exit(this);
                return true;
            }
            Monitor.Exit(this);
            return false;
        }

        public int RemoveRange(IEnumerable<T> items)
        {
            int counter = 0;
            Monitor.Enter(this);
            foreach (T item in items)
            {
                if (base.Remove(item))
                {
                    counter++;
                }
            }
            if (counter > 0)
            {
                OnCollectionRemove();
                OnPropertyChanged("Count");
            }
            Monitor.Exit(this);
            return counter;
        }

        public new int RemoveWhere(Predicate<T> predicate)
        {
            Monitor.Enter(this);
            int counter = base.RemoveWhere(predicate);
            if(counter > 0)
            {
                OnCollectionRemove();
                OnPropertyChanged("Count");
            }
            Monitor.Exit(this);
            return counter;
        }
    }
}
