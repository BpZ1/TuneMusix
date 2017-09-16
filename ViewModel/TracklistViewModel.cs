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
    class TracklistViewModel : ViewModelBase
    {

        DataModel dataModel = DataModel.Instance;

        //Relaycommands

        public RelayCommand DeleteSelectedTracks { get; set; }

        //Method for adding selected tracks to playlist
        public RelayCommand AddToPlaylist { get; set; }


        //Constructor
        public TracklistViewModel()
        {
            DeleteSelectedTracks = new RelayCommand(deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            dataModel.DataModelChanged += OnTrackListChanged;
        }

        public void OnTrackListChanged(object source,object obj)
        {
            RaisePropertyChanged("TrackList");
        }

        /// <summary>
        /// Deletes all selected tracks from the tracklist.
        /// </summary>
        /// <param name="argument"></param>
        public void deleteSelectedTracks(object argument)
        {
            dataModel.Delete(SelectedTracks.ToList<Track>());      
        }

        /// <summary>
        /// Adds all selected tracks to the playlist
        /// </summary>
        /// <param name="parameter"></param>
        private void _addToPlaylist(object parameter)
        {
            if (SelectedPlaylist != null)
            {
                SelectedPlaylist.AddAllTracks(SelectedTracks.ToList<Track>());
                RaisePropertyChanged("SelectedPlayList");
            }
        }


    }
}
