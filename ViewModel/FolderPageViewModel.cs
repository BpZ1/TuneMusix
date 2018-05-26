using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
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
            DeleteSelected = new RelayCommand(deleteSelected);
            PlayTrack = new RelayCommand(playTrack);
            SelectedItemChanged = new RelayCommand(selectedItemChanged);
            FolderDoubleClicked = new RelayCommand(folderDoubleClicked);
            TrackDoubleClicked = new RelayCommand(trackDoubleClicked);

            //events
            dataModel.DataModelChanged += onRootlistChanged;
        }
        //method gets called whenever rootfolders in the datamodel change.
        private void onRootlistChanged(object source,object obj)
        {
            RaisePropertyChanged("RootFolders");
        }

        private void deleteSelected(object argument)
        {
            if (SelectedFolder != null)
            {
                dataModel.Delete(SelectedFolder);
                SelectedFolder = null;
            }
            else if(SelectedTrack != null)
            {
                List<Track> selectedTracks = new List<Track>();
                selectedTracks.Add(SelectedTrack);
                dataModel.Delete(selectedTracks.ToList<Track>());
            }                    
        }
        /// <summary>
        /// changes the selected items in the datamodel.
        /// </summary>
        /// <param name="argument"></param>
        private void selectedItemChanged(object argument)
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

        private void playTrack(object argument)
        {
            if (SelectedTrack != null)
            {
                List<Track> templist = new List<Track>();
                templist.Add(SelectedTrack);
                TrackQueue = templist;
            }                
        }

        private void folderDoubleClicked(object argument)
        {
            throw new NotImplementedException();
        }

        private void trackDoubleClicked(object argument)
        {
            throw new NotImplementedException();
        }
    }
}
