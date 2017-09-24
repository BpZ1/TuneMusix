using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;

namespace TuneMusix.ViewModel.Dialog
{
    class GetTextViewModel : ViewModelBase
    {
        private string _textBoxText;
        private string _header;

        public GetTextViewModel(string header)
        {
            TextBoxText = "";
            Header = header;
        }

        public string TextBoxText
        {
            get
            {
                return this._textBoxText;
            }
            set
            {
                this._textBoxText = value;
                RaisePropertyChanged("TextBoxText");
            }
        }

        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                RaisePropertyChanged("Header");
            }
        }
    }
}
