using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data;
using WMPLib;
using TuneMusix.Model;
using System.ComponentModel;
using System.Diagnostics;

namespace TuneMusix.Helpers
{
    public class AudioControls
    {

        public WindowsMediaPlayer player = new WindowsMediaPlayer();

        DataModel dataModel = DataModel.Instance;

        private static AudioControls instance;

        private AudioControls()
        {
            player.PlayStateChange += OnStateChanged;
            dataModel.CurrentTrackChanged += OnCurrentTrackChanged;
        }

        public static AudioControls Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioControls();
                }
                return instance;
            }
        }
        
        private void OnStateChanged(int state)
        {
          
        }

        public void PlayNext()
        {

        }

        private void OnCurrentTrackChanged(object source,object NewTrack)
        {
            Track NewCurrentTrack = NewTrack as Track;

            player.URL = NewCurrentTrack.sourceURL;
            OnNewTrackLoaded();
            player.controls.play();
        }

        public delegate void AudioControlsEventHandler(object obj);
        public event AudioControlsEventHandler NewTrackLoaded;

        protected virtual void OnNewTrackLoaded()
        {
            if (NewTrackLoaded!=null)
            {
                NewTrackLoaded(dataModel.CurrentTrack);
            }
        }

        /// <summary>
        /// Returns true if a track is currently playing
        /// </summary>
        /// <returns></returns>
       public bool IsPlaying()
        {
            if (player.playState == WMPPlayState.wmppsPlaying)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double CurrentPosition
        {
            get
            {
                return this.player.controls.currentPosition;
            }
            set
            {
                this.player.controls.currentPosition = value;
            }
        }

    }
}
