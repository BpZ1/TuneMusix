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

        public List<Album> SelectedAlbums { get; set; }
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand PlayAlbum { get; set; }
        public RelayCommand DeleteAlbum { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand AddAlbumToQueue { get; set; }

        public AlbumPageViewModel()
        {
            SelectedAlbums = new List<Album>();
            SelectionChanged = new RelayCommand(_selectionChanged);
            PlayAlbum = new RelayCommand(_playAlbum);
            DeleteAlbum = new RelayCommand(_deleteAlbum);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            AddAlbumToQueue = new RelayCommand(_addAlbumToQueue);
        }

        private void _selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView == null) return;

            SelectedAlbums.Clear();
            List<Album> currentSelection = new List<Album>();
            foreach (Album track in listView.SelectedItems)
            {
                currentSelection.Add(track);
            }
            SelectedAlbums.AddRange(currentSelection);
        }

        private void _playAlbum(object argument)
        {
            if(SelectedAlbums.Count > 0)
            {
                List<Track> trackList = new List<Track>();
                foreach(Album album in SelectedAlbums)
                {
                    trackList.AddRange(album.Itemlist);
                }
                CurrentPlaylist = null;
                DataModel.Instance.TrackQueue = new ObservableList<Track>(trackList);
            }
        }

        private void _deleteAlbum(object argument)
        {
            if(SelectedAlbums.Count > 0)
            {
                DialogResult result = DialogService.OpenDialog("Are you sure you want to delete " 
                    + SelectedAlbums.Count + " albums?");

                if(result == DialogResult.Yes)
                {
                    //TODO Implement
                    //DataModel.Instance.Delete(SelectedAlbum);
                }
            }
            else
            {
                DialogService.NotificationMessage("No album was selected.");
            }
        }

        private void _addToPlaylist(object argument)
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

        private void _addAlbumToQueue(object argument)
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
