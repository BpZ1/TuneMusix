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
        AudioControls audio = AudioControls.Instance;

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }

        //Constructor
        public TracklistViewModel()
        {
            DeleteSelectedTracks = new RelayCommand(_deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            SelectionChanged = new RelayCommand(_selectionChanged);
            PlayTrack = new RelayCommand(_playTrack);
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
        public void _deleteSelectedTracks(object argument)
        {
            dataModel.Delete(SelectedTracks.ToList<Track>());      
        }

        /// <summary>
        /// Adds all selected tracks to the playlist
        /// </summary>
        /// <param name="parameter"></param>
        private void _addToPlaylist(object argument)
        {
            if (SelectedPlaylist != null)
            {
                SelectedPlaylist.AddAllTracks(SelectedTracks.ToList<Track>());
                RaisePropertyChanged("SelectedPlayList");
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

        private void _playTrack(object argument)
        {
            if (SelectedTracks.Count > 0)
            { 
                TrackQueue = SelectedTracks.ToList<Track>();
            }
            Track Playtrack = SelectedTracks.First();
            if (Playtrack == null){ return; }
            if (dataModel.CurrentTrack == Playtrack) { return; }
            else
            {
                Debug.WriteLine("Playing Selected Track(s)");
                dataModel.CurrentTrack = Playtrack;
            }
        }

    }
}
