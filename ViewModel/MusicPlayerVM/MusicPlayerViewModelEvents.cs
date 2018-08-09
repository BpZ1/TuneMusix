using System;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;
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
                timer.Start();
                audioControls.Play();
                RaisePropertyChanged("PlayButtonIcon");
            }
        }
        private void onPlaystateChanged(object argument)
        {
            RaisePropertyChanged("PlayButtonIcon");
        }
        private void OnTrackChanged(object sender,object argument)
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
            if (!dragging && audioControls.IsLoaded)
            {
                CurrentSliderPosition = CurrentPosition;
            }
            RaisePropertyChanged("SliderPostionString");
        }

        private void onCurrentPlaylistChanged(object source,object newPlaylist)
        {
            RaisePropertyChanged("CurrentPlaylistName");
        }
        /// <summary>
        /// Gets called when the PreviewMouseDown event on the 
        /// grid surrounding the position slider is called.
        /// </summary>
        /// <param name="sender"></param>
        private void sliderDraggingOn(object sender)
        {
            dragging = true;
        }
        /// <summary>
        /// Gets called when the PreviewMouseUp event on the 
        /// grid surrounding the position slider is called.
        /// </summary>
        /// <param name="sender"></param>
        private void sliderDraggingOff(object sender)
        {
            dragging = false;
        }
        /// <summary>
        /// Gets called when the user has finished manipulating the position slider.
        /// </summary>
        /// <param name="sender"></param>
        private void leftMouseUp_Slider(object sender)
        {
            dragging = false;
            var slider = sender as Slider;
            CurrentPosition = slider.Value;
        }

        private void onCurrentTrackChanged(object source,object newCurrentTrack)
        {
            RaisePropertyChanged("CurrentSliderPosition");
            RaisePropertyChanged("Length");
            RaisePropertyChanged("CurrentTrackName");
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

        private void onVolumeButtonReleased(object argument)
        {
            Options.Instance.SaveValues();
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

        /// <summary>
        /// Opens the popup for the volume slider and stops the 
        /// closing timer.
        /// </summary>
        /// <param name="argument"></param>
        private void openVolumePopup(object argument)
        {
            VolumeSliderVisible = true;
            RaisePropertyChanged("VolumeSliderVisible");
            if(dispatcherTimer != null)
                ((DispatcherTimer)dispatcherTimer).Stop();
        }

        /// <summary>
        /// Starts the timer for the closing of the popup.
        /// </summary>
        /// <param name="argument"></param>
        private void startPopupClosingTimer(object argument)
        {
            dispatcherTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            dispatcherTimer.Tick += closeVolumePopup;

            dispatcherTimer.Start();
        }

        /// <summary>
        /// Closes the popup for the volume slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeVolumePopup(object sender, EventArgs e)
        {          
            VolumeSliderVisible = false;
            RaisePropertyChanged("VolumeSliderVisible");
            ((DispatcherTimer)dispatcherTimer).Stop();
        }

    }
}
