using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
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

        
        private static volatile Options instance;
        private static object lockObject = new Object();

        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Options();
                        }
                    }
                }
                return instance;
            }
        }

        private Options() { }

        private bool modified;
        private bool effectsActive = true;
        private bool loggerActive;
        private int volume;      
        private bool shuffle;
        private bool askConfirmation = true;
        private bool muted = false;
        //Values of the swatch for the database.
        private bool theme;
        private int primaryColorIndex;
        private int accentColorIndex;

        public bool Modified
        {
            private get { return modified; }
            set { modified = value; }
        }

        //tracks in queue will shuffle randomly when set to true.
        public bool Shuffle
        {
            get { return shuffle; }
            set
            {
                shuffle = value;
            }
        }
        // 0 = No repeat
        // 1 = Repeat all
        // 2 = repeat track
        private int repeatTrack;
        //balance of the playback
        private int balance;
        private bool isStereo;
       
        /// <summary>
        /// Boolean representing the current muted option.
        /// </summary>
        public bool Muted
        {
            get { return muted; }
            set
            {
                muted = value;
                SaveValues();
            }
        }

        #region events
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
                EffectsActiveChanged(effectsActive);
            }
        }
        protected virtual void OnIsStereoChanged()
        {
            if (IsStereoChanged != null)
            {
                IsStereoChanged(isStereo);
            }
        }
        #endregion

        #region getter and setter
        /// <summary>
        /// Defines the primary color for the application.
        /// </summary>
        public Swatch PrimaryColor
        {
            set
            {
                if(value != null)
                {
                    var paletteHelper = new PaletteHelper();
                    paletteHelper.ReplacePrimaryColor(value);

                    //Get swatch from list to get index.
                    var swatchList = new SwatchesProvider().Swatches.ToList();
                    var swatch = swatchList.Single(s => s.Name.Equals(value.Name));
                    primaryColorIndex = swatchList.IndexOf(swatch);
                    Modified = true;
                }
            }
        }

        /// <summary>
        /// Defines the accent color for the application.
        /// </summary>
        public Swatch AccentColor
        {
            set
            {
                if (value != null)
                {
                    var paletteHelper = new PaletteHelper();
                    paletteHelper.ReplaceAccentColor(value);

                    //Get swatch from list to get index.
                    var swatchList = new SwatchesProvider().Swatches.ToList();
                    var swatch = swatchList.Single(s =>  s.Name.Equals(value.Name) );
                    accentColorIndex = swatchList.IndexOf(swatch);
                    Modified = true;
                }
            }
        }

        /// <summary>
        /// Defines the theme of the application.
        /// </summary>
        public bool Theme
        {
            set
            {
                new PaletteHelper().SetLightDark(value);
                theme = value;
                Modified = true;
            }
            get { return theme; }
        }
        /// <summary>
        /// Returns the index of the swatch that is set as accent color.
        /// </summary>
        public int PrimaryColorIndex
        {
            get { return primaryColorIndex; }
        }
        /// <summary>
        /// Returns the index of the swatch that is set as accent color.
        /// </summary>
        public int AccentColorIndex
        {
            get { return accentColorIndex; }
        }

        public bool IsStereo
        {
            get { return isStereo; }
            set
            {
                isStereo = value;
                OnIsStereoChanged();
            }
        }
        /// <summary>
        /// If false, all effects are disabled.
        /// </summary>
        public bool EffectsActive
        {
            get { return effectsActive; }
            set
            {
                this.effectsActive = value;
                Modified = true;
            }
        }
        /// <summary>
        /// Changes the volume of the playback
        /// </summary>
        public int Volume
        {
            get { return this.volume; }
            set
            {
                //Volume is saved in an event that is called when the slider
                //is released. 
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
        /// <summary>
        /// Not Implemented
        /// </summary>
        public int Balance
        {
            get { return this.balance; }
            set
            {
                this.balance = value;
                //Needs event that is called when adjustment on slider is finished to save the value.
                OnBalanceChanged();
            }
        }
        /// <summary>
        /// Avtivates or deactivates the logger that saves logging in a file
        /// in the directory.
        /// </summary>
        public bool LoggerActive
        {
            get { return this.loggerActive; }
            set
            {
                this.loggerActive = value;
                Modified = true;
            }
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
                SaveValues();
                OnRepeatChanged();
            }
        }
        /// <summary>
        /// If this is set to false, confirmation dialogs will be disabled.
        /// </summary>
        public bool AskConfirmation
        {
            get { return askConfirmation; }
            set
            {
                askConfirmation = value;
                Modified = true;
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
                Database manager = Database.Instance;
                manager.UpdateOptions(IDGenerator.GetID(false), this);
                manager.UpdateEffectQueue(dataModel.EffectQueue.ToList<BaseEffect>());
                Modified = false;
                foreach (BaseEffect effect in dataModel.EffectQueue)
                {
                    effect.IsModified = false;
                }
                Logger.Log("Options and effects saved to database");
            }
        }

        public void SaveValues()
        {
            Database manager = Database.Instance;
            manager.UpdateOptions(IDGenerator.GetID(false), this);
            Logger.Log("Options saved to database");
        }

        public void SetOptions(int volume, bool shuffle, int repeatTrack, int primaryColorIndex, int accentColorIndex, bool theme, bool askConfirmation)
        {
            this.volume = volume;
            this.shuffle = shuffle;
            this.repeatTrack = repeatTrack;
            this.askConfirmation = askConfirmation;
            this.primaryColorIndex = primaryColorIndex;
            this.accentColorIndex = accentColorIndex;
            this.theme = theme;

            //set colors
            var swatches = new SwatchesProvider().Swatches.ToArray();
            var palette = new PaletteHelper();
            palette.ReplacePrimaryColor(swatches[primaryColorIndex]);
            palette.ReplaceAccentColor(swatches[accentColorIndex]);
            palette.SetLightDark(theme);

            OnRepeatChanged();
            OnVolumeChanged();
        }

        
    }
}
