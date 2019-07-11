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

namespace TuneMusix.ViewModel
{

    class TrackQueueViewModel : ViewModelBase, INotifyPropertyChanged, IDragSource, IDropTarget
    {
        public Track SelectedTrack { get; set; }

        //Relaycommands
        public RelayCommand PlayTrack { get; set; }
        public RelayCommand RemoveSelectedTrack { get; set; }
        public RelayCommand AddToPlaylist { get; set; }
        public RelayCommand TrackDoubleClicked { get; set; }

        public TrackQueueViewModel()
        {
            RemoveSelectedTrack = new RelayCommand(_removeSelectedTrack);
            AddToPlaylist = new RelayCommand(_addToPlaylist);
            PlayTrack = new RelayCommand(_playTrack);
            TrackDoubleClicked = new RelayCommand(_trackDoubleClicked);
       
            _dataModel.TrackQueue.TrackQueueChanged += OnTrackQueueChanged;
            Options.Instance.ColorChanged += OnColorChanged;
        }

        #region commands
        /// <summary>
        /// Changed the current track to the selected track.
        /// </summary>
        /// <param name="argument"></param>
        private void _playTrack(object argument)
        {
            if(SelectedTrack != null)
            {
                CurrentTrack = SelectedTrack;
            }     
        }

        private void _trackDoubleClicked(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

            var track = argument as Track;
            if (track != null)
            {
                CurrentTrack = track;
            }
        }
        /// <summary>
        /// Adds the selected track to a playlist
        /// </summary>
        /// <param name="obj"></param>
        private void _addToPlaylist(object argument)
        {
            Playlist selectedPlaylist = argument as Playlist;

            if (selectedPlaylist != null)
            {
                _dataModel.AddTracksToPlaylist(new List<Track>() { SelectedTrack }, selectedPlaylist);
            }
        }

        private void _removeSelectedTrack(object argument)
        {
            if(SelectedTrack != null)
            {
                _dataModel.RemoveTrackFromQueue(SelectedTrack);
                RaisePropertyChanged("CurrentTrackQueue");
            }
        }
        #endregion

        #region properties
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
                if(TrackQueue.Count > 0)
                {
                    return "Track Queue [" + TrackQueue.Count + "]" + " - "  + combinedTrackTimes();
                }
                else
                {
                    return "Track Queue [" + TrackQueue.Count + "]";
                }
            }
        }
        #endregion
        private void OnColorChanged()
        {
            RaisePropertyChanged("HighlightColor");
        }
        private void OnTrackQueueChanged(object source, object argument)
        {
            RaisePropertyChanged("TrackQueue");
            RaisePropertyChanged("HeaderText");
        }
        private String combinedTrackTimes()
        {
            String result = "";
            foreach(Track track in TrackQueue)
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
                ListUtil.ChangeItemPosition(TrackQueue, track, dropInfo.UnfilteredInsertIndex);
                RaisePropertyChanged("TrackQueue");
            }
        }
        #endregion


    }
}
