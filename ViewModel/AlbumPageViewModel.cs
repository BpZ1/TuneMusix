using MaterialDesignThemes.Wpf;
using System;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class AlbumPageViewModel : ViewModelBase
    {
        public ObservableList<Album> AlbumList => DataModel.Instance.Albumlist;

        public RelayCommand OnClick { get; set; }

        public AlbumPageViewModel()
        {
            OnClick = new RelayCommand(_onCLick);
        }

        private void _onCLick(object sender)
        {
            Flipper flipper = sender as Flipper;
            if (flipper == null) return;

            if (flipper.IsFlipped)
            {
                flipper.IsFlipped = false;
            }
            else
            {
                flipper.IsFlipped = true;
            }
        }

    }
}
