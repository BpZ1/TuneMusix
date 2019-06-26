using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
