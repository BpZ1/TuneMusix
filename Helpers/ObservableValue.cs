using System;

namespace TuneMusix.Helpers
{
    public class ObservableValue<T> : NotifyPropertyChangedBase
    {

        public delegate void ObservableValueEventHandler();

        public event ObservableValueEventHandler OnValueSet;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnValueSet?.Invoke();
                NotifyPropertyChanged();
            }
        }

        public ObservableValue() { }

        public ObservableValue( Action actionOnSet )
        {
            OnValueSet += () =>
            {
                actionOnSet.Invoke();
            };
        }

        public ObservableValue( T value )
        {
            _value = value;
        }

        public ObservableValue( T value, Action actionOnSet ) : this( actionOnSet )
        {
            _value = value;
        }

        private T _value;
    }
}
