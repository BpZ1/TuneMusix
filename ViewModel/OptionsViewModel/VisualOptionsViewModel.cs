using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.OptionsViewModel
{
    class VisualOptionsViewModel
    {
        public IEnumerable<Swatch> Swatches { get; }

        public RelayCommand ApplyPrimaryColor { get; set; }
        public RelayCommand ApplyAccentColor { get; set; }
        public RelayCommand ChangeTheme { get; }

        public VisualOptionsViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;

            ApplyPrimaryColor = new RelayCommand(applyPrimaryColor);
            ApplyAccentColor = new RelayCommand(applyAccentColor);
            ChangeTheme = new RelayCommand(o => changeTheme((bool)o));
        }        

        private void applyPrimaryColor(object argument)
        {
            var swatch = argument as Swatch;
            if (swatch == null)
                throw new ArgumentNullException();

            Options.Instance.SetPrimaryColor = swatch;
        }

        private void applyAccentColor(object argument)
        {
            var swatch = argument as Swatch;
            if (swatch == null)
                throw new ArgumentNullException();

            Options.Instance.SetAccentColor = swatch;
        }

        private void changeTheme(bool isDark)
        {
            Options.Instance.SetTheme = isDark;
        }
    }
}
