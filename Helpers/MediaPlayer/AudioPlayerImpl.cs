using System;
using System.Diagnostics;
using TuneMusix.Helpers.MediaPlayer;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Implementation of the audio player.
    /// Every instance of this class represents a loaded track and has to be disposed
    /// when a new track is loaded.
    /// </summary>
    public class AudioPlayerImpl : AudioPlayer, IDisposable
    {
      
        public bool Repeat { get; set; }
       
        private IWaveSource soundSource;
        private ISoundOut soundOut;
        private bool isInitialized;


        public AudioPlayerImpl(string url, float volume, int balance, bool isStereo, EffectQueue effects, bool effectsActive)
        {
            soundOut = getSoundOut();
            soundSource = getSoundSource(url);

            //Apply effects if they are activated
            if (effectsActive)
               soundSource = effects.Apply(soundSource);
         
            if(soundSource != null)
            {
                soundOut.Initialize(soundSource);
                soundOut.Volume = volume;
                isInitialized = true;
                soundOut.Stopped += PlaybackStopped;
                if (!isStereo)
                    soundSource.ToMono();
            }              
            //FOR VISUALISATION: CSCore.DSG.fft ... 
        }
        /// <summary>
        /// Checks if Wasapi is supported and then uses that or directsound as ISoundOut.
        /// </summary>
        /// <returns></returns>
        private ISoundOut getSoundOut()
        {           
            if (WasapiOut.IsSupportedOnCurrentPlatform)
            {
                Debug.WriteLine("Using Wasapi");
                return new WasapiOut();
            }
            else
            {
                Debug.WriteLine("Using Direct sound");
                return new DirectSoundOut();
            }
        }

        private IWaveSource getSoundSource(string URL)
        {
            IWaveSource waveSource;
            try
            {
                waveSource = CodecFactory.Instance.GetCodec(URL);      
            }catch
            {
                DialogService.WarnMessage("Could not play track","The track '" + URL +"' could not be played.");
                return null;
            }
            return waveSource;
        }

        //Eventhandler
        public delegate void AudioPlayerEventHandler(object source);

        public event AudioPlayerEventHandler PlaybackFinished;

        protected virtual void OnPlaybackFinished()
        {
            if (PlaybackFinished != null)
            {
                PlaybackFinished(this);
            }
        }

        /// <summary>
        /// Method that is called when the playback has stopped.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void PlaybackStopped(object source,EventArgs args)
        {           
            if(soundSource == null || soundOut == null)
            {
                OnPlaybackFinished();
            }
            if(soundOut.WaveSource != null)
            {
                try
                {
                    if ((TimeSpan.Compare(soundSource.GetPosition(), soundSource.GetLength())) == 0)
                    {
                        OnPlaybackFinished();
                    }
                }
                catch (NullReferenceException ex)
                {
                    Logger.LogException(ex);
                }
                                   
            }          
        }
        /// <summary>
        /// Pauses the playback of the current track.
        /// </summary>
        public void Pause()
        {
            if (isInitialized)
            {
                soundOut.Pause();
            }
        }
        /// <summary>
        /// Resumes the playback of the current track.
        /// </summary>
        public void Resume()
        {
            if (isInitialized)
            {
                soundOut.Resume();
            }
        }
        /// <summary>
        /// Starts the playback if the player is initialized with a track.
        /// </summary>
        public void Play()
        {
            if (isInitialized)
            {
                soundOut.Play();             
            }
        }
        /// <summary>
        /// Stops the current playback.
        /// </summary>
        public void Stop()
        {
            if (isInitialized)
            {
                soundOut.Stop();
            }
        }
        /// <summary>
        /// Volume of the current playback
        /// </summary>
        public float Volume
        {
            set
            {
                if (isInitialized)
                    soundOut.Volume = value;
            }
        }
        /// <summary>
        /// Returns true if the player is currently playing a track.
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            if(soundOut.PlaybackState.Equals(PlaybackState.Playing))
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
        /// <summary>
        /// The current point in time for the playback of the current track.
        /// </summary>
        /// <returns></returns>
        public TimeSpan CurrentPosition()
        {
            if (isInitialized)
            {
                return soundSource.GetPosition();
            }
            else return new TimeSpan();
        }
        /// <summary>
        /// Sets the current position in time for the playback of the current track.
        /// </summary>
        /// <param name="pos"></param>
        public void SetCurrentPosition(TimeSpan pos)
        {
            if (isInitialized)
            {
                soundSource.SetPosition(pos);
            }
        }
        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="balance"></param>
        public void SetBalance(int balance)
        {
            if (isInitialized)
            {
            }
        }
        /// <summary>
        /// The total length of the currently playing track.
        /// </summary>
        /// <returns></returns>
        public TimeSpan Length()
        {
            if (isInitialized)
            {
                return soundSource.GetLength();
            }
            else return new TimeSpan();
        }
        /// <summary>
        /// Disposes the current audio playback.
        /// </summary>
        public void Dispose()
        {
            if (isInitialized)
            {
                soundOut.Dispose();
                soundSource.Dispose();
                isInitialized = false;
            }
        }

    }
}
