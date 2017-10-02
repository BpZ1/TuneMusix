using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.OptionsViewModel
{
    class GeneralOptionsViewModel
    {
        private Options options = Options.Instance;
        public bool LoggerActive
        {
            get { return options.LoggerActive; }
            set
            {
                options.LoggerActive = value;
                options.IsModified = true;
            }
        }
    }
}
