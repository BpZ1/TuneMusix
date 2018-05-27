using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class VisualOptionsViewModel
    {
        public IEnumerable<Swatch> Swatches { get; }

        public RelayCommand ApplyPrimaryColor { get; set; }
        public RelayCommand ApplyAccentColor { get; set; }

        public VisualOptionsViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;

            ApplyPrimaryColor = new RelayCommand(applyPrimaryColor);
            ApplyAccentColor = new RelayCommand(applyAccentColor);
        }        

        private void applyPrimaryColor(object argument)
        {
            var swatch = argument as Swatch;
            if (swatch == null)
                throw new ArgumentNullException();

            Options.Instance.PrimaryColor = swatch;
        }

        private void applyAccentColor(object argument)
        {
            var swatch = argument as Swatch;
            if (swatch == null)
                throw new ArgumentNullException();

            Options.Instance.AccentColor = swatch;
        }

        public bool ChangeTheme
        {
            get { return Options.Instance.Theme; }
            set { Options.Instance.Theme = value; }
        }
    }
}
