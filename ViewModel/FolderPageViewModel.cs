using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class FolderPageViewModel : ViewModelBase
    {

        DataModel dataModel;

        public RelayCommand Delete;
        public RelayCommand PlayTrack;
        public RelayCommand SelectedItemChanged;

        //constructor
        public FolderPageViewModel()
        {
            dataModel = DataModel.Instance;

            Delete = new RelayCommand(delete);
            PlayTrack = new RelayCommand(playTrack);
            SelectedItemChanged = new RelayCommand(selectedItemChanged);

            //events
            dataModel.DataModelChanged += OnRootlistChanged;
        }
        //method gets called whenever rootfolders in the datamodel change.
        public void OnRootlistChanged(object source,object obj)
        {
            RaisePropertyChanged("RootFolders");
        }

        private void delete(object argument)
        {
            if (dataModel.SelectedFolder != null)
            {
                dataModel.Delete(dataModel.SelectedFolder);
                dataModel.SelectedFolder = null;
            }
            if (SelectedTracks.Count > 0)
            {
                dataModel.Delete(SelectedTracks.ToList<Track>());
            }
        }

        private void selectedItemChanged(object argument)
        {
            var FolderTree = argument as TreeView;
            if (FolderTree != null)
            {
                var selectedItem = FolderTree.SelectedItem;
                if (selectedItem.GetType() == typeof(Folder))
                {
                    SelectedTracks.Clear();
                    dataModel.SelectedFolder = (Folder)selectedItem;
                }
                else
                {
                    SelectedTracks.Clear();
                    dataModel.SelectedFolder = null;
                    SelectedTracks.Add((Track)selectedItem);
                }
                
            }
        }

        private void playTrack(object argument)
        {

        }
    }
}
