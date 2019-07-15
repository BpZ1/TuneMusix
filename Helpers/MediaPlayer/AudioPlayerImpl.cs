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

        private Complex[] _fftValues = new Complex[2048];
        private SingleBlockNotificationStream _notificationStream;
        private FftProvider _fftProvider;
        private IWaveSource _soundSource;
        private ISoundOut _soundOut;
        private bool _isInitialized;


        public AudioPlayerImpl(string url, float volume, int balance, bool isStereo, EffectQueue effects, bool effectsActive)
        {
            _soundSource = GetSoundSource(url);
            //Apply effects if they are activated
            if (effectsActive)
                _soundSource = effects.Apply(_soundSource);

            this._notificationStream = new SingleBlockNotificationStream(_soundSource.ToSampleSource());
            _fftProvider = new FftProvider(_soundSource.WaveFormat.Channels, FftSize.Fft2048);
            _soundOut = GetSoundOut();

            _notificationStream.SingleBlockRead += AddAudioSamples;
            

            if(_soundSource != null)
            {
                _soundOut.Initialize(_notificationStream.ToWaveSource());
                _soundOut.Volume = volume;
                _isInitialized = true;
                _soundOut.Stopped += PlaybackStopped;
                if (!isStereo)
                    _soundSource.ToMono();
            }                       
        }

        private void AddAudioSamples(object sender, SingleBlockReadEventArgs e)
        {
            try
            {
                this._fftProvider.Add(e.Left, e.Right);
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
            if (!_fftProvider.IsNewDataAvailable) return false;

            bool res = _fftProvider.GetFftData(_fftValues);
            //If the size of the returned array is smaller or equals the data will only be returned for that length
            if (resultBuffer.Length <= _fftValues.Length)
            {             
                for (int i = 0; i < resultBuffer.Length; i++)
                {
                    resultBuffer[i] = (float)Math.Sqrt(Math.Pow(_fftValues[i].Imaginary, 2) + Math.Pow(_fftValues[i].Real, 2));
                }
            }
            else
            {
                for (int i = 0; i < _fftValues.Length; i++)
                {
                    resultBuffer[i] = (float)Math.Sqrt(Math.Pow(_fftValues[i].Imaginary, 2) + Math.Pow(_fftValues[i].Real, 2));
                }
            }    
            return res;
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

        private IWaveSource GetSoundSource(string url)
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
            if(_soundSource == null || _soundOut == null)
            {
                OnPlaybackFinished();
            }
            if (_soundOut.WaveSource != null)
            {
                try
                {
                    if ((TimeSpan.Compare(_soundSource.GetPosition(), _soundSource.GetLength())) >= 0)
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
            if (_isInitialized)
            {
                _soundOut.Pause();
            }
        }
        /// <summary>
        /// Resumes the playback of the current track.
        /// </summary>
        public void Resume()
        {
            if (_isInitialized)
            {
                _soundOut.Resume();
            }
        }
        /// <summary>
        /// Starts the playback if the player is initialized with a track.
        /// </summary>
        public void Play()
        {
            if (_isInitialized)
            {
                _soundOut.Play();             
            }
        }
        /// <summary>
        /// Stops the current playback.
        /// </summary>
        public void Stop()
        {
            if (_isInitialized)
            {
                _soundOut.Stop();
            }
        }
        /// <summary>
        /// Volume of the current playback
        /// </summary>
        public float Volume
        {
            set
            {
                if (_isInitialized)
                    _soundOut.Volume = value;
            }
        }
        /// <summary>
        /// Returns true if the player is currently playing a track.
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            if(_soundOut.PlaybackState.Equals(PlaybackState.Playing))
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
            if (_isInitialized && _soundSource != null)
            {
                return _soundSource.GetPosition();
            }
            else return new TimeSpan();
        }
        /// <summary>
        /// Sets the current position in time for the playback of the current track.
        /// </summary>
        /// <param name="pos"></param>
        public void SetCurrentPosition(TimeSpan pos)
        {
            if (_isInitialized)
            {
                _soundSource.SetPosition(pos);
            }
        }
        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="balance"></param>
        public void SetBalance(int balance)
        {
            if (_isInitialized)
            {
            }
        }
        /// <summary>
        /// The total length of the currently playing track.
        /// </summary>
        /// <returns></returns>
        public TimeSpan Length()
        {
            if (_isInitialized)
            {
                return _soundSource.GetLength();
            }
            else return new TimeSpan();
        }
        /// <summary>
        /// Disposes the current audio playback.
        /// </summary>
        public void Dispose()
        {
            if (_isInitialized)
            {
                _soundOut.Dispose();
                _soundSource.Dispose();
                _isInitialized = false;
            }
        }

        public int SampleRate
        {
            get
            {
                int sampleRate = 0;
                if(_soundSource != null)
                {
                    return _soundSource.WaveFormat.SampleRate;

                }
                return sampleRate;
            }
        }
    }
}
