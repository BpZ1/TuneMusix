using TuneMusix.Data;

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
