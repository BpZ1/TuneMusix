using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class TracklistViewModel : ViewModelBase
    {

        DataModel dataModel = DataModel.Instance;

        public ObservableCollection<Track> SelectedTracks { get; set; }
        private string searchText;

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SearchChanged { get; set; }
        public RelayCommand DeleteSearch { get; set; }


        //Constructor
        public TracklistViewModel()
        {
            searchText = "";
            SelectedTracks = new ObservableCollection<Track>();


            DeleteSelectedTracks = new RelayCommand(deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);
            SearchChanged = new RelayCommand(searchChanged);
            DeleteSearch = new RelayCommand(deleteSearch);

            //events
            dataModel.DataModelChanged += OnTrackListChanged;

        }

        public List<Track> FilteredTracks
        {
            get
            {
                if (!searchText.Equals(""))
                {
                    IEnumerable<Track> result = TrackList.Where(t => TrackService.Contains(t, searchText, false));
                    return result.ToList<Track>();
                }
                else
                {
                    return TrackList.ToList<Track>();
                }
            
            }
        }

        #region methods
        public void OnTrackListChanged(object source,object obj)
        {
            RaisePropertyChanged("FilteredTracks");
        }

        private void playTrack(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            {
                CurrentPlaylist = null;
                TrackQueue = SelectedTracks.ToList<Track>();
            }
        }


        /// <summary>
        /// Deletes all selected tracks from the tracklist.
        /// </summary>
        /// <param name="argument"></param>
        private void deleteSelectedTracks(object argument)
        {
            dataModel.Delete(SelectedTracks.ToList<Track>());
        }

        /// <summary>
        /// Adds all selected tracks to the playlist with the given id.
        /// </summary>
        /// <param name="argument"></param>
        private void addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                dataModel.AddTracksToPlaylist(SelectedTracks.ToList<Track>(),selectedPlaylist);
            }        
        }

        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedTracks.Clear();
            foreach (Track track in listView.SelectedItems)
            {
                SelectedTracks.Add(track);
            }
        }

        //Called every time the text in the textbox changes
        private void searchChanged(object argument)
        {
            if (((string)argument).Equals(searchText))
                return;

            searchText = (string)argument;
            RaisePropertyChanged("FilteredTracks");
        }

        //called if the button for the deletion of the textbox is clicked
        private void deleteSearch(object argument)
        {
            if (searchText.Equals(""))
                return;

            searchText = "";
            RaisePropertyChanged("FilteredTracks");
        }
        #endregion
    }
}
