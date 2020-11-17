using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.Model
{
    /// <summary>
    /// This class contains all properties that are needed in almost all parts of the program
    /// and have to be saved to the database.
    /// </summary>
    public partial class Options
    {
        private static volatile Options _instance;
        private static object _lockObject = new Object();

        public static Options Instance
        {
            get
            {
                if ( _instance == null )
                {
                    lock ( _lockObject )
                    {
                        if ( _instance == null )
                        {
                            _instance = new Options();
                        }
                    }
                }
                return _instance;
            }
        }

        private Options() { }

        private bool _effectsActive = true;
        private bool _loggerActive;
        private int _volume;
        private bool _askConfirmation = true;
        private bool _muted = false;
        //Values of the swatch for the database.
        private bool _theme;

        public delegate void OptionsChangedEventHandler( object sender );

        public event OptionsChangedEventHandler ColorChanged;

        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke( this );
        }

        // 0 = No repeat
        // 1 = Repeat all
        // 2 = repeat track
        private RepeatType _repeatTrack;
        //balance of the playback
        private int balance;
        private bool isStereo;

        /// <summary>
        /// Boolean representing the current muted option.
        /// </summary>
        public bool Muted
        {
            get { return _muted; }
            set
            {
                _muted = value;
                SaveValues();
            }
        }

        #region events
        public delegate void OptionsEventHandler( object sender, object changed );

        public event OptionsEventHandler VolumeChanged;
        public event OptionsEventHandler RepeatChanged;
        public event OptionsEventHandler BalanceChanged;
        public event OptionsEventHandler EffectsActiveChanged;
        public event OptionsEventHandler IsStereoChanged;

        protected virtual void OnVolumeChanged()
        {
            VolumeChanged?.Invoke( this, this.Volume );
        }
        protected virtual void OnRepeatChanged()
        {
            RepeatChanged?.Invoke( this, this.RepeatTrack );
        }
        protected virtual void OnBalanceChanged()
        {
            BalanceChanged?.Invoke( this, this.Balance );
        }
        protected virtual void OnEffectsActiveChanged()
        {
            EffectsActiveChanged?.Invoke( this, _effectsActive );
        }
        protected virtual void OnIsStereoChanged()
        {
            IsStereoChanged?.Invoke( this, isStereo );
        }
        #endregion

        #region Properties
        public bool Modified { get; set; }
        //tracks in queue will shuffle randomly when set to true.
        public bool Shuffle { get; set; }
        /// <summary>
        /// Defines the primary color for the application.
        /// </summary>
        public Swatch PrimaryColor
        {
            set
            {
                if ( value != null )
                {
                    var paletteHelper = new PaletteHelper();
                    paletteHelper.ReplacePrimaryColor( value );

                    //Get swatch from list to get index.
                    var swatchList = new SwatchesProvider().Swatches.ToList();
                    var swatch = swatchList.Single( s => s.Name.Equals( value.Name ) );
                    PrimaryColorIndex = swatchList.IndexOf( swatch );
                    OnColorChanged();
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
                if ( value != null )
                {
                    var paletteHelper = new PaletteHelper();
                    paletteHelper.ReplaceAccentColor( value );

                    //Get swatch from list to get index.
                    var swatchList = new SwatchesProvider().Swatches.ToList();
                    var swatch = swatchList.Single( s => s.Name.Equals( value.Name ) );
                    AccentColorIndex = swatchList.IndexOf( swatch );
                    OnColorChanged();
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
                new PaletteHelper().SetLightDark( value );
                _theme = value;
                OnColorChanged();
                Modified = true;
            }
            get { return _theme; }
        }
        /// <summary>
        /// Returns the index of the swatch that is set as accent color.
        /// </summary>
        public int PrimaryColorIndex { get; private set; }
        /// <summary>
        /// Returns the index of the swatch that is set as accent color.
        /// </summary>
        public int AccentColorIndex { get; private set; }

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
            get { return _effectsActive; }
            set
            {
                this._effectsActive = value;
                Modified = true;
            }
        }
        /// <summary>
        /// Changes the volume of the playback
        /// </summary>
        public int Volume
        {
            get { return this._volume; }
            set
            {
                //Volume is saved in an event that is called when the slider
                //is released. 
                if ( value > 100 )
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
            get { return this._loggerActive; }
            set
            {
                this._loggerActive = value;
                Modified = true;
            }
        }
        /// <summary>
        /// 0 = No repeat
        /// 1 = Repeat all
        /// 2 = repeat track
        /// </summary>
        public RepeatType RepeatTrack
        {
            get
            {
                return _repeatTrack;
            }
            set
            {
                _repeatTrack = value;
                SaveValues();
                OnRepeatChanged();
            }
        }
        /// <summary>
        /// If this is set to false, confirmation dialogs will be disabled.
        /// </summary>
        public bool AskConfirmation
        {
            get { return _askConfirmation; }
            set
            {
                _askConfirmation = value;
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
            if ( Modified )
                return true;

            if ( AudioControls.Instance.EffectQueue.IsModified )
                return true;

            foreach ( BaseEffect effect in AudioControls.Instance.EffectQueue.Effectlist )
            {
                if ( effect.IsModified )
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Saves all options to the database.
        /// </summary>
        public void Save()
        {
            if ( IsModified() )
            {
                DataModel dataModel = DataModel.Instance;
                Database manager = Database.Instance;
                manager.UpdateOptions( IDGenerator.GetID( false ), this );
                manager.UpdateEffectQueue( AudioControls.Instance.EffectQueue.Effectlist.ToList<BaseEffect>() );
                Modified = false;
                AudioControls.Instance.EffectQueue.IsModified = false;
                foreach ( BaseEffect effect in AudioControls.Instance.EffectQueue.Effectlist )
                {
                    effect.IsModified = false;
                }
                Logger.Log( "Options and effects saved to database" );
            }
        }

        public void SaveValues()
        {
            Database manager = Database.Instance;
            manager.UpdateOptions( IDGenerator.GetID( false ), this );
            Logger.Log( "Options saved to database" );
        }

        public void SetOptions( int volume, bool shuffle, RepeatType repeatTrack, int primaryColorIndex, int accentColorIndex, bool theme, bool askConfirmation )
        {
            this._volume = volume;
            this.Shuffle = shuffle;
            this._repeatTrack = repeatTrack;
            this._askConfirmation = askConfirmation;
            this.PrimaryColorIndex = primaryColorIndex;
            this.AccentColorIndex = accentColorIndex;
            this._theme = theme;

            //set colors
            var swatches = new SwatchesProvider().Swatches.ToArray();
            var palette = new PaletteHelper();
            palette.ReplacePrimaryColor( swatches[primaryColorIndex] );
            palette.ReplaceAccentColor( swatches[accentColorIndex] );
            palette.SetLightDark( theme );

            OnColorChanged();
            OnRepeatChanged();
            OnVolumeChanged();
        }


    }
}
