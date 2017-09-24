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

        DataModel dataModel;

        public RelayCommand DeleteSelected { get; set; }
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand SelectedItemChanged { get; set; }

        public Track SelectedTrack { get; set; }
        public Folder SelectedFolder { get; set; }

        //constructor
        public FolderPageViewModel()
        {
            dataModel = DataModel.Instance;

            DeleteSelected = new RelayCommand(_deleteSelected);
            PlayTrack = new RelayCommand(_playTrack);
            SelectedItemChanged = new RelayCommand(_selectedItemChanged);

            //events
            dataModel.DataModelChanged += _onRootlistChanged;
        }
        //method gets called whenever rootfolders in the datamodel change.
        public void _onRootlistChanged(object source,object obj)
        {
            RaisePropertyChanged("RootFolders");
        }

        private void _deleteSelected(object argument)
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
                List<Track> templist = new List<Track>();
                templist.Add(SelectedTrack);
                TrackQueue = templist;
            }                
        }


    }
}
