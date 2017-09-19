using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Options
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

        private bool _LoggerActive = false;
        private int _volume;
        public bool Shuffle { get; set; }
        private int _repeatTrack;
        private int _balance;

        public delegate void OptionsEventHandler(object changed);

        public event OptionsEventHandler VolumeChanged;
        public event OptionsEventHandler RepeatChanged;
        public event OptionsEventHandler BalanceChanged;

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

        //Getter and setter
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




    }
}
