using System;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    /// <summary>
    /// DataContext of the MusicPlayer page.
    /// </summary>
    partial class MusicPlayerViewModel
    {
        /// <summary>
        /// Gets called when the playbutton is pressed.
        /// </summary>
        /// <param name="argument"></param>
        private void _playButton( object argument )
        {
            if ( _audioControls.IsPlaying )
            {
                _timer.Stop();
                _audioControls.Pause();
                RaisePropertyChanged( "PlayButtonIcon" );
            }
            else
            {
                _timer.Start();
                _audioControls.Play();
                RaisePropertyChanged( "PlayButtonIcon" );
            }
        }
        private void OnPlaystateChanged( object argument )
        {
            RaisePropertyChanged( "PlayButtonIcon" );
        }
        private void OnTrackChanged( object sender, object argument )
        {
            RaisePropertyChanged( "TrackLoaded" );
            RaisePropertyChanged( "Length" );
            RaisePropertyChanged( "CurrentTrackName" );
            RaisePropertyChanged( "CurrentPosition" );
            _timer.Start();
        }
        /// <summary>
        /// This method gets called every time the time set in the
        /// timer has elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimeElapsed( object sender, ElapsedEventArgs e )
        {
            if ( !_dragging && _audioControls.IsLoaded )
            {
                CurrentSliderPosition.Value = CurrentPosition;
            }
            RaisePropertyChanged( "SliderPostionString" );
        }

        private void OnCurrentPlaylistChanged( object source, object newPlaylist )
        {
            RaisePropertyChanged( "CurrentPlaylistName" );
        }
        /// <summary>
        /// Gets called when the PreviewMouseDown event on the 
        /// grid surrounding the position slider is called.
        /// </summary>
        /// <param name="sender"></param>
        private void _sliderDraggingOn( object sender )
        {
            _dragging = true;
        }
        /// <summary>
        /// Gets called when the PreviewMouseUp event on the 
        /// grid surrounding the position slider is called.
        /// </summary>
        /// <param name="sender"></param>
        private void _sliderDraggingOff( object sender )
        {
            _dragging = false;
        }
        /// <summary>
        /// Gets called when the user has finished manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        private void _leftMouseUpSlider( object sender )
        {
            _dragging = false;
            var slider = sender as Slider;
            CurrentPosition = slider.Value;
        }

        private void OnCurrentTrackChanged( object source, object newCurrentTrack )
        {
            RaisePropertyChanged( "CurrentSliderPosition" );
            RaisePropertyChanged( "Length" );
            RaisePropertyChanged( "CurrentTrackName" );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void _nextTrack( object argument )
        {
            _audioControls.PlayNext();
        }

        //Plays the previous track
        private void _previousTrack( object argument )
        {
            _audioControls.PlayPrevious();
        }

        private void _onVolumeButtonClicked( object argument )
        {
            if ( !_audioControls.Mute )
            {
                //Mute playback
                _audioControls.Mute = true;
                RaisePropertyChanged( "VolumeButtonIcon" );
            }
            else
            {
                //unmute playback
                _audioControls.Mute = false;
                RaisePropertyChanged( "VolumeButtonIcon" );
            }
        }

        private void _onVolumeButtonReleased( object argument )
        {
            Options.Instance.SaveValues();
        }

        //Changes the state of the repeating functionality
        private void _onRepeatButtonClicked( object argument )
        {
            var currentRepeatValue = ( int ) RepeatTrack;
            RepeatTrack = ( RepeatType ) ( ( currentRepeatValue + 1 ) % 3 );
            RaisePropertyChanged( "RepeatButtonIcon" );
        }

        private void _shuffleButton( object argument )
        {
            _audioControls.ShuffleChanged();
            RaisePropertyChanged( "ShuffleButtonIcon" );
        }

        /// <summary>
        /// Opens the popup for the volume slider and stops the 
        /// closing timer.
        /// </summary>
        /// <param name="argument"></param>
        private void _openVolumePopup( object argument )
        {
            VolumeSliderVisible = true;
            RaisePropertyChanged( "VolumeSliderVisible" );
            if ( _dispatcherTimer != null )
                ( ( DispatcherTimer ) _dispatcherTimer ).Stop();
        }

        /// <summary>
        /// Starts the timer for the closing of the popup.
        /// </summary>
        /// <param name="argument"></param>
        private void _startPopupClosingTimer( object argument )
        {
            _dispatcherTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds( 1 )
            };

            _dispatcherTimer.Tick += _closeVolumePopup;

            _dispatcherTimer.Start();
        }

        /// <summary>
        /// Closes the popup for the volume slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _closeVolumePopup( object sender, EventArgs e )
        {
            VolumeSliderVisible = false;
            RaisePropertyChanged( "VolumeSliderVisible" );
            _dispatcherTimer.Stop();
        }

    }
}
