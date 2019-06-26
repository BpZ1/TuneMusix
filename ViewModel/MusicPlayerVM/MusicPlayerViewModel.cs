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
        private Options _options = Options.Instance;
        private AudioControls _audioControls = AudioControls.Instance;

        //-------------RelayCommands----------------------------------------
        public RelayCommand SliderDraggingOn { get; set; }
        public RelayCommand SliderDraggingOff { get; set; }
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
        private Timer _timer;
        private double _currentPosition;
        private bool _dragging;
        private DispatcherTimer _dispatcherTimer;


        //Constructor
        public MusicPlayerViewModel()
        {
            _dragging = false;
            _timer = new Timer(100);
            VolumeSliderVisible = false;

            //RelayCommands
            PositionSlider_MouseUp = new RelayCommand(_leftMouseUpSlider);
            SliderDraggingOn = new RelayCommand(_sliderDraggingOn);
            SliderDraggingOff = new RelayCommand(_sliderDraggingOff);
            PlayButton = new RelayCommand(_playButton);
            NextTrack = new RelayCommand(_nextTrack);
            PreviousTrack = new RelayCommand(_previousTrack);
            RepeatButton = new RelayCommand(_onRepeatButtonClicked);
            VolumeButton = new RelayCommand(_onVolumeButtonClicked);
            VolumeButtonReleased = new RelayCommand(_onVolumeButtonReleased);
            ShuffleButton = new RelayCommand(_shuffleButton);
            OpenVolumePopup = new RelayCommand(_openVolumePopup);
            CloseVolumePopup = new RelayCommand(_startPopupClosingTimer);

            //Events
            _timer.Elapsed += OnTimeElapsed;
            _dataModel.CurrentTrackChanged += OnTrackChanged;
            _audioControls.PlaystateChanged += OnPlaystateChanged;
            _dataModel.CurrentPlaylistChanged += OnCurrentPlaylistChanged;
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
                if (_audioControls.IsPlaying)
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
                if (_audioControls.Mute)
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
                return this._currentPosition;
            }
            set
            {
                _currentPosition = value;
                RaisePropertyChanged("CurrentSliderPosition");
            }
        }
        public double CurrentPosition
        {
            get
            {
                return _audioControls.CurrentPosition.TotalSeconds;
            }
            set
            {
                _audioControls.CurrentPosition = TimeSpan.FromSeconds(value);
            }
        }
        public string SliderPostionString
        {
            get { return Converter.TimeSpanToString(TimeSpan.FromSeconds(this._currentPosition)); }
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
                if(_dataModel.CurrentPlaylist != null){
                    return _dataModel.CurrentPlaylist.Name;
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
               return _audioControls.IsLoaded;
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
                return _audioControls.Length.TotalSeconds;
            }
        }
        //Value of the volume slider
        public int Volume
        {
            get
            {
                return _options.Volume;
            }
            set
            {
                _options.Volume = value;
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
                if (_options.Shuffle)
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
            get { return _options.RepeatTrack; }
            set
            {
                _options.RepeatTrack = value;
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
