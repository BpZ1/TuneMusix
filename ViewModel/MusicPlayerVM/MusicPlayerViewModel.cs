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
        private bool _dragging;
        private DispatcherTimer _dispatcherTimer;


        //Constructor
        public MusicPlayerViewModel()
        {
            _dragging = false;
            _timer = new Timer( 100 );
            VolumeSliderVisible = false;

            //RelayCommands
            PositionSlider_MouseUp = new RelayCommand( _leftMouseUpSlider );
            SliderDraggingOn = new RelayCommand( _sliderDraggingOn );
            SliderDraggingOff = new RelayCommand( _sliderDraggingOff );
            PlayButton = new RelayCommand( _playButton );
            NextTrack = new RelayCommand( _nextTrack );
            PreviousTrack = new RelayCommand( _previousTrack );
            RepeatButton = new RelayCommand( _onRepeatButtonClicked );
            VolumeButton = new RelayCommand( _onVolumeButtonClicked );
            VolumeButtonReleased = new RelayCommand( _onVolumeButtonReleased );
            ShuffleButton = new RelayCommand( _shuffleButton );
            OpenVolumePopup = new RelayCommand( _openVolumePopup );
            CloseVolumePopup = new RelayCommand( _startPopupClosingTimer );

            //Events
            _timer.Elapsed += OnTimeElapsed;
            _dataModel.TrackQueue.CurrentTrackChanged += OnTrackChanged;
            _audioControls.PlaystateChanged += OnPlaystateChanged;
            _dataModel.CurrentPlaylistChanged += OnCurrentPlaylistChanged;
        }

        public bool VolumeSliderVisible { get; set; }

        #region getter and setter
        //Getter and setter  
        public string PlayButtonIcon => _audioControls.IsPlaying ? PAUS_ICON : PLAY_ICON;

        //Returns the string for the volume button icon
        public string VolumeButtonIcon
        {
            get
            {
                if ( _audioControls.Mute )
                {
                    return VOLUME_OFF_ICON;
                }
                else
                {
                    int vol = Volume;
                    if ( vol >= 60 )
                    {
                        return VOLUME_HIGH_ICON;
                    }

                    if ( vol < 60 && vol >= 30 )
                    {
                        return VOLUME_MEDIUM_ICON;
                    }

                    if ( vol > 0 && vol < 30 )
                    {
                        return VOLUME_LOW_ICON;
                    }

                    return VOLUME_OFF_ICON;
                }
            }
        }
        public ObservableValue<double> CurrentSliderPosition { get; set; } = new ObservableValue<double>();

        public double CurrentPosition
        {
            get
            {
                return _audioControls.CurrentPosition.TotalSeconds;
            }
            set
            {
                _audioControls.CurrentPosition = TimeSpan.FromSeconds( value );
            }
        }
        public string SliderPostionString => Converter.TimeSpanToString( TimeSpan.FromSeconds( CurrentPosition ) );

        public string PositionTime => TimeSpan.FromSeconds( CurrentPosition ).ToString();

        public string CurrentPlaylistName => _dataModel.CurrentPlaylist != null ? _dataModel.CurrentPlaylist.Name : "...";

        /// <summary>
        /// Returns true if the current AudioPlayer is not null.
        /// </summary>
        public bool TrackLoaded => _audioControls.IsLoaded;

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
        public double Length => _audioControls.Length.TotalSeconds;

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
                RaisePropertyChanged( "Volume" );
                RaisePropertyChanged( "VolumeButtonIcon" );
            }
        }
        //Position of the current track playback
        public string CurrentTrackName => CurrentTrack != null ? CurrentTrack.Name : "...";

        public string ShuffleButtonIcon => _options.Shuffle ? SHUFFLE_ON_ICON : SHUFFLE_OFF_ICON;

        /// <summary>
        /// 0 = No repeat
        /// 1 = Repeat all
        /// 2 = repeat track
        /// </summary>
        public RepeatType RepeatTrack
        {
            get { return _options.RepeatTrack; }
            set
            {
                _options.RepeatTrack = value;
                RaisePropertyChanged( nameof( RepeatTrack ) );
            }
        }
        public string RepeatButtonIcon
        {
            get
            {
                switch ( RepeatTrack )
                {
                    case RepeatType.NoRepeat:
                        return REPEAT_OFF_ICON;

                    case RepeatType.RepeatAll:
                        return REPEAT_ICON;

                    case RepeatType.RepeatCurrent:
                        return REPEAT_ONCE_ICON;

                    default:
                        throw new ArgumentOutOfRangeException( $"Invalid value for RepeatTrack: {RepeatTrack}" );
                }
            }
        }
        #endregion
    }
}
