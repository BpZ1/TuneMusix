using GongSolutions.Wpf.DragDrop;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{

    class TrackQueueViewModel : ViewModelBase, INotifyPropertyChanged, IDragSource, IDropTarget
    {
        public Track SelectedTrack { get; set; }

        //Relaycommands
        public RelayCommand PlayTrackCommand { get; set; }
        public RelayCommand RemoveSelectedTrackCommand { get; set; }
        public RelayCommand AddToPlaylistCommand { get; set; }
        public RelayCommand TrackDoubleClickedCommand { get; set; }

        public TrackQueueViewModel()
        {
            RemoveSelectedTrackCommand = new RelayCommand( RemoveSelectedTrack );
            AddToPlaylistCommand = new RelayCommand( AddToPlaylist );
            PlayTrackCommand = new RelayCommand( PlayTrack );
            TrackDoubleClickedCommand = new RelayCommand( TrackDoubleClicked );

            _dataModel.TrackQueue.TrackQueueChanged += OnTrackQueueChanged;
            Options.Instance.ColorChanged += OnColorChanged;
        }

        /// <summary>
        /// Changed the current track to the selected track.
        /// </summary>
        /// <param name="argument"></param>
        private void PlayTrack( object argument )
        {
            if ( SelectedTrack != null )
            {
                CurrentTrack = SelectedTrack;
            }
        }

        private void TrackDoubleClicked( object argument )
        {
            if ( argument == null )
                throw new ArgumentNullException();

            var track = argument as Track;
            if ( track != null )
            {
                CurrentTrack = track;
            }
        }
        /// <summary>
        /// Adds the selected track to a playlist
        /// </summary>
        /// <param name="obj"></param>
        private void AddToPlaylist( object argument )
        {
            Playlist selectedPlaylist = argument as Playlist;

            if ( selectedPlaylist != null )
            {
                _dataModel.AddTracksToPlaylist( new List<Track>() { SelectedTrack }, selectedPlaylist );
            }
        }

        private void RemoveSelectedTrack( object argument )
        {
            if ( SelectedTrack != null )
            {
                _dataModel.RemoveTrackFromQueue( SelectedTrack );
                RaisePropertyChanged( nameof( TrackQueue ) );
            }
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
                return new SolidColorBrush( hue.Color );
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
                if ( TrackQueue.Count > 0 )
                {
                    return $"Track Queue [{TrackQueue.Count}] - {CombinedTrackTimes()}";
                }
                else
                {
                    return $"Track Queue [{TrackQueue.Count}]";
                }
            }
        }

        private void OnColorChanged( object sender )
        {
            RaisePropertyChanged( nameof( HighlightColor ) );
        }
        private void OnTrackQueueChanged( object source, object argument )
        {
            RaisePropertyChanged( nameof( TrackQueue ) );
            RaisePropertyChanged( nameof( HeaderText ) );
        }
        private string CombinedTrackTimes()
        {
            var result = "";
            foreach ( Track track in TrackQueue )
            {
                result = TrackService.AddDurations( result, track.Duration );
            }
            return result;
        }
        #region drag and drop
        public void StartDrag( IDragInfo dragInfo )
        {
            dragInfo.Data = dragInfo.SourceItem;
            dragInfo.Effects = DragDropEffects.Copy;
        }

        public bool CanStartDrag( IDragInfo dragInfo )
        {
            if ( dragInfo != null )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dropped( IDropInfo dropInfo )
        {
        }

        public void DragCancelled()
        {
        }

        public bool TryCatchOccurredException( Exception exception )
        {
            Logger.LogException( exception );
            throw exception;
        }

        public void DragOver( IDropInfo dropInfo )
        {
            dropInfo.NotHandled = true;
            var sourceItem = dropInfo.Data;
            if ( sourceItem != null )
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop( IDropInfo dropInfo )
        {
            Track track = dropInfo.Data as Track;
            if ( track != null && dropInfo != null )
            {
                ListUtil.ChangeItemPosition( TrackQueue, track, dropInfo.UnfilteredInsertIndex );
                RaisePropertyChanged( "TrackQueue" );
            }
        }
        #endregion


    }
}
