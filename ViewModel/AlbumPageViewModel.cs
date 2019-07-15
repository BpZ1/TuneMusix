using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.Util;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class AlbumPageViewModel : ViewModelBase
    {
        public ObservableList<Album> AlbumList => DataModel.Instance.Albumlist;
        public ObservableList<Album> SelectedAlbums { get; set; } = new ObservableList<Album>();
        public ObservableList<Track> SelectedTracks { get; set; } = new ObservableList<Track>();
        public RelayCommand AlbumSelectionChanged { get; set; }
        public RelayCommand TrackSelectionChanged { get; set; }
        public RelayCommand PlayButtonPushed { get; set; }
        public RelayCommand ContextMenuPlayAlbum { get; set; }
        public RelayCommand ContextMenuDeleteAlbum { get; set; }
        public RelayCommand ContextMenuAddAlbumToPlaylist { get; set; }
        public RelayCommand ContextMenuAddAlbumToQueue { get; set; }
        public RelayCommand ContextMenuPlayTracks { get; set; }
        public RelayCommand ContextMenuAddTracksToPlaylist { get; set; }
        public RelayCommand ContextMenuDeleteTracks { get; set; }
        public RelayCommand AlbumTrackDoubleClick { get; set; }

        public AlbumPageViewModel()
        {
            AlbumSelectionChanged = new RelayCommand(_albumSelectionChanged);
            TrackSelectionChanged = new RelayCommand(_trackSelectionChanged);
            ContextMenuPlayAlbum = new RelayCommand(_contextMenuPlayAlbum);
            PlayButtonPushed = new RelayCommand(_playButtonPushed);
            ContextMenuDeleteAlbum = new RelayCommand(_contextMenuDeleteAlbum);
            ContextMenuAddAlbumToPlaylist = new RelayCommand(_contextMenuAddAlbumToPlaylist);
            ContextMenuAddAlbumToQueue = new RelayCommand(_contextMenuAddAlbumToQueue);
            ContextMenuPlayTracks = new RelayCommand(_contextMenuPlayTracks);
            ContextMenuAddTracksToPlaylist = new RelayCommand(_contextMenuAddTracksToPlaylist);
            ContextMenuDeleteTracks = new RelayCommand(_contextMenuDeleteTracks);
            AlbumTrackDoubleClick = new RelayCommand(_albumTrackDoubleClick);

            DataModel.Instance.AlbumlistChanged += OnAlbumlistChanged;
        }

        private void OnAlbumlistChanged(object sender, object argument)
        {
            RaisePropertyChanged("AlbumList");
        }
        private void _albumTrackDoubleClick(object argument)
        {
            ListView listView = argument as ListView;
            if (listView == null) return;

            Track track = listView.SelectedItem as Track;
            if (track != null)
            {
                _dataModel.TrackQueue.CurrentTrack = track;
            }
        }

        private void _contextMenuDeleteTracks(object argument)
        {
            if (SelectedTracks.Count > 0)
            {
                DialogResult result = DialogService.OpenDialog("Are you sure you want to delete "
                    + SelectedTracks.Count + " tracks?");

                if (result == DialogResult.Yes)
                {
                    List<Track> toBeDeletedTracks = new List<Track>(SelectedTracks);

                    _dataModel.Delete(toBeDeletedTracks);
                }
            }
            else
            {
                DialogService.NotificationMessage("No track was selected.");
            }
        }
        private void _contextMenuAddTracksToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;
            if (selectedPlaylist == null) return;

            selectedPlaylist.AddRange(SelectedTracks);
        }

        private void _contextMenuPlayTracks(object argument)
        {
            TrackQueue = SelectedTracks;
        }

        private void _albumSelectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedAlbums.Clear();
            List<Album> currentSelection = new List<Album>();
            foreach (Album album in listView.SelectedItems)
            {
                currentSelection.Add(album);
            }
            SelectedAlbums.AddRange(currentSelection);
        }

        private void _trackSelectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedTracks.Clear();
            List<Track> currentSelection = new List<Track>();
            foreach (Track track in listView.SelectedItems)
            {
                currentSelection.Add(track);
            }
            SelectedTracks.AddRange(currentSelection);
        }

        private void _playButtonPushed(object argument)
        {
            Album album = argument as Album;
            if(album != null)
            {
                TrackQueue = new ObservableList<Track>(album.Itemlist);
            }
        }

        private void _contextMenuPlayAlbum(object argument)
        {
            if(SelectedAlbums.Count > 0)
            {
                List<Track> trackList = new List<Track>();
                foreach(Album album in SelectedAlbums)
                {
                    trackList.AddRange(album.Itemlist);
                }
                CurrentPlaylist = null;
                TrackQueue = new ObservableList<Track>(trackList);
            }
        }

        private void _contextMenuDeleteAlbum(object argument)
        {
            if(SelectedAlbums.Count > 0)
            {
                DialogResult result = DialogService.OpenDialog("Are you sure you want to delete " 
                    + SelectedAlbums.Count + " albums?");

                if(result == DialogResult.Yes)
                {
                    List<Album> toBeDeletedAlbums = new List<Album>(SelectedAlbums);

                    foreach(Album album in toBeDeletedAlbums)
                    {
                        DataModel.Instance.Delete(album);
                    }
                }
            }
            else
            {
                DialogService.NotificationMessage("No album was selected.");
            }
        }

        private void _contextMenuAddAlbumToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;
            if(selectedPlaylist != null)
            {
                if(SelectedAlbums.Count > 0)
                {
                    //Add the tracks of each album to the playlist
                    foreach(Album album in SelectedAlbums)
                    {
                        selectedPlaylist.AddRange(album.Itemlist);
                    }
                }
                else
                {
                    DialogService.NotificationMessage("No album was selected.");
                }
            }
        }

        private void _contextMenuAddAlbumToQueue(object argument)
        {
            if (SelectedAlbums.Count > 0)
            {
                //Add the tracks of each album to the queue
                foreach (Album album in SelectedAlbums)
                {
                    TrackQueue.AddRange(album.Itemlist);
                }
                RaisePropertyChanged("TrackQueue");
            }
            else
            {
                DialogService.NotificationMessage("No album was selected.");
            }
        }
    }
}
