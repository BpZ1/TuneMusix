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
        public RelayCommand LeftMouseDown_ForwardButton { get; set; }
        public RelayCommand LeftMouseDown_PlayButton { get; set; }
        public RelayCommand LeftMouseDown_BackButton { get; set; }

        private string _playButtonURL;
        private string _stopButtonURL;
        private string _playButton;
        private string _forwardButton;
        private string _backwardsButton;
        private string _currentTrackName;
        private double _currentPosition;
        private Timer timer;
        private bool Dragging;

        //Constructor
        public MusicPlayerViewModel()
        {
            _playButton = "../Images/playbutton.png";
            _playButtonURL = "../Images/playbutton.png";
            _stopButtonURL = "../Images/pausebutton.png";
            _forwardButton = "../Images/forwardbutton.png";
            _backwardsButton = "../Images/backbutton.png";
            Dragging = false;
            timer = new Timer(100);

            //RelayCommands
            LeftMouseDown_Slider = new RelayCommand(_leftMouseDown_Slider);
            LeftMouseUp_Slider = new RelayCommand(_leftMouseUp_Slider);
            LeftMouseDown_ForwardButton = new RelayCommand(_leftMouseDown_ForwardButton);
            LeftMouseDown_PlayButton = new RelayCommand(_leftMouseDown_PlayButton);
            LeftMouseDown_BackButton = new RelayCommand(_leftMouseDown_BackButton);

            //Events
            audioControls.player.PlayStateChange += OnPlayStateChange;
            timer.Elapsed += OnTimeElapsed;
            audioControls.player.PositionChange += OnPlayPositionChange;
            audioControls.player.MediaChange += OnCurrentItemChange;
        }

        //Getter and setter
        public string PlayButton
        {
            get
            {
                return this._playButton;
            }
            set
            {
                if(value != null)
                {
                    this._playButton = value;
                }          
            }
        }
        public string ForwardButton
        {
            get { return this._forwardButton; }
        }
        public string BackwardButton
        {
            get { return this._backwardsButton; }
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
