using GongSolutions.Wpf.DragDrop;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TuneMusix.Helpers;
using TuneMusix.Model;
using System;
using System.Windows;
using System.Collections.ObjectModel;

namespace TuneMusix.ViewModel
{

    class TrackQueueViewModel : ViewModelBase, INotifyPropertyChanged, IDragSource, IDropTarget
    {
        private Track selectedTrack { get; set; }

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand DeleteSelectedTracks { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand TrackDoubleClicked { get; set; }
        public RelayCommand SelectionChanged { get; set; }

        public TrackQueueViewModel()
        {
            DeleteSelectedTracks = new RelayCommand(deleteSelectedTrack);
            AddToPlaylist = new RelayCommand(addToPlaylist);
            SelectionChanged = new RelayCommand(selectionChanged);
            PlayTrack = new RelayCommand(playTrack);
            dataModel.TrackQueueChanged += onTrackQueueChanged;
            TrackDoubleClicked = new RelayCommand(trackDoubleClicked);

            Options.Instance.ColorChanged += onColorChanged;
        }

        #region commands
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
        private void trackDoubleClicked(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

            var track = argument as Track;
            if (track != null)
            {
                CurrentTrack = track;
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(track);
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
        #endregion

        #region properties
        public ObservableCollection<Track> CurrentTrackQueue
        {
            get { return dataModel.TrackQueue; }
        }
        /// <summary>
        /// Brush that sets the highlight color of the current track in the trackqueue.
        /// </summary>
        public Brush HighlightColor
        {
            get
            {
                var palette = new PaletteHelper().QueryPalette();
                var hue = palette.AccentSwatch.AccentHues.ToArray()[palette.AccentHueIndex];
                return new SolidColorBrush(hue.Color);
            }
        }
        /// <summary>
        /// Returns the title for the queue, containing the number of tracks and 
        /// their combined time.
        /// </summary>
        public string HeaderText
        {
            get
            {
                if(CurrentTrackQueue.Count > 0)
                {
                    return "Track Queue [" + CurrentTrackQueue.Count + "]" + " - "  + combinedTrackTimes();
                }
                else
                {
                    return "Track Queue [" + CurrentTrackQueue.Count + "]";
                }
            }
        }
        #endregion
        private void onColorChanged()
        {
            RaisePropertyChanged("HighlightColor");
        }
        private void onTrackQueueChanged(object source, object argument)
        {
            RaisePropertyChanged("CurrentTrackQueue");
            RaisePropertyChanged("HeaderText");
        }
        private String combinedTrackTimes()
        {
            String result = "";
            foreach(Track track in CurrentTrackQueue)
            {
                result = TrackService.AddDurations(result, track.Duration);
            }
            return result;
        }
        #region drag and drop
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
        }

        public void DragCancelled()
        {
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            Logger.LogException(exception);
            throw exception;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.NotHandled = true;
            var sourceItem = dropInfo.Data;
            if (sourceItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Track track = dropInfo.Data as Track;
            if (track != null && dropInfo != null)
            {
                ListUtil.ChangeItemPosition(CurrentTrackQueue, track, dropInfo.UnfilteredInsertIndex);
                RaisePropertyChanged("CurrentTrackQueue");
            }
        }
        #endregion


    }
}
