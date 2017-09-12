using System;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel
    {
        public void _leftMouseDown_ForwardButton(object argument)
        {
            //Implement
        }

        public void _leftMouseDown_PlayButton(object argument)
        {
            if (audioControls.player.currentMedia != null)
            {
                Console.WriteLine("Pause");
                if (audioControls.player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    Console.WriteLine("Pause");
                    audioControls.player.controls.pause();
                    Console.WriteLine("Pause");
                }
                else
                {
                    audioControls.player.controls.play();
                }
            }

        }

        public void _leftMouseDown_BackButton(object argument)
        {
            //Implement
        }

        public void OnPlayStateChange(int NewState)
        {
            if (NewState == 1 || NewState == 2)
            {
                //Stop
                _playButton = _playButtonURL;
                RaisePropertyChanged("PlayButton");
            }
            if (NewState == 3)
            {
                //Start
                _playButton = _stopButtonURL;
                RaisePropertyChanged("PlayButton");
            }
        }

        public void OnPlayPositionChange(double oldValue, double newValue)
        {
            CurrentSliderPosition = audioControls.player.controls.currentPosition;
            RaisePropertyChanged("CurrentPosition");
        }


        public void OnCurrentItemChange(object e)
        {
            this.CurrentTrackName = dataModel.CurrentTrackDM.tTitle;
            RaisePropertyChanged("Length");
            RaisePropertyChanged("CurrentPosition");
            RaisePropertyChanged("TrackLoaded");
            timer.Start();
            CurrentSliderPosition = 0;
        }

        public void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            if (!Dragging)
            {
                try
                {
                    CurrentSliderPosition = audioControls.player.controls.currentPosition;
                }
                catch
                {

                }

            }
        }

        public void _leftMouseDown_Slider(object sender)
        {
            Dragging = true;
        }

        public void _leftMouseUp_Slider(object sender)
        {
            Dragging = false;
            var slider = sender as Slider;
            RealCurrentPosition = slider.Value;
        }
    }
}
