using System;
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

        public RelayCommand DeleteSelected { get; set; }
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand SelectedItemChanged { get; set; }

        //constructor
        public FolderPageViewModel()
        {
            dataModel = DataModel.Instance;

            DeleteSelected = new RelayCommand(deleteSelected);
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

        private void deleteSelected(object argument)
        {
            if (dataModel.SelectedFolder != null)
            {
                dataModel.Delete(dataModel.SelectedFolder);
                dataModel.SelectedFolder = null;
            }
            else if(dataModel.SelectedTracks != null)
            {
                dataModel.Delete(SelectedTracks.ToList<Track>());
            }                    
        }

        private void selectedItemChanged(object argument)
        {
            if (argument != null)
            {
                if (argument.GetType() == typeof(Folder))
                {
                    SelectedTracks.Clear();
                    dataModel.SelectedFolder = (Folder)argument;
                }
                else
                {
                    SelectedTracks.Clear();
                    dataModel.SelectedFolder = null;
                    SelectedTracks.Add((Track)argument);
                }
            }
            if (argument == null)
            {
                Console.WriteLine("Argument is null");
            }
        }

        private void playTrack(object argument)
        {
            if (dataModel.SelectedTracks != null)
            {
                if(dataModel.SelectedTracks.Count > 0)
                {
                    dataModel.TrackQueue = dataModel.SelectedTracks.ToList<Track>();
                }
            }                
        }


    }
}
