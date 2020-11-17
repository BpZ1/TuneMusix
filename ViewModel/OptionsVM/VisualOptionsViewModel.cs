using MaterialDesignColors;
using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class VisualOptionsViewModel
    {
        public IEnumerable<Swatch> Swatches { get; }

        public RelayCommand ApplyPrimaryColorCommand { get; set; }
        public RelayCommand ApplyAccentColorCommand { get; set; }

        public VisualOptionsViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;

            ApplyPrimaryColorCommand = new RelayCommand(ApplyPrimaryColor);
            ApplyAccentColorCommand = new RelayCommand(ApplyAccentColor);
        }        

        private void ApplyPrimaryColor(object argument)
        {
            var swatch = argument as Swatch;
            if (swatch == null)
                throw new ArgumentNullException();

            Options.Instance.PrimaryColor = swatch;
        }

        private void ApplyAccentColor(object argument)
        {
            var swatch = argument as Swatch;
            Options.Instance.AccentColor = swatch;
        }

        public bool ChangeTheme
        {
            get { return Options.Instance.Theme; }
            set { Options.Instance.Theme = value; }
        }
    }
}
