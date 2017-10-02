using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.OptionsViewModel
{
    class OptionsOverviewViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand Apply { get; set; }
        public RelayCommand Cancel { get; set; }

        public OptionsOverviewViewModel()
        {
            Apply = new RelayCommand(_apply);
            Cancel = new RelayCommand(_cancel);
        }

        private void _apply(object argument)
        {
            if (options.IsModified)
            {
                //save data.
            }
        }

        private void _cancel(object argument)
        {
            if (options.IsModified)
            {
                //open window to ask if sure.
            }
        }
    }
}
