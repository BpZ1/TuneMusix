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


            DeleteSelectedTracks = new RelayCommand(deleteSelectedTracks);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);

            //events
            dataModel.DataModelChanged += OnTrackListChanged;

        }

        public void OnTrackListChanged(object source,object obj)
        {
            RaisePropertyChanged("TrackList");
        }

        public void playTrack(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            { 
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
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            var textBox = argument as TextBox;
            string idstring = textBox.Text;
            long? id = long.Parse(idstring);
            if (id != null)
            {
                Console.WriteLine("----------------------------------- ID: " + id);
                foreach (Playlist playlist in Playlists)
                {
                    if (id == playlist.ID)
                    {
                        dataModel.AddTracksToPlaylist(SelectedTracks.ToList<Track>(), playlist);
                    }
                }
            }
        }

        public void selectionChanged(object argument)
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
    }
}
