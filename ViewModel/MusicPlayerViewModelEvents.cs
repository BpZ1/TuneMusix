using System.Timers;
using System.Windows.Controls;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel
    {

        public void playButton(object argument)
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

        private void OnPlaying(object source)
        {
            PlayButtonIcon = pauseIcon;           
        }
        private void OnPaused(object source)
        {
            PlayButtonIcon = playIcon;
        }
        private void OnStopped(object source)
        {
            PlayButtonIcon = playIcon;
        }

        private void OnTrackChanged(object e)
        {
            RaisePropertyChanged("Length");
            RaisePropertyChanged("CurrentTrackName");     
            RaisePropertyChanged("CurrentPosition");
            RaisePropertyChanged("PlayButtonIcon");
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
            if (!Dragging)
            {
                RaisePropertyChanged("PlayButtonIcon");
                try
                {
                    CurrentSliderPosition = CurrentPosition;
                }
                catch
                {

                }
            }
        }

        public void leftMouseDown_Slider(object sender)
        {
            Dragging = true;
        }

        public void leftMouseUp_Slider(object sender)
        {
            Dragging = false;
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
        private void nextTrack(object argument)
        {
            audioControls.PlayNext();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        private void previousTrack(object argument)
        {
            audioControls.PlayPrevious();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
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
    }
}
