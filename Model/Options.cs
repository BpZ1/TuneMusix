using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.Model
{
    /// <summary>
    /// This class contains all properties that are needed in almost all parts of the program
    /// and have to be saved to the database.
    /// </summary>
    public partial class Options
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

        private bool modified = false;
        public bool Modified
        {
            private get { return modified; }
            set { modified = value; }
        }

        private bool _effectsActive;
        //Normal logging is only active when set to true.
        private bool _LoggerActive = false;
        //volume of the audioplayer
        private int volume;
        //tracks in queue will shuffle randomly when set to true.
        private bool shuffle;
        public bool Shuffle
        {
            get { return shuffle; }
            set
            {
                shuffle = value;
                //TODO save to database
            }
        }
        // 0 = No repeat
        // 1 = Repeat all
        // 2 = repeat track
        private int repeatTrack;
        //balance of the playback
        private int balance;
        private bool isStereo;
       
        private bool muted = false;
        public bool Muted
        {
            get { return muted; }
            set
            {
                muted = value;
                //TODO save to database
            }
        }

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
                IsStereoChanged(isStereo);
            }
        }


        #region getter and setter
        //Getter and setter
        public bool IsStereo
        {
            get { return isStereo; }
            set
            {
                isStereo = value;
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
            get { return this.volume; }
            set
            {
                if (value > 100)
                {                    
                    this.volume = 100;
                    OnVolumeChanged();
                }
                else
                {
                    this.volume = value;
                    OnVolumeChanged();
                }

            }
        }
        public int Balance
        {
            get { return this.balance; }
            set
            {
                this.balance = value;
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
                return this.repeatTrack;
            }
            set
            {
                this.repeatTrack = value;
                OnRepeatChanged();
            }
        }
        #endregion
  
        /// <summary>
        /// Returns true if any value in options or the effectqueue has changed.
        /// </summary>
        /// <returns></returns>
        public bool IsModified()
        {
            if (Modified)
                return true;

            foreach(BaseEffect effect in DataModel.Instance.EffectQueue)
            {
                if (effect.IsModified)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Saves all options to the database.
        /// </summary>
        public void Save()
        {
            if (IsModified())
            {
                DataModel dataModel = DataModel.Instance;
                SQLManager manager = new SQLManager();
                manager.UpdateOptions(IDGenerator.GetID(false), this);
                manager.UpdateEffectQueue(dataModel.EffectQueue.ToList<BaseEffect>());
                Modified = false;
                foreach (BaseEffect effect in dataModel.EffectQueue)
                {
                    effect.IsModified = false;
                }
            }
        }
        /// <summary>
        /// Saves the volume value to the database
        /// </summary>
        public void SaveVolume()
        {

        }

    }
}
