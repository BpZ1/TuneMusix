using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
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
        private EffectQueue effectQueue;

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
        private bool isPlaying;  

        public AudioControls()
        {
            //Events
            dataModel.CurrentTrackChanged += onCurrentTrackChanged;
            options.BalanceChanged += onBalanceChanged;
            options.VolumeChanged += onVolumeChanged;
            
        }

        public delegate void AudioControlsEventHandler(object source);

        public event AudioControlsEventHandler TrackChanged;
        public event AudioControlsEventHandler Playing;
        public event AudioControlsEventHandler Stopped;
        public event AudioControlsEventHandler Paused;
        public event AudioControlsEventHandler PlaystateChanged;

        protected virtual void OnPlaying()
        {
            isPlaying = true;

            if(Playing != null)
                Playing(this);

            if (PlaystateChanged != null)
                PlaystateChanged(this);

        }
        protected virtual void OnStopped()
        {
            isPlaying = false;

            if (Stopped != null)
                Stopped(this);

            if (PlaystateChanged != null)
                PlaystateChanged(this);
        }
        protected virtual void OnPaused()
        {
            isPlaying = false;

            if (Paused != null)
                Paused(this);

            if (PlaystateChanged != null)
                PlaystateChanged(this);
        }
        protected virtual void OnTrackChanged()
        {
            if (TrackChanged!= null)
                TrackChanged(this);

            if (PlaystateChanged != null)
                PlaystateChanged(this);
        }

        /// <summary>
        /// called on program start to create the effect queue.
        /// </summary>
        public void LoadEffects()
        {
            effectQueue = new EffectQueue();
            dataModel.EffectQueueChanged += onEffectQueueChanged;
        }
        private void onEffectQueueChanged(object source,object queue)
        {
            ObservableCollection<BaseEffect> effectQueue = queue as ObservableCollection<BaseEffect>;          
            this.effectQueue = new EffectQueue();
            foreach (BaseEffect effect in effectQueue)
            {
                this.effectQueue.AddEffect(effect);
            }
            if(Player != null)
            {
                TimeSpan lastPosition = CurrentPosition;
                PlayTrack(dataModel.CurrentTrack);
                CurrentPosition = lastPosition;
            }      
        }
        private void onPlaybackFinished(object source)
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
        /// <summary>
        /// Plays the next song in the track queue.
        /// </summary>
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
        private float getFloatVolume(int value)
        {
            float newvalue = (float)value;
            return newvalue / 100;
        }
        private void onVolumeChanged(object volume)
        {
            if (Player != null) Player.Volume = getFloatVolume((int)volume);
        }

        private void onBalanceChanged(object balance)
        {
            var Balance = (int)balance;
            this.Balance = Balance;
        }
        /// <summary>
        /// Sets the new current track after it was changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="NewCurrentTrack"></param>
        private void onCurrentTrackChanged(object source,object NewCurrentTrack)
        {
            Debug.WriteLine("Current Track changed");
            if (NewCurrentTrack != null)
            {
                this.PlayTrack(NewCurrentTrack as Track);
            }
            else
            {
                if(Player != null)
                    Player.Dispose();

                dataModel.QueueIndex--; //Correct queue index because of removed element
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
        /// <summary>
        /// Private method called by PlayTrack to create a new player
        /// </summary>
        private void createPlayer(Track track)
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
                    getFloatVolume(volume),
                    options.Balance,
                    true,
                    effectQueue,
                    true);
                Player.PlaybackFinished += onPlaybackFinished;
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
            if (Player!=null)
            {              
                Player.Dispose();
                Player = null;
                Debug.WriteLine("Player Disposed");
            }
            createPlayer(track);
            this.Play();
        }

        public bool IsPlaying
        {
            get{ return isPlaying; }
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
                isPlaying = false;
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
                    Player.Volume = getFloatVolume(options.Volume);
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

        /// <summary>
        /// Shuffles the trackqueue when called.
        /// Also changes the state of shuffle.
        /// If the trackqueue is already shuffled it will be unshuffled and
        /// sorted again.
        /// </summary>
        public void ShuffleChanged()
        {
            Track oldTrack = null;
            TimeSpan oldPosition = new TimeSpan();
            bool playing = IsPlaying;
            
            //If a track is currently playing backup the track and position
            bool hasCurrentTrack = false;
            if (dataModel.CurrentTrack != null)
                hasCurrentTrack = true;

            //Backup old track data
            if(hasCurrentTrack)
            {
                oldTrack = dataModel.CurrentTrack;
                oldPosition = CurrentPosition;
            }

            if (!options.Shuffle)
            {             
                //Shuffle the list
                dataModel.ShuffleTrackQueue();

                //Change state after shuffling to avoid shuffle loop in setter of TrackQueue
                options.Shuffle = true;
                dataModel.SaveOptions();
            }
            else
            {
                //Change state before unshuffling to avoid shuffling in setter of TrackQueue.
                options.Shuffle = false;
                dataModel.SaveOptions();

                //Unshuffle the list
                dataModel.UnShuffleTrackQueue();            
            }

            //Restore backup of track and position
            if (hasCurrentTrack)
            {
                dataModel.CurrentTrack = oldTrack;
                CurrentPosition = oldPosition;
                dataModel.QueueIndex = dataModel.TrackQueue.IndexOf(dataModel.CurrentTrack);
                if (playing)
                {
                    Play();
                }
                else
                {
                    Pause();
                }
            }
        }

    }
}
