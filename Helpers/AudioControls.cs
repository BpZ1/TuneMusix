using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data;
using WMPLib;
using TuneMusix.Model;
using System.ComponentModel;

namespace TuneMusix.Helpers
{
    public class AudioControls : INotifyPropertyChanged
    {

        public WindowsMediaPlayer player = new WindowsMediaPlayer();

        DataModel dataModel = DataModel.Instance;

        private static AudioControls instance;

        private AudioControls() { }

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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PlayTrack()
        {
            if (dataModel.CurrentTrackDM != null)
            {
                player.URL = dataModel.CurrentTrackDM.url;
                player.controls.play();
                return true;
            }
            else
            {
                return false;
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


        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }




        public void smth()
        {
            
        }
    }
}
