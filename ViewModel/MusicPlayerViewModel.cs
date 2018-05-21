using ControlzEx;
using System;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel : ViewModelBase
    {
        //------------Objects----------------------------------
        private Options options = Options.Instance;
        private AudioControls audioControls = AudioControls.Instance;

        //-------------RelayCommands----------------------------------------
        public RelayCommand LeftMouseDown_Slider { get; set; }
        public RelayCommand LeftMouseUp_Slider { get; set; }
        public RelayCommand PlayButton { get; set; }
        public RelayCommand NextTrack { get; set; }
        public RelayCommand PreviousTrack { get; set; }
        public RelayCommand RepeatButton { get; set; }
        public RelayCommand VolumeButton { get; set; }
        public RelayCommand ShuffleButton { get; set; }

       //---------------Constants----------------------------------
        private const string PLAY_ICON = "PlayCircleOutline";
        private const string PAUS_ICON = "PauseCircleOutline";
        private const string VOLUME_HIGH_ICON = "VolumeHigh";
        private const string VOLUME_OFF_ICON = "VolumeOff";
        private const string VOLUME_LOW_ICON = "VolumeLow";
        private const string VOLUME_MEDIUM_ICON = "VolumeMedium";
        private const string SHUFFLE_ON_ICON = "Shuffle";
        private const string SHUFFLE_OFF_ICON = "ShuffleDisabled";
        private const string REPEAT_ICON = "repeat";
        private const string REPEAT_ONCE_ICON = "repeatonce";
        private const string REPEAT_OFF_ICON = "repeatoff";

        //-------------Fields----------------------------------
        private Timer timer;
        private double currentPosition;
        private bool _dragging;

        //Constructor
        public MusicPlayerViewModel()
        {
            _dragging = false;
            timer = new Timer(100);

            //RelayCommands
            LeftMouseDown_Slider = new RelayCommand(leftMouseDown_Slider);
            LeftMouseUp_Slider = new RelayCommand(leftMouseUp_Slider);
            PlayButton = new RelayCommand(playButton);
            NextTrack = new RelayCommand(nextTrack);
            PreviousTrack = new RelayCommand(previousTrack);
            RepeatButton = new RelayCommand(onRepeatButtonClicked);
            VolumeButton = new RelayCommand(onVolumeButtonClicked);
            ShuffleButton = new RelayCommand(shuffleButton);

            //Events
            timer.Elapsed += OnTimeElapsed;
            audioControls.TrackChanged += OnTrackChanged;
            audioControls.PlaystateChanged += onPlaystateChanged;
            dataModel.CurrentPlaylistChanged += onCurrentPlaylistChanged;
        }

        #region getter and setter
        //Getter and setter  
        public string PlayButtonIcon
        {
            get
            {
                if (audioControls.IsPlaying)
                {
                    return PAUS_ICON;
                }
                return PLAY_ICON;        
            }
        }
        //Returns the string for the volume button icon
        public string VolumeButtonIcon
        {
            get
            {
                if (audioControls.Mute)
                {
                    return VOLUME_OFF_ICON;
                }
                else
                {
                    int vol = Volume;
                    if (vol >= 60)
                        return VOLUME_HIGH_ICON;

                    if (vol < 60 && vol >= 30)
                        return VOLUME_MEDIUM_ICON;

                    if (vol > 0 && vol < 30)
                        return VOLUME_LOW_ICON;

                    return VOLUME_OFF_ICON;
                }        
            }
        }


        public double CurrentSliderPosition
        {
            get
            {
                return this.currentPosition;
            }
            set
            {
                currentPosition = value;
                RaisePropertyChanged("CurrentSliderPosition");
            }
        }
        public double CurrentPosition
        {
            get
            {
                return audioControls.CurrentPosition.TotalSeconds;
            }
            set
            {
                audioControls.CurrentPosition = TimeSpan.FromSeconds(value);
            }
        }
        public string CurrentPlaylistName
        {
            get
            {
                if(dataModel.CurrentPlaylist != null){
                    return dataModel.CurrentPlaylist.Name;
                }
                else
                {
                    return "...";
                }
            }
        }
        /// <summary>
        /// Returns true if the current AudioPlayer is not null.
        /// </summary>
        public bool TrackLoaded
        {
            get
            {
               return audioControls.IsLoaded;
            }
        }

        /// <summary>
        /// Changes the balance for playback (not yet implemented!).
        /// </summary>
        public int Balance
        {
            set { } //Implement
        }

        /// <summary>
        /// Returns the length of the currently loaded track.
        /// </summary>
        public double Length
        {
            get
            {
                return audioControls.Length.TotalSeconds;
            }
        }

        //Value of the volume slider
        public int Volume
        {
            get
            {
                return options.Volume;
            }
            set
            {
                options.Volume = value;
                RaisePropertyChanged("VolumeButtonIcon");
            }
        }

        //Position of the current track playback
        public string CurrentTrackName
        {
            get
            {
                if (CurrentTrack != null)
                {
                    return CurrentTrack.Name;
                }
                else
                {
                    return "...";
                }
            }
        }

        public string ShuffleButtonIcon
        {
            get
            {
                if (options.Shuffle)
                {
                    return SHUFFLE_ON_ICON;
                }
                else
                {
                    return SHUFFLE_OFF_ICON;
                }
            }
        }

        /// <summary>
        /// 0 = No repeat
        /// 1 = Repeat all
        /// 2 = repeat track
        /// </summary>
        public int RepeatTrack
        {
            get { return options.RepeatTrack; }
            set
            {
                options.RepeatTrack = value;
                RaisePropertyChanged("RepeatTrack");                     
            }
        }

        public string RepeatButtonIcon
        {
            get
            {
                if (RepeatTrack == 0)
                {
                    return REPEAT_OFF_ICON;
                }
                else if (RepeatTrack == 1)
                {
                    return REPEAT_ICON;
                }
                else
                {
                    return REPEAT_ONCE_ICON;
                }
            }
        }
        #endregion
    }
}
