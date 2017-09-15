using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class FolderPageViewModel : ViewModelBase
    {

        DataModel dataModel;

        //constructor
        public FolderPageViewModel()
        {
            dataModel = DataModel.Instance;
            dataModel.DataModelChanged += OnRootlistChanged;
        }

        public void OnRootlistChanged(object source,object obj)
        {
            RaisePropertyChanged("RootFolders");
        }


    }
}
