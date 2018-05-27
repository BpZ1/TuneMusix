using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    /// <summary>
    /// This class is the datacontext for the page that displays the tracks of the current playlist.
    /// </summary>
    class CurrentPlaylistViewModel : ViewModelBase, IDropTarget, IDragSource
    {
      
        public RelayCommand SelectionChanged { get; set; }
        public RelayCommand SetPlaylistIndex { get; set; }
        public RelayCommand RemoveFromPlaylist { get; set; }
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand TrackDoubleClicked { get; set; }
        public RelayCommand OnItemDrop { get; set; }
        public ObservableCollection<Track> SelectedTracks { get; set; }


        public CurrentPlaylistViewModel()
        {

            SelectedTracks = new ObservableCollection<Track>();
            SelectionChanged = new RelayCommand(selectionChanged);
            SetPlaylistIndex = new RelayCommand(indexChanged);
            PlayTrack = new RelayCommand(playTrack);
            RemoveFromPlaylist = new RelayCommand(removeFromCurrentPlaylist);
            TrackDoubleClicked = new RelayCommand(trackDoubleClicked);

            dataModel.CurrentPlaylistChanged += onCurrentPlaylistChanged;
        }
        #region commands
        /// <summary>
        /// Changes the selection of the playlist.
        /// </summary>
        /// <param name="argument"></param>
        private void selectionChanged(object argument)
        {
            var listView = argument as ListView;
            if (listView != null && SelectedTracks != null)
            {
                SelectedTracks.Clear();
                foreach (Track track in listView.SelectedItems)
                {
                    SelectedTracks.Add(track);
                }
            }
        }

        private void playTrack(object argument)
        {
            if (SelectedTracks == null) return;
            if (SelectedTracks.Count > 0)
            {
                Track selected = SelectedTracks.ToList<Track>().First();
                CurrentTrack = selected;
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(selected);
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
                dataModel.QueueIndex = dataModel.CurrentPlaylist.Itemlist.IndexOf(track);
            }
        }

        private void trackDoubleClicked(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

            var track = argument as Track;
            if(track != null)
            {
                CurrentTrack = track;
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(track);
            }
        }
        #endregion
        private void onCurrentPlaylistChanged(object source, object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
            RaisePropertyChanged("CurrentPlaylistTracks");
            SelectedTracks.Clear();
        }

        #region drag and drop
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
            //Check the type of the dropped item.
            if (dropInfo.Data.GetType() == typeof(Track))
            {
                if(dropInfo.VisualTargetItem != null)
                {
                    //Check if it was dropped in the listview.
                    if(dropInfo.VisualTargetItem.GetType() == typeof(ListViewItem))
                    {
                        Track track = dropInfo.Data as Track;
                        if (track != null && dropInfo != null)
                        {
                            ListUtil.ChangeItemPosition<Track>(CurrentPlaylistTracks, track, dropInfo.UnfilteredInsertIndex);
                            dataModel.CurrentPlaylist.IsModified = true;
                            RaisePropertyChanged("CurrentPlaylistTracks");
                        }
                    }
                }               
            }
            //If the droppen item was a playlist.
            if(dropInfo.Data.GetType() == typeof(Playlist))
            {
                Playlist playlist = dropInfo.Data as Playlist;
                dataModel.CurrentPlaylist = playlist;
            }
                 
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            dragInfo.Data = dragInfo.SourceItem;
            dragInfo.Effects = DragDropEffects.Copy;
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            if (dragInfo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dropped(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void DragCancelled()
        {
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            Logger.LogException(exception);
            throw exception;
        }
        #endregion

        #region properties
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
                    return dataModel.CurrentPlaylist.Itemlist;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
