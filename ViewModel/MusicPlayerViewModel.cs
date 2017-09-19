using ControlzEx;
using System;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
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

        private double _currentPosition;
        private Timer timer;
        private bool Dragging;

        //Constructor
        public MusicPlayerViewModel()
        {
            Dragging = false;
            timer = new Timer(100);
            

            //RelayCommands
            LeftMouseDown_Slider = new RelayCommand(_leftMouseDown_Slider);
            LeftMouseUp_Slider = new RelayCommand(_leftMouseUp_Slider);
            PlayButton = new RelayCommand(_playButton);
            NextTrack = new RelayCommand(_nextTrack);
            PreviousTrack = new RelayCommand(_previousTrack);
            RepeatButton = new RelayCommand(_onRepeatButtonClicked);

            //Events
            timer.Elapsed += OnTimeElapsed;
            audioControls.TrackChanged += OnTrackChanged;
        }

        //Getter and setter  
        public string PlayButtonIcon
        {
            get
            {
                Console.WriteLine("ISPLAYING: " + audioControls.IsPlaying);
                if (audioControls.IsPlaying)
                {
                    return "PauseCircleOutline";
                }
                else
                {
                    return "PlayCircleOutline";
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
                return audioControls.CurrentPosition.TotalSeconds;
            }
            set
            {
                audioControls.CurrentPosition = TimeSpan.FromSeconds(value);
            }
        }


        public int Balance
        {
            set { }
        }
        public bool Muted
        {
            set { }
        }
        public double Length
        {
            get
            {
                return audioControls.Length.TotalSeconds;
            }
        }
        public int Volume
        {
            get
            {
                return options.Volume;
            }
            set
            {
                options.Volume = value;
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
