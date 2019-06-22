using System;
using System.Diagnostics;
using TuneMusix.Helpers.MediaPlayer;
using CSCore.SoundOut;
using CSCore.Codecs;
using CSCore;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;
using CSCore.DSP;
using CSCore.Utils;
using CSCore.Streams;

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

        private Complex[] fftValues = new Complex[2048];
        private SingleBlockNotificationStream notificationStream;
        private FftProvider fftProvider;
        private IWaveSource soundSource;
        private ISoundOut soundOut;
        private bool isInitialized;


        public AudioPlayerImpl(string url, float volume, int balance, bool isStereo, EffectQueue effects, bool effectsActive)
        {
            soundSource = getSoundSource(url);
            this.notificationStream = new SingleBlockNotificationStream(soundSource.ToSampleSource());
            fftProvider = new FftProvider(soundSource.WaveFormat.Channels, FftSize.Fft2048);
            soundOut = getSoundOut();

            notificationStream.SingleBlockRead += addAudioSamples;
            //Apply effects if they are activated
            if (effectsActive)
               soundSource = effects.Apply(soundSource);

            if(soundSource != null)
            {
                soundOut.Initialize(notificationStream.ToWaveSource());
                soundOut.Volume = volume;
                isInitialized = true;
                soundOut.Stopped += PlaybackStopped;
                if (!isStereo)
                    soundSource.ToMono();
            }                       
        }

        private void addAudioSamples(object sender, SingleBlockReadEventArgs e)
        {
            try
            {
                this.fftProvider.Add(e.Left, e.Right);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Returns the magnitudes of the fft.
        /// </summary>
        /// <param name="resultBuffer">Buffer in which the result will be stored. 
        /// If the array is longer than 2048 only the first 2048 spaces will be filled with data.</param>
        /// <returns>True if the new data was calculated and false if not enough sampels were read.</returns>
        public bool GetFftData(float[] resultBuffer)
        {
            if (!fftProvider.IsNewDataAvailable) return false;

            bool res = fftProvider.GetFftData(fftValues);
            //If the size of the returned array is smaller or equals the data will only be returned for that length
            if (resultBuffer.Length <= fftValues.Length)
            {             
                for (int i = 0; i < resultBuffer.Length; i++)
                {
                    resultBuffer[i] = (float)Math.Sqrt(Math.Pow(fftValues[i].Imaginary, 2) + Math.Pow(fftValues[i].Real, 2));
                }
            }
            else
            {
                for (int i = 0; i < fftValues.Length; i++)
                {
                    resultBuffer[i] = (float)Math.Sqrt(Math.Pow(fftValues[i].Imaginary, 2) + Math.Pow(fftValues[i].Real, 2));
                }
            }    
            return res;
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

        private IWaveSource getSoundSource(string url)
        {
            IWaveSource waveSource;
            try
            {
                waveSource = CodecFactory.Instance.GetCodec(url);      
            }catch
            {
                DialogService.WarnMessage("Could not play track","The track '" + url + "' could not be played.");
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
            if (soundOut.WaveSource != null)
            {
                try
                {
                    Console.WriteLine(soundSource.GetPosition().ToString() + " equals " + soundSource.GetLength().ToString());
                    if ((TimeSpan.Compare(soundSource.GetPosition(), soundSource.GetLength())) >= 0)
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
            if (isInitialized && soundSource != null)
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

        public int SampleRate
        {
            get
            {
                int sampleRate = 0;
                if(soundSource != null)
                {
                    return soundSource.WaveFormat.SampleRate;

                }
                return sampleRate;
            }
        }
    }
}
