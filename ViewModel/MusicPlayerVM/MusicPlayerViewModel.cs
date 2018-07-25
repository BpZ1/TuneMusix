using System;
using System.Timers;
using System.Windows.Threading;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel : ViewModelBase
    {
        //------------------------------------------------------
        private Options options = Options.Instance;
        private AudioControls audioControls = AudioControls.Instance;

        //-------------RelayCommands----------------------------------------
        public RelayCommand PositionSlider_MouseDown { get; set; }
        public RelayCommand PositionSlider_MouseUp { get; set; }
        public RelayCommand PlayButton { get; set; }
        public RelayCommand NextTrack { get; set; }
        public RelayCommand PreviousTrack { get; set; }
        public RelayCommand RepeatButton { get; set; }
        public RelayCommand VolumeButton { get; set; }
        public RelayCommand VolumeButtonReleased { get; set; }
        public RelayCommand ShuffleButton { get; set; }
        public RelayCommand OpenVolumePopup { get; set; }
        public RelayCommand CloseVolumePopup { get; set; }

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
        private bool dragging;
        private DispatcherTimer dispatcherTimer;


        //Constructor
        public MusicPlayerViewModel()
        {
            dragging = false;
            timer = new Timer(100);
            VolumeSliderVisible = false;

            //RelayCommands
            PositionSlider_MouseDown = new RelayCommand(leftMouseDown_Slider);
            PositionSlider_MouseUp = new RelayCommand(leftMouseUp_Slider);
            PlayButton = new RelayCommand(playButton);
            NextTrack = new RelayCommand(nextTrack);
            PreviousTrack = new RelayCommand(previousTrack);
            RepeatButton = new RelayCommand(onRepeatButtonClicked);
            VolumeButton = new RelayCommand(onVolumeButtonClicked);
            VolumeButtonReleased = new RelayCommand(onVolumeButtonReleased);
            ShuffleButton = new RelayCommand(shuffleButton);
            OpenVolumePopup = new RelayCommand(openVolumePopup);
            CloseVolumePopup = new RelayCommand(startPopupClosingTimer);

            //Events
            timer.Elapsed += OnTimeElapsed;
            dataModel.CurrentTrackChanged += OnTrackChanged;
            audioControls.PlaystateChanged += onPlaystateChanged;
            dataModel.CurrentPlaylistChanged += onCurrentPlaylistChanged;
        }

        public bool VolumeSliderVisible
        {
            get;
            set;
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
        public string PositionTime
        {
            get
            {
                TimeSpan position = TimeSpan.FromSeconds(CurrentPosition);
                return position.ToString();
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
                RaisePropertyChanged("Volume");
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
