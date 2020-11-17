using System.ComponentModel;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class GeneralOptionsViewModel : INotifyPropertyChanged
    {
        private Options options = Options.Instance;

        public bool LoggerActive
        {
            get { return options.LoggerActive; }
            set
            {
                options.LoggerActive = value;
                RaisePropertyChanged("LoggerActive");
            }
        }

        public bool AskConfirmation
        {
            get { return options.AskConfirmation; }
            set
            {
                options.AskConfirmation = value;
                RaisePropertyChanged("AskConfirmation");
            }
        }

        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
