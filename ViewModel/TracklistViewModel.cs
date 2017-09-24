using System;
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

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public ObservableCollection<Track> SelectedTracks { get; set; }

        //Constructor
        public TracklistViewModel()
        {
            SelectedTracks = new ObservableCollection<Track>();


            DeleteSelectedTracks = new RelayCommand(_deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            SelectionChanged = new RelayCommand(_selectionChanged);
            PlayTrack = new RelayCommand(_playTrack);

            //events
            dataModel.DataModelChanged += OnTrackListChanged;

        }

        public void OnTrackListChanged(object source,object obj)
        {
            RaisePropertyChanged("TrackList");
        }

        private void _playTrack(object argument)
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
        private void _deleteSelectedTracks(object argument)
        {
            dataModel.Delete(SelectedTracks.ToList<Track>());
        }

        /// <summary>
        /// Adds all selected tracks to the playlist with the given id.
        /// </summary>
        /// <param name="argument"></param>
        private void _addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                dataModel.AddTracksToPlaylist(SelectedTracks.ToList<Track>(),selectedPlaylist);
            }        
        }

        private void _selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedTracks.Clear();
            foreach (Track track in listView.SelectedItems)
            {
                SelectedTracks.Add(track);
            }
        }



    }
}
