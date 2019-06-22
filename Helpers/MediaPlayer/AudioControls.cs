using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TuneMusix.Data;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.Exceptions;
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

        private AudioPlayerImpl player;
        private DataModel dataModel = DataModel.Instance;
        private Options options = Options.Instance;
        //list containing all loaded effects

        private bool isPlaying;  

        private AudioControls()
        {
            effectQueue = new EffectQueue();
            effectQueue.QueueChanged += onEffectsChanged;
            //Events
            dataModel.CurrentTrackChanged += onCurrentTrackChanged;
            options.BalanceChanged += onBalanceChanged;
            options.VolumeChanged += onVolumeChanged;
            
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
        /// Sets the new current track after it was changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="NewCurrentTrack"></param>
        private void onCurrentTrackChanged(object source, object NewCurrentTrack)
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
                    onPlaybackFinished(this);
                }
            }
            else
            {
                if (player != null)
                    player.Dispose();

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

        private void onVolumeChanged(object volume)
        {
            if (player != null) player.Volume = getFloatVolume((int)volume);
        }

        private void onBalanceChanged(object balance)
        {
            var Balance = (int)balance;
            this.Balance = Balance;
        }

        /// <summary>
        /// Restarts the player.
        /// </summary>
        /// <param name="source"></param>
        private void onEffectsChanged(object source)
        {
            if (player != null)
            {
                //setting the old position
                TimeSpan lastPosition = CurrentPosition;
                PlayTrack(dataModel.CurrentTrack);
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
        /// <summary>
        /// Plays the previous song in the track queue.
        /// If the current track is on index 0 in the queue
        /// the track on the last index will be played.
        /// </summary>
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
        /// <summary>
        /// Private method called by PlayTrack to create a new player
        /// </summary>
        private void createPlayer(Track track)
        {
            if (track != null)
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

                player = new AudioPlayerImpl(
                    track.SourceURL,
                    getFloatVolume(volume),
                    options.Balance,
                    true,
                    effectQueue,
                    options.EffectsActive);
                player.PlaybackFinished += onPlaybackFinished;
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
            if (player != null)
            {
                player.Dispose();
                player = null;
                Debug.WriteLine("Player Disposed");
            }
            createPlayer(track);
            this.Play();
        }

        /// <summary>
        /// Starts playback of the currently loaded track.
        /// </summary>
        public void Play()
        {
            if (player != null)
            {
                player.Play();
                OnPlaying();
            }
        }
        /// <summary>
        /// Pauses playback of the currently loaded track.
        /// </summary>
        public void Pause()
        {
            if (player != null)
            {
                player.Pause();
                OnPaused();
            }
        }
        /// <summary>
        /// Resumes playback of the currently loaded track.
        /// </summary>
        public void Resume()
        {
            if (player != null)
            {
                player.Resume();
                OnPlaying();
            }
        }
        /// <summary>
        /// Stops playback of the currently loaded track.
        /// </summary>
        public void Stop()
        {
            if (player != null)
            {
                player.Stop();
                OnStopped();
            }
        }
        /// <summary>
        /// Disposes the current player.
        /// </summary>
        public void Dispose()
        {
            if (player != null)
            {
                isPlaying = false;
                player.Dispose();
            }
        }

        #endregion

        #region Fields

        public EffectQueue EffectQueue
        {
            get
            {
                return effectQueue;
            }
        }

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
                if (player != null)
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
                if (player != null) return player.Length();
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
                if (player != null)
                {
                    return player.CurrentPosition();
                }
                return new TimeSpan();
            }
            set
            {
                player.SetCurrentPosition(value);
            }
        }
        public bool Mute
        {
            set
            {
                if (value)
                {
                    if (IsLoaded)
                        player.Volume = 0;

                    options.Muted = true;
                }
                if (!value)
                {
                    if (player != null)
                        player.Volume = getFloatVolume(options.Volume);

                    options.Muted = false;
                }
            }
            get { return options.Muted; }
        }
        /// <summary>
        /// sets the Balance from -100 to 100.
        /// </summary>
        public int Balance
        {
            set
            {
                player.SetBalance(value);
            }
        }

        /// <summary>
        /// Returns true if a track is currently loaded.
        /// </summary>
        public bool TrackLoaded
        {
            get
            {
                if (player != null)
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

        private float getFloatVolume(int value)
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
            if (dataModel.TrackQueue.Count > 500)
            {
                result = DialogService.OpenDialog("Shuffling that many tracks can take a long time. Continue?");

            }
            if (result == DialogResult.Yes)
            {

                if (!options.Shuffle)
                {
                    //Shuffle the list
                    dataModel.ShuffleTrackQueue();

                    //Change state after shuffling to avoid shuffle loop in setter of TrackQueue
                    options.Shuffle = true;
                    Options.Instance.SaveValues();
                }
                else
                {
                    //Change state before unshuffling to avoid shuffling in setter of TrackQueue.
                    options.Shuffle = false;
                    Options.Instance.SaveValues();

                    //Unshuffle the list
                    dataModel.UnShuffleTrackQueue();
                }
            }
        }


        public bool GetFFTData(float[] fftDataBuffer)
        {
            if(player != null && isPlaying)
            {
                return player.GetFftData(fftDataBuffer);
            }
            else
            {
                return false;
            }
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            double maxFrequency = 22050d;
            if (player != null)
            {
                maxFrequency = player.SampleRate / 2.0d;          
            }
            return (int)((frequency / maxFrequency) * 1024d);
        }
    }
}
