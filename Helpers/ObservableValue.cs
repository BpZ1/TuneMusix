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

        public override bool Equals( object obj )
        {
            if ( !( obj is T objectT ) )
            {
                return false;
            }
            if ( obj == null )
            {
                if ( _value == null )
                {
                    return true;
                }
                return false;
            }
            return objectT.Equals( _value );
        }

        public override string ToString()
        {
            return _value?.ToString();
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
