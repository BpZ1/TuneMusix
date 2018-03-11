using CSCore.Streams.Effects;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer
{
    partial class AudioControls : IDisposable
    {
        private EffectQueue _effectQueue;

        private static AudioControls instance;
        public static AudioControls Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioControls();
                }
                return instance;
            }
        }

        private AudioPlayerImpl Player;
        private DataModel dataModel = DataModel.Instance;
        private Options options = Options.Instance;    

        public AudioControls()
        {
            //Events
            dataModel.CurrentTrackChanged += OnCurrentTrackChanged;
            options.BalanceChanged += OnBalanceChanged;
            options.VolumeChanged += OnVolumeChanged;
            
        }

        public delegate void AudioControlsEventHandler(object source);

        public event AudioControlsEventHandler TrackChanged;
        public event AudioControlsEventHandler Playing;
        public event AudioControlsEventHandler Stopped;
        public event AudioControlsEventHandler Paused;

        protected virtual void OnPlaying()
        {
            if(Playing != null)
            {
                Playing(this);
            }
        }
        protected virtual void OnStopped()
        {
            if (Stopped != null)
            {
                Stopped(this);
            }
        }
        protected virtual void OnPaused()
        {
            if (Paused != null)
            {
                Paused(this);
            }
        }
        protected virtual void OnTrackChanged()
        {
            if (TrackChanged!= null)
            {
                TrackChanged(this);
            }
        }

        public void LoadEffects()
        {
            _effectQueue = new EffectQueue();
            dataModel.EffectQueueChanged += OnEffectQueueChanged;
        }
        private void OnEffectQueueChanged(object source,object queue)
        {
            ObservableCollection<BaseEffect> effectQueue = queue as ObservableCollection<BaseEffect>;          
            _effectQueue = new EffectQueue();
            foreach (BaseEffect effect in effectQueue)
            {
                _effectQueue.AddEffect(effect);
            }
            if(Player != null)
            {
                TimeSpan lastPosition = CurrentPosition;
                PlayTrack(dataModel.CurrentTrack);
                CurrentPosition = lastPosition;
            }      
        }
        private void OnPlaybackFinished(object source)
        {
            if (options.RepeatTrack == 0)
            {
                if (dataModel.QueueIndex+1 < dataModel.TrackQueue.Count)
                {
                    PlayNext();
                }
            }
            if (options.RepeatTrack == 1)
            {
                if (dataModel.TrackQueue.Count == 1)
                {
                    PlayTrack(dataModel.CurrentTrack);
                }
                PlayNext();
            }
            if (options.RepeatTrack == 2)
            {
                PlayTrack(dataModel.CurrentTrack);
                this.Play();
            }
        }
        public void PlayNext()
        {          
            if(dataModel.TrackQueue.Count > 0)
            {
                dataModel.QueueIndex++;
                if (dataModel.QueueIndex + 1 <= dataModel.TrackQueue.Count)
                {
                    dataModel.CurrentTrack = dataModel.TrackQueue[dataModel.QueueIndex];
                }
                if (dataModel.QueueIndex + 1 > dataModel.TrackQueue.Count)
                {
                    dataModel.QueueIndex = 0;
                    dataModel.CurrentTrack = dataModel.TrackQueue[dataModel.QueueIndex];
                }
            }          
        }
        public void PlayPrevious()
        {
            if(dataModel.TrackQueue.Count > 0)
            {
                if (dataModel.QueueIndex > 0)
                {
                    dataModel.QueueIndex--;
                    dataModel.CurrentTrack = dataModel.TrackQueue[dataModel.QueueIndex];
                }
                else
                {
                    dataModel.QueueIndex = dataModel.TrackQueue.Count - 1;
                    dataModel.CurrentTrack = dataModel.TrackQueue[dataModel.QueueIndex];
                }
            }          
        }
        private float GetFloatVolume(int value)
        {
            float newvalue = (float)value;
            return newvalue / 100;
        }
        private void OnVolumeChanged(object volume)
        {
            if (Player != null) Player.Volume = GetFloatVolume((int)volume);
        }

        private void OnBalanceChanged(object balance)
        {
            var Balance = (int)balance;
            this.Balance = Balance;
        }

        private void OnCurrentTrackChanged(object source,object NewCurrentTrack)
        {
            Debug.WriteLine("Current Track changed");
            if (NewCurrentTrack != null)
            {
                this.PlayTrack(NewCurrentTrack as Track);
            }
            else
            {
                Player.Dispose();
            }
        }
        /// <summary>
        /// Private method called by PlayTrack
        /// </summary>
        private void CreatePlayer(Track track)
        {
            if(track != null)
            {
                int volume = 0;
                if (options.Muted)
                {
                    volume = 0;
                }
                else
                {
                    volume = options.Volume;
                }

                Player = new AudioPlayerImpl(
                    track.sourceURL,
                    GetFloatVolume(volume),
                    options.Balance,
                    true,
                    _effectQueue,
                    true);
                Player.PlaybackFinished += OnPlaybackFinished;
                OnTrackChanged();
            }
        }
        /// <summary>
        /// Creates a new Player and disposes of the old one if present.
        /// </summary>
        /// <param name="track"></param>
        public void PlayTrack(Track track)
        {
            if (Player!=null)
            {
                Debug.WriteLine("Player Disposed");
                Player.Dispose();
                Player = null;
            }
            CreatePlayer(track);
            this.Play();
        }

        public bool IsPlaying
        {
            get
            {
                if(Player != null)
                {
                    return Player.IsPlaying();
                }
                else
                {
                    return false;
                }
                
            }
        }
        public bool IsLoaded
        {
            get
            {
                if (Player != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void Play()
        {
            if (Player != null)
            {
                Player.Play();
                OnPlaying();
            }          
        }
        public void Pause()
        {
            if (Player != null)
            {
                Player.Pause();
                OnPaused();
            }           
        }
        public void Resume()
        {
            if (Player != null)
            {
                Player.Resume();
                OnPlaying();
            }         
        }
        public void Stop()
        {
            if (Player != null)
            {
                Player.Stop();
                OnStopped();
            }
        }
        public void Dispose()
        {
            if (Player != null)
            {
                Player.Dispose();
            }
        }
        public TimeSpan Length
        {
            get
            {
                if (Player != null) return Player.Length();
                else return new TimeSpan();
            }
        }
        public TimeSpan CurrentPosition
        {
            get
            {
                if(Player != null)
                {
                    return Player.CurrentPosition();
                }
                return new TimeSpan();
            }
            set
            {
                Player.SetCurrentPosition(value);
            }
        }
        public bool Mute
        {
            set
            {
                if (value)
                {
                    if(IsLoaded)
                        Player.Volume = 0;

                    options.Muted = true;
                }
                if (!value)
                {
                    Player.Volume = GetFloatVolume(options.Volume);
                    options.Muted = false;
                }
            }
            get { return options.Muted; }
        }
        /// <summary>
        /// sets the Balance from -100 to 100
        /// </summary>
        public int Balance
        {
            set
            {
                Player.SetBalance(value);
            }
        }

        public bool TrackLoaded
        {
            get
            {
                if (Player != null)
                {
                    return true;
                }
                else
                { 
                    return false;
                }
            }
        }



    }
}
