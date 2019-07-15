using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class FolderPageViewModel : ViewModelBase
    {

        public RelayCommand DeleteSelected { get; set; }
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand SelectedItemChanged { get; set; }
        public RelayCommand FolderDoubleClicked { get; set; }
        public RelayCommand TrackDoubleClicked { get; set; }

        public Track SelectedTrack { get; set; }
        public Folder SelectedFolder { get; set; }

        //constructor
        public FolderPageViewModel()
        {
            DeleteSelected = new RelayCommand(_deleteSelected);
            PlayTrack = new RelayCommand(_playTrack);
            SelectedItemChanged = new RelayCommand(_selectedItemChanged);
            FolderDoubleClicked = new RelayCommand(_folderDoubleClicked);
            TrackDoubleClicked = new RelayCommand(_trackDoubleClicked);

            //events
            _dataModel.DataModelChanged += OnRootlistChanged;
        }
        //method gets called whenever rootfolders in the datamodel change.
        private void OnRootlistChanged(object source,object obj)
        {
            RaisePropertyChanged("RootFolders");
        }

        private void _deleteSelected(object argument)
        {
            if (SelectedFolder != null)
            {
                _dataModel.Delete(SelectedFolder);
                SelectedFolder = null;
            }
            else if(SelectedTrack != null)
            {
                List<Track> selectedTracks = new List<Track>();
                selectedTracks.Add(SelectedTrack);
                _dataModel.Delete(selectedTracks.ToList<Track>());
            }                    
        }
        /// <summary>
        /// changes the selected items in the datamodel.
        /// </summary>
        /// <param name="argument"></param>
        private void _selectedItemChanged(object argument)
        {
            if (argument != null)
            {
                if (argument.GetType() == typeof(Folder))
                {
                    SelectedTrack = null;
                    SelectedFolder = (Folder)argument;
                }
                else
                {                  
                    SelectedFolder = null;
                    SelectedTrack = ((Track)argument);
                }
            }
        }

        private void _playTrack(object argument)
        {
            if (SelectedTrack != null)
            {
                ObservableList<Track> templist = new ObservableList<Track>();
                templist.Add(SelectedTrack);
                TrackQueue = templist;
            }                
        }

        private void _folderDoubleClicked(object argument)
        {
            throw new NotImplementedException();
        }

        private void _trackDoubleClicked(object argument)
        {
            throw new NotImplementedException();
        }
    }
}
