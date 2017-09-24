using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;

namespace TuneMusix.ViewModel.Dialog
{
    class AddPlaylistViewModel : ViewModelBase
    {
        private string textBoxText;

        public AddPlaylistViewModel()
        {
            TextBoxText = "";
        }

        public string TextBoxText
        {
            get
            {
                return this.textBoxText;
            }
            set
            {
                this.textBoxText = value;
                RaisePropertyChanged("TextBoxText");
            }
        }
    }
}
