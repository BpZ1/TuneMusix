using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class TracklistViewModel : ViewModelBase
    {

        DataModel dataModel = DataModel.Instance;

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }

        //Constructor
        public TracklistViewModel()
        {
            DeleteSelectedTracks = new RelayCommand(deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);
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
        private void addToPlaylist(object argument)
        {
            if (SelectedPlaylist != null)
            {
                SelectedPlaylist.AddAllTracks(SelectedTracks.ToList<Track>());
                RaisePropertyChanged("SelectedPlayList");
            }
        }

        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedTracks.Clear();
            dataModel.SelectedFolder = null;
            foreach (Track track in listView.SelectedItems)
            {
                dataModel.SelectedFolder = null;
                SelectedTracks.Add(track);
            }
        }

        private void playTrack(object argument)
        {
            if (SelectedTracks.Count > 0)
            { 
                TrackQueue = SelectedTracks.ToList<Track>();
            }
        }

    }
}
