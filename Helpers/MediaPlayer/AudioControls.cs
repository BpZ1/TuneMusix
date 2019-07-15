using System;
using System.Diagnostics;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer
{
    /// <summary>
    /// This class contains all control elements for the audio player.
    /// The methods in this class connect the viewmodels with the audioplayer implementation.
    /// </summary>
    partial class AudioControls : IDisposable
    {
        private EffectQueue _effectQueue;

        private static AudioControls _instance;
        public static AudioControls Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioControls();
                }
                return _instance;
            }
        }

        private AudioPlayerImpl _player;
        private DataModel _dataModel = DataModel.Instance;
        private TrackQueue _trackQueue;
        private Options _options = Options.Instance;
        //list containing all loaded effects

        private bool isPlaying;

        private AudioControls()
        {
            _trackQueue = _dataModel.TrackQueue;
            _effectQueue = new EffectQueue();
            _effectQueue.QueueChanged += OnEffectsChanged;
            //Events
            _trackQueue.CurrentTrackChanged += OnCurrentTrackChanged;
            _options.BalanceChanged += OnBalanceChanged;
            _options.VolumeChanged += OnVolumeChanged;        
        }

        #region Events
        public delegate void AudioControlsEventHandler(object source);

        public event AudioControlsEventHandler TrackChanged;
        public event AudioControlsEventHandler Playing;
        public event AudioControlsEventHandler Stopped;
        public event AudioControlsEventHandler Paused;
        public event AudioControlsEventHandler PlaystateChanged;

        protected virtual void OnPlaying()
        {
            isPlaying = true;

            Playing?.Invoke(this);
            PlaystateChanged?.Invoke(this);

        }

        protected virtual void OnStopped()
        {
            isPlaying = false;

            Stopped?.Invoke(this);
            PlaystateChanged?.Invoke(this);
        }

        protected virtual void OnPaused()
        {
            isPlaying = false;

            Paused?.Invoke(this);
            PlaystateChanged?.Invoke(this);
        }

        protected virtual void OnTrackChanged()
        {
            TrackChanged?.Invoke(this);
            PlaystateChanged?.Invoke(this);
        }

        private void OnPlaybackFinished(object source)
        {
            //If no tracks are to be repeated
            if (_options.RepeatTrack == 0)
            {
                if (_trackQueue.QueueIndex != _trackQueue.Queue.Count - 1)
                {
                    PlayNext();
                }
            }
            //If all tracks are to be repeated
            else if (_options.RepeatTrack == 1)
            {
                PlayNext();
            }
            //If only one track is to be repeated
            else if (_options.RepeatTrack == 2)
            {
                PlayTrack(_trackQueue.CurrentTrack);
                this.Play();
            }

        }

        /// <summary>
        /// Sets the new current track after it was changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="NewCurrentTrack"></param>
        private void OnCurrentTrackChanged(object source, object NewCurrentTrack)
        {
            Debug.WriteLine("Current Track changed");
            if (NewCurrentTrack != null)
            {
                if (((Track)NewCurrentTrack).IsValid)
                {
                    this.PlayTrack(NewCurrentTrack as Track);
                }
                else
                {
                    OnPlaybackFinished(this);
                }
            }
            else
            {
                if (_player != null)
                    _player.Dispose();

                //If the track was playing before it was set to null play next track
                if (IsPlaying)
                {
                    PlayNext();
                }
                else //If the track was deleted but not playing, just go to the next track.
                {
                    PlayNext();
                    Pause();
                }

            }
        }

        private void OnVolumeChanged(object sender, object volume)
        {
            if (_player != null) _player.Volume = GetFloatVolume((int)volume);
        }

        private void OnBalanceChanged(object sender, object balance)
        {
            var Balance = (int)balance;
            this.Balance = Balance;
        }

        /// <summary>
        /// Restarts the player.
        /// </summary>
        /// <param name="source"></param>
        private void OnEffectsChanged(object source)
        {
            if (_player != null)
            {
                //setting the old position
                TimeSpan lastPosition = CurrentPosition;
                PlayTrack(_trackQueue.CurrentTrack);
                CurrentPosition = lastPosition;
                if (!IsPlaying)
                {
                    Pause();
                }
            }
        }
        #endregion

        #region Actions

        /// <summary>
        /// Plays the next song in the track queue.
        /// If the track is the last element of the queue
        /// the first track in the queue will be played.
        /// </summary>
        public void PlayNext()
        {
            _trackQueue.IncrementQueueIndex();
        }
        /// <summary>
        /// Plays the previous song in the track queue.
        /// If the current track is on index 0 in the queue
        /// the track on the last index will be played.
        /// </summary>
        public void PlayPrevious()
        {
            _trackQueue.DecrementQueueIndex();     
        }
        /// <summary>
        /// Private method called by PlayTrack to create a new player
        /// </summary>
        private void CreatePlayer(Track track)
        {
            if (track != null)
            {
                int volume = 0;
                if (_options.Muted)
                {
                    volume = 0;
                }
                else
                {
                    volume = _options.Volume;
                }

                _player = new AudioPlayerImpl(
                    track.SourceURL,
                    GetFloatVolume(volume),
                    _options.Balance,
                    true,
                    _effectQueue,
                    _options.EffectsActive);
                _player.PlaybackFinished += OnPlaybackFinished;
                OnTrackChanged();
            }
            else
            {
                throw new ArgumentNullException("The given track when creating a player can't be null");
            }
        }
        /// <summary>
        /// Creates a new Player and disposes of the old one if present.
        /// </summary>
        /// <param name="track"></param>
        public void PlayTrack(Track track)
        {
            if (_player != null)
            {
                _player.Dispose();
                Debug.WriteLine("Player Disposed");
            }
            CreatePlayer(track);
            this.Play();
        }

        /// <summary>
        /// Starts playback of the currently loaded track.
        /// </summary>
        public void Play()
        {
            if (_player != null)
            {
                _player.Play();
                OnPlaying();
            }
        }
        /// <summary>
        /// Pauses playback of the currently loaded track.
        /// </summary>
        public void Pause()
        {
            if (_player != null)
            {
                _player.Pause();
                OnPaused();
            }
        }
        /// <summary>
        /// Resumes playback of the currently loaded track.
        /// </summary>
        public void Resume()
        {
            if (_player != null)
            {
                _player.Resume();
                OnPlaying();
            }
        }
        /// <summary>
        /// Stops playback of the currently loaded track.
        /// </summary>
        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
                OnStopped();
            }
        }
        /// <summary>
        /// Disposes the current player.
        /// </summary>
        public void Dispose()
        {
            if (_player != null)
            {
                isPlaying = false;
                _player.Dispose();
            }
        }

        #endregion

        #region Fields

        public EffectQueue EffectQueue => _effectQueue;

        /// <summary>
        /// Returns true of the player is currently playing a track.
        /// </summary>
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        /// <summary>
        /// Returns true if the player is loaded and not null.
        /// </summary>  
        public bool IsLoaded
        {
            get
            {
                if (_player != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns the length of the current track.
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                if (_player != null) return _player.Length();
                else return new TimeSpan();
            }
        }
        /// <summary>
        /// The current position of the loaded track.
        /// </summary>
        public TimeSpan CurrentPosition
        {
            get
            {
                if (_player != null)
                {
                    return _player.CurrentPosition();
                }
                return new TimeSpan();
            }
            set
            {
                _player.SetCurrentPosition(value);
            }
        }
        public bool Mute
        {
            set
            {
                if (value)
                {
                    if (IsLoaded)
                        _player.Volume = 0;

                    _options.Muted = true;
                }
                if (!value)
                {
                    if (_player != null)
                        _player.Volume = GetFloatVolume(_options.Volume);

                    _options.Muted = false;
                }
            }
            get { return _options.Muted; }
        }
        /// <summary>
        /// sets the Balance from -100 to 100.
        /// </summary>
        public int Balance
        {
            set
            {
                _player.SetBalance(value);
            }
        }

        /// <summary>
        /// Returns true if a track is currently loaded.
        /// </summary>
        public bool TrackLoaded
        {
            get
            {
                if (_player != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        private float GetFloatVolume(int value)
        {
            float newvalue = (float)value;
            return newvalue / 100;
        }

        /// <summary>
        /// Shuffles the trackqueue when called.
        /// Also changes the state of shuffle.
        /// If the trackqueue is already shuffled it will be unshuffled and
        /// sorted again.
        /// </summary>  
        public void ShuffleChanged()
        {
            DialogResult result = DialogResult.Yes;
            if (_trackQueue.Queue.Count > 500)
            {
                result = DialogService.OpenDialog("Shuffling that many tracks can take a long time. Continue?");

            }
            if (result == DialogResult.Yes)
            {

                if (!_options.Shuffle)
                {
                    //Shuffle the list
                    _trackQueue.Shuffle();

                    //Change state after shuffling to avoid shuffle loop in setter of TrackQueue
                    _options.Shuffle = true;
                    Options.Instance.SaveValues();
                }
                else
                {
                    //Change state before unshuffling to avoid shuffling in setter of TrackQueue.
                    _options.Shuffle = false;
                    Options.Instance.SaveValues();

                    //Unshuffle the list
                    _trackQueue.UnShuffle();
                }
            }
        }

        /// <summary>
        /// Returns the FFT data array.
        /// </summary>
        /// <param name="fftDataBuffer"></param>
        /// <returns>FFT data array.</returns>
        public bool GetFFTData(float[] fftDataBuffer)
        {
            if(_player != null && isPlaying)
            {
                return _player.GetFftData(fftDataBuffer);
            }
            else
            {
                return false;
            }
        }
    }
}
