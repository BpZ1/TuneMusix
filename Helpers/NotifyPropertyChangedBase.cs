using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuneMusix.Helpers
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged( [CallerMemberName] String propertyName = "" )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
    }
}
