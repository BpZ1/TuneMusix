using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
