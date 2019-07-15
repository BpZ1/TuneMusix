using System;

namespace TuneMusix.Helpers.MediaPlayer
{
    interface AudioPlayer
    {
        /// <summary>
        /// Returns true if the MediaPlayer is currently playing a medium.
        /// </summary>
        /// <returns></returns>
        bool IsPlaying();

        /// <summary>
        /// Pauses the currently loaded Track.
        /// </summary>
        void Pause();
        /// <summary>
        /// Resumes playback after the medium has been paused.
        /// </summary>
        void Resume();

        /// <summary>
        /// Plays the currently loaded Track.
        /// </summary>
        void Play();

        /// <summary>
        /// Stops playback the currently loaded Track.
        /// </summary>
        void Stop();

        /// <summary>
        /// Returns the current position of the playing Track.
        /// </summary>
        /// <returns></returns>
        TimeSpan CurrentPosition();

        /// <summary>
        /// Returns the length of the current Track.
        /// </summary>
        /// <returns></returns>
        TimeSpan Length();
        /// <summary>
        /// Returns the sample rate of the current Track.
        /// </summary>
        int SampleRate { get; }

        /// <summary>
        /// Sets the volume of the current playback
        /// </summary>
        float Volume { set; }

        bool Repeat { set; }

        

    }
}
