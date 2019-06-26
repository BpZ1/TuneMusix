using System.Collections.ObjectModel;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class AlbumPageViewModel : ViewModelBase
    {
        public ObservableCollection<Album> AlbumList
        {
            get
            {
                return DataModel.Instance.Albumlist;
            }
        }


    }
}
