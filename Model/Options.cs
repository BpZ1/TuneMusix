﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.Model
{
    /// <summary>
    /// This class contains all properties that are needed in more parts of the program
    /// and have to be saved to the database.
    /// </summary>
    public partial class Options : INotifyPropertyChanged
    {
        private static Options instance;

        private Options() { }

        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Options();
                }
                return instance;
            }
        }

        public bool IsModified { get; set; } = false;
        private bool _effectsActive;
        //Normal logging is only active when set to true.
        private bool _LoggerActive = false;
        //volume of the audioplayer
        private int _volume;
        //tracks in queue will shuffle randomly when set to true.
        public bool Shuffle { get; set; }
        // 0 = No repeat
        // 1 = Repeat all
        // 2 = repeat track
        private int _repeatTrack;
        //balance of the playback
        private int _balance;
        private bool _isStereo;


        //events
        public delegate void OptionsEventHandler(object changed);

        public event OptionsEventHandler VolumeChanged;
        public event OptionsEventHandler RepeatChanged;
        public event OptionsEventHandler BalanceChanged;
        public event OptionsEventHandler EffectsActiveChanged;
        public event OptionsEventHandler IsStereoChanged;

        protected virtual void OnVolumeChanged()
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(this.Volume);
            }
        }
        protected virtual void OnRepeatChanged()
        {
            if (RepeatChanged != null)
            {
                RepeatChanged(this.RepeatTrack);
            }
        }
        protected virtual void OnBalanceChanged()
        {
            if (BalanceChanged != null)
            {
                BalanceChanged(this.Balance);
            }
        }
        protected virtual void OnEffectsActiveChanged()
        {
            if (EffectsActiveChanged != null)
            {
                EffectsActiveChanged(_effectsActive);
            }
        }
        protected virtual void OnIsStereoChanged()
        {
            if (IsStereoChanged != null)
            {
                IsStereoChanged(_isStereo);
            }
        }

        //Getter and setter
        public bool IsStereo
        {
            get { return _isStereo; }
            set
            {
                _isStereo = value;
                OnIsStereoChanged();
            }
        }
        public bool EffectsActive
        {
            get { return _effectsActive; }
            set
            {
                _effectsActive = value;
                OnEffectsActiveChanged();
            }
        }
        public int Volume
        {
            get { return this._volume; }
            set
            {
                if (value > 100)
                {                    
                    this._volume = 100;
                    OnVolumeChanged();
                }
                else
                {
                    this._volume = value;
                    OnVolumeChanged();
                }

            }
        }
        public int Balance
        {
            get { return this._balance; }
            set
            {
                this._balance = value;
                OnBalanceChanged();
            }
        }
        public bool LoggerActive
        {
            get { return this._LoggerActive; }
            set { this._LoggerActive = value; }
        }
        /// <summary>
        /// 0 = No repeat
        /// 1 = Repeat all
        /// 2 = repeat track
        /// </summary>
        public int RepeatTrack
        {
            get
            {
                return this._repeatTrack;
            }
            set
            {
                this._repeatTrack = value;
                OnRepeatChanged();
            }
        }


        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }

    }
}
