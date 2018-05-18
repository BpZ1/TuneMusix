using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    /// <summary>
    /// This class is the datacontext for the page that displays the tracks of the current playlist.
    /// </summary>
    class CurrentPlaylistViewModel : ViewModelBase, IDropTarget
    {

        DataModel dataModel = DataModel.Instance;
      
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SetPlaylistIndex { get; set; }
        public RelayCommand RemoveFromPlaylist { get; set; }
        public RelayCommand OnItemDrop { get; set; }
        public ObservableCollection<Track> SelectedTracks { get; set; }


        public CurrentPlaylistViewModel()
        {

            SelectedTracks = new ObservableCollection<Track>();
            SelectionChanged = new RelayCommand(selectionChanged);
            SetPlaylistIndex = new RelayCommand(indexChanged);
            RemoveFromPlaylist = new RelayCommand(removeFromCurrentPlaylist);

            dataModel.CurrentPlaylistChanged += onCurrentPlaylistChanged;
        }

        /// <summary>
        /// Changes the selection of the playlist.
        /// </summary>
        /// <param name="argument"></param>
        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView != null)
            {
                SelectedTracks.Clear();
                foreach (Track track in listView.SelectedItems)
                {
                    SelectedTracks.Add(track);
                }
            }
        }

        private void removeFromCurrentPlaylist(object argument)
        {
            if (SelectedTracks != null && dataModel.CurrentPlaylist != null)
            {
                dataModel.RemoveTracksFromPlaylist(SelectedTracks.ToList<Track>(), dataModel.CurrentPlaylist);
            }
        }
        /// <summary>
        /// This method changes the index of the trackqueue to the currently selected track.
        /// </summary>
        /// <param name="argument"></param>
        private void indexChanged(object argument)
        {
            if (SelectedTracks != null && CurrentPlaylist != null)
            {
                var track = SelectedTracks.First();
                dataModel.CurrentTrack = track;
                dataModel.QueueIndex = dataModel.CurrentPlaylist.Tracklist.IndexOf(track);
            }
        }

        private void onCurrentPlaylistChanged(object source, object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
            RaisePropertyChanged("CurrentPlaylistTracks");
            SelectedTracks.Clear();
        }

        //drag and drop methods
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = true;
            Playlist sourceItem = dropInfo.Data as Playlist;
            if (sourceItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Playlist playlist = dropInfo.Data as Playlist;
            dataModel.CurrentPlaylist = playlist;
        }


        //setter and getter
        public string CurrentPlaylistName
        {
            get
            {
                if(dataModel.CurrentPlaylist != null)
                {
                    return dataModel.CurrentPlaylist.Name;
                }
                else
                {
                    return "...";
                }
            }
        }
        public ObservableCollection<Track> CurrentPlaylistTracks
        {
            get
            {
                if (dataModel.CurrentPlaylist != null)
                {
                    return dataModel.CurrentPlaylist.Tracklist;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
