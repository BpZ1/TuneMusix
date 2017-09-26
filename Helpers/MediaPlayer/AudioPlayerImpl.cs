using System;
using System.Diagnostics;
using TuneMusix.Helpers.MediaPlayer;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Implementation of the audio player.
    /// Every instance of this class represents a loaded track and has to be disposed
    /// when a new track is loaded.
    /// </summary>
    public class AudioPlayerImpl : AudioPlayer,IDisposable
    {
      
        public bool Repeat { get; set; }
        private IWaveSource soundSource;
        private ISoundOut soundOut;
        private bool isInitialized;


        public AudioPlayerImpl(string url,float volume,bool isStereo)
        {
            soundOut = GetSoundOut();
            soundSource = GetSoundSource(url);

            soundOut.Initialize(soundSource);
            soundOut.Volume = volume;
            if (!isStereo)
            {
               soundSource.ToMono();
            }
            isInitialized = true;

            soundOut.Stopped += PlaybackStopped;
        }
        /// <summary>
        /// Checks if Wasapi is supported and then uses that or directsound as ISoundOut.
        /// </summary>
        /// <returns></returns>
        private ISoundOut GetSoundOut()
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

        private IWaveSource GetSoundSource(string URL)
        {
            try
            {
                return CodecFactory.Instance.GetCodec(URL);
            }catch
            {
                DialogService.WarnMessage("Could not play track","The track '" + URL +"' could not be played.");
                return null;
            }
           
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
                    if (soundSource.GetPosition() == soundSource.GetLength())
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
        public void Pause()
        {
            if (isInitialized)
            {
                soundOut.Pause();
            }
        }
        public void Resume()
        {
            if (isInitialized)
            {
                soundOut.Resume();
            }
        }
        public void Play()
        {
            if (isInitialized)
            {
                soundOut.Play();             
            }
        }
        public void Stop()
        {
            if (isInitialized)
            {
                soundOut.Stop();
            }
        }
        public float Volume
        {
            set
            {
                if (isInitialized)
                {
                    soundOut.Volume = value;
                }
            }
        }
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
        public TimeSpan CurrentPosition()
        {
            if (isInitialized)
            {
                return soundSource.GetPosition();
            }
            else return new TimeSpan();
        }
        public void SetCurrentPosition(TimeSpan pos)
        {
            if (isInitialized)
            {
                soundSource.SetPosition(pos);
            }
        }
        public void SetBalance(int balance)
        {
            if (isInitialized)
            {
            }
        }
        public TimeSpan Length()
        {
            if (isInitialized)
            {
                return soundSource.GetLength();
            }
            else return new TimeSpan();
        }
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
