using ControlzEx;
using System;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    partial class MusicPlayerViewModel : ViewModelBase
    {
        DataModel dataModel = DataModel.Instance;
        Options options = Options.Instance;
        AudioControls audioControls = AudioControls.Instance;

        public RelayCommand LeftMouseDown_Slider { get; set; }
        public RelayCommand LeftMouseUp_Slider { get; set; }
        public RelayCommand PlayButton { get; set; }
        public RelayCommand NextTrack { get; set; }
        public RelayCommand PreviousTrack { get; set; }
        public RelayCommand RepeatButton { get; set; }

        private string _playButtonIcon;
        private double _currentPosition;
        private Timer timer;
        private bool Dragging;

        //Constructor
        public MusicPlayerViewModel()
        {
            Dragging = false;
            timer = new Timer(100);
            _playButtonIcon = "PlayCircleOutline";

            //RelayCommands
            LeftMouseDown_Slider = new RelayCommand(_leftMouseDown_Slider);
            LeftMouseUp_Slider = new RelayCommand(_leftMouseUp_Slider);
            PlayButton = new RelayCommand(_playButton);
            NextTrack = new RelayCommand(_nextTrack);
            PreviousTrack = new RelayCommand(_previousTrack);
            RepeatButton = new RelayCommand(_onRepeatButtonClicked);

            //Events
            timer.Elapsed += OnTimeElapsed;
            audioControls.NewTrackLoaded += OnTrackLoaded;
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

        public double CurrentPosition
        {
            get
            {
                return audioControls.CurrentPosition;
            }
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
        public bool Shuffle
        {
            get { return options.Shuffle; }
            set
            {
                options.Shuffle = value;
                RaisePropertyChanged("Shuffle");
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
                    return "repeatoff";
                }
                else if (RepeatTrack == 1)
                {
                    return "repeat";
                }
                else
                {
                    return "repeatonce";
                }
            }
        }

    }
}
