
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
        private void playButton(object argument)
        {
            if (audioControls.IsPlaying)
            {
                timer.Stop();
                audioControls.Pause();
                RaisePropertyChanged("PlayButtonIcon");
            }
            else
            {
                audioControls.Play();
                RaisePropertyChanged("PlayButtonIcon");
            }
        }

        private void onPlaystateChanged(object argument)
        {
            RaisePropertyChanged("PlayButtonIcon");
        }

        private void OnTrackChanged(object e)
        {
            RaisePropertyChanged("TrackLoaded");
            RaisePropertyChanged("Length");
            RaisePropertyChanged("CurrentTrackName");     
            RaisePropertyChanged("CurrentPosition");        
            timer.Start();
        }
        /// <summary>
        /// This method gets called every time the time set in the
        /// timer has elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_dragging && audioControls.IsLoaded)
            {
                CurrentSliderPosition = CurrentPosition;
            }
        }

        private void onCurrentPlaylistChanged(object source,object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
        }

        /// <summary>
        /// Gets called when the user has started manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        public void leftMouseDown_Slider(object sender)
        {
            _dragging = true;
        }
        /// <summary>
        /// Gets called when the user has finished manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        private void leftMouseUp_Slider(object sender)
        {
            _dragging = false;
            var slider = sender as Slider;
            CurrentPosition = slider.Value;
        }

        private void onCurrentTrackChanged(object source,object newCurrentTrack)
        {
            RaisePropertyChanged("CurrentSliderPosition");
            RaisePropertyChanged("Length");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void nextTrack(object argument)
        {
            audioControls.PlayNext();
        }
        
        //Plays the previous track
        private void previousTrack(object argument)
        {
            audioControls.PlayPrevious();
        }

        private void onVolumeButtonClicked(object argument)
        {
            if(!audioControls.Mute)
            {
                //Mute playback
                audioControls.Mute = true;
                RaisePropertyChanged("VolumeButtonIcon");
            }
            else
            {
                //unmute playback
                audioControls.Mute = false;
                RaisePropertyChanged("VolumeButtonIcon");
            }
        }

        //Changes the state of the repeating functionality
        private void onRepeatButtonClicked(object argument)
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

        private void shuffleButton(object argument)
        {
            audioControls.ShuffleChanged();
            RaisePropertyChanged("ShuffleButtonIcon");
        }
    }
}
