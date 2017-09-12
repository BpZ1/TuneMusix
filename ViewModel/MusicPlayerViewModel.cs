using ControlzEx;
using System;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel : ViewModelBase
    {
        DataModel dataModel = DataModel.Instance;
        AudioControls audioControls = AudioControls.Instance;

        public RelayCommand LeftMouseDown_Slider { get; set; }
        public RelayCommand LeftMouseUp_Slider { get; set; }
        public RelayCommand ForwardButton { get; set; }
        public RelayCommand PlayButton { get; set; }
        public RelayCommand BackButton { get; set; }

        private string _playButtonIcon;
        private string _currentTrackName;
        private double _currentPosition;
        private Timer timer;
        private bool Dragging;

        //Constructor
        public MusicPlayerViewModel()
        {
            Dragging = false;
            timer = new Timer(10);
            _playButtonIcon = "PlayCircleOutline";

            //RelayCommands
            LeftMouseDown_Slider = new RelayCommand(_leftMouseDown_Slider);
            LeftMouseUp_Slider = new RelayCommand(_leftMouseUp_Slider);
            ForwardButton = new RelayCommand(_forwardButton);
            PlayButton = new RelayCommand(_playButton);
            BackButton = new RelayCommand(_backButton);

            //Events
            audioControls.player.PlayStateChange += OnPlayStateChange;
            timer.Elapsed += OnTimeElapsed;
            audioControls.player.PositionChange += OnPlayPositionChange;
            audioControls.player.MediaChange += OnCurrentItemChange;
        }

        //Getter and setter  
        public string PlayButtonIcon
        {
            get
            {
                return this._playButtonIcon;
            }
            set
            {
                this._playButtonIcon = value;
                RaisePropertyChanged("PlayButtonIcon");
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

        public double RealCurrentPosition
        {
            set
            {
                audioControls.player.controls.currentPosition = value;
            }
        }


        public int Balance
        {
            get { return audioControls.player.settings.balance; }
            set { audioControls.player.settings.balance = value; }
        }
        public bool Muted
        {
            get { return audioControls.player.settings.mute; }
            set { audioControls.player.settings.mute = value; }
        }
        public double Length
        {
            get
            {
                if (audioControls != null)
                {
                    if (audioControls.player != null)
                    {
                        if (audioControls.player.currentMedia != null)
                        {
                            return audioControls.player.currentMedia.duration;
                        }
                        else { return 0; }
                    }
                    else { return 0; }
                }
                else { return 0; }
            }
        }
        public int Volume
        {
            get { return audioControls.player.settings.volume; }
            set
            {
                audioControls.player.settings.volume = value;
                RaisePropertyChanged("Volume");
            }
        }
        public string CurrentTrackName
        {
            get
            {
                if (CurrentTrackMVM != null)
                {
                    return _currentTrackName;
                }
                else
                {
                    return "...";
                }
            }
            set
            {
                _currentTrackName = value;
                RaisePropertyChanged("CurrentTrackName");
            }
        }

    }
}
