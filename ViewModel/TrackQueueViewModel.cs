using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{

    class TrackQueueViewModel : ViewModelBase
    {
        private Track selectedTrack { get; set; }

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand SelectionChanged { get; set; }

        public TrackQueueViewModel()
        {
            DeleteSelectedTracks = new RelayCommand(deleteSelectedTrack);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);
            dataModel.TrackQueueChanged += onTrackQueueChanged;
        }
        /// <summary>
        /// Changed the current track to the selected track.
        /// </summary>
        /// <param name="argument"></param>
        private void playTrack(object argument)
        {
            if(selectedTrack != null)
            {
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(selectedTrack);
                dataModel.CurrentTrack = selectedTrack;
            }     
        }
        /// <summary>
        /// Changes the selected track.
        /// </summary>
        /// <param name="argument"></param>
        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if(listView != null)
            {
                selectedTrack = listView.SelectedItem as Track;
            }
        }
        /// <summary>
        /// Adds the selected track to a playlist
        /// </summary>
        /// <param name="obj"></param>
        private void addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                dataModel.AddTracksToPlaylist(new List<Track>() { selectedTrack }, selectedPlaylist);
            }
        }

        private void deleteSelectedTrack(object argument)
        {
            if(selectedTrack != null)
            {
                dataModel.RemoveTrackFromQueue(selectedTrack);
                RaisePropertyChanged("CurrentTrackQueue");
            }
        }

        public List<Track> CurrentTrackQueue
        {
            get { return dataModel.TrackQueue.ToList<Track>(); }
        }

        public Brush HighlightColor
        {
            get
            {
                var palette = new PaletteHelper().QueryPalette();
                var hue = palette.AccentSwatch.AccentHues.ToArray()[palette.AccentHueIndex];
                return new SolidColorBrush(hue.Color);
            }
        }

        private void onTrackQueueChanged(object source, object argument)
        {
            RaisePropertyChanged("CurrentTrackQueue");
        }
    }
}
