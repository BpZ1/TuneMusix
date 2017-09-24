
using System;
using System.Timers;
using System.Windows.Controls;

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
        public void _playButton(object argument)
        {
            if (audioControls.IsPlaying)
            {
                _timer.Stop();
                audioControls.Pause();
                RaisePropertyChanged("PlayButtonIcon");
            }
            else
            {
                audioControls.Play();
                RaisePropertyChanged("PlayButtonIcon");
            }
        }

        private void OnPlaying(object source)
        {
            PlayButtonIcon = _pauseIcon;           
        }
        private void OnPaused(object source)
        {
            PlayButtonIcon = _playIcon;
        }
        private void OnStopped(object source)
        {
            PlayButtonIcon = _playIcon;
        }

        private void OnTrackChanged(object e)
        {
            RaisePropertyChanged("TrackLoaded");
            RaisePropertyChanged("Length");
            Console.WriteLine("Current track changed");
            RaisePropertyChanged("CurrentTrackName");     
            RaisePropertyChanged("CurrentPosition");
            RaisePropertyChanged("PlayButtonIcon");           
            _timer.Start();
        }
        /// <summary>
        /// This method gets called every time the time set in the
        /// timer has elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_dragging)
            {
                try
                {
                    CurrentSliderPosition = CurrentPosition;
                }
                catch
                {

                }
            }
        }

        private void _onCurrentPlaylistChanged(object source,object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
        }
        private void _onCurrentTrackChanged(object source,object newPlaylist)
        {
            RaisePropertyChanged("CurrentTrackName");
        }
        /// <summary>
        /// Gets called when the user has started manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        public void _leftMouseDown_Slider(object sender)
        {
            _dragging = true;
        }
        /// <summary>
        /// Gets called when the user has finished manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        public void _leftMouseUp_Slider(object sender)
        {
            _dragging = false;
            var slider = sender as Slider;
            CurrentPosition = slider.Value;
        }

        public void OnCurrentTrackChanged(object source,object newCurrentTrack)
        {
            RaisePropertyChanged("CurrentSliderPosition");
            RaisePropertyChanged("Length");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void _nextTrack(object argument)
        {
            audioControls.PlayNext();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void _previousTrack(object argument)
        {
            audioControls.PlayPrevious();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void _onRepeatButtonClicked(object argument)
        {
            if (RepeatTrack+1 > 2)
            {
                RepeatTrack = 0;
            }
            else
            {
                RepeatTrack++;
            }                   
            RaisePropertyChanged("RepeatButtonIcon");
        }
    }
}
