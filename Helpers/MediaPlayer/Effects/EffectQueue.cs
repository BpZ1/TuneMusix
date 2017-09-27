using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{ 
    public class EffectQueue
    {
        private bool _modified;
        public EqualizerEffect EqualizerEff { get; set; }
        public CompressorEffect Compressor { get; set; }
        public FlangerEffect Flanger { get; set; }

        private LinkedList<Func<IWaveSource,IWaveSource>> Queue;

        public EffectQueue()
        {
            Queue = new LinkedList<Func<IWaveSource, IWaveSource>>();
        }
        /// <summary>
        /// Adds an effect of type flanger at the end of the queue.
        /// </summary>
        /// <param name="flanger"></param>
        public void AddFlanger(FlangerEffect flanger)
        {
            Func<IWaveSource, IWaveSource> func = flanger.Apply;
            Queue.AddLast(func);
            OnQueueChanged();
        }
        /// <summary>
        /// Adds an effect of type equalizer at the end of the queue.
        /// </summary>
        /// <param name="equalizer"></param>
        public void AddEqualizer(EqualizerEffect equalizer)
        {
            Func<IWaveSource, IWaveSource> func = equalizer.Apply;
            Queue.AddLast(func);
            OnQueueChanged();
        }
        /// <summary>
        /// Adds an 10 band equalizer at the end of the queue.
        /// </summary>
        public void Add10BandEqualizer()
        {
            Func<IWaveSource, IWaveSource> func = _create10BandEqualizer;
            Queue.AddLast(func);
            OnQueueChanged();
        }
        /// <summary>
        /// Adds an effect of type reverb at the end of the queue.
        /// </summary>
        /// <param name="reverb"></param>
        public void AddReverb(ReverbEffect reverb)
        {
            Func<IWaveSource, IWaveSource> func = reverb.Apply;
            Queue.AddLast(func);
            OnQueueChanged();
        }

        /// <summary>
        /// Adds an effect of type chorus at the end of the queue.
        /// </summary>
        /// <param name="chorus"></param>
        public void AddChorus(ChorusEffect chorus)
        {
            Func<IWaveSource, IWaveSource> func = chorus.Apply;
            Queue.AddLast(func);
            OnQueueChanged();
        }

        private IWaveSource _create10BandEqualizer(IWaveSource waveSource)
        {
            return waveSource
                .ToSampleSource()
                .AppendSource(Equalizer.Create10BandEqualizer)
                .ToWaveSource();
        }

        /// <summary>
        /// Removes the last element of the Effectqueue if the queue has at least one element.
        /// </summary>
        public void RemoveLast()
        {
            if(Queue.Count > 0)
            {
                Queue.RemoveLast();
                OnQueueChanged();
            }           
        }

        public int Count
        {
            get { return Queue.Count; }
        }


        /// <summary>
        /// Applies all effects that are contained in the queue to the given IWavesource and returns it.
        /// </summary>
        /// <param name="waveSource"></param>
        /// <returns></returns>
        public IWaveSource Apply(IWaveSource waveSource)
        {
            foreach (Func<IWaveSource,IWaveSource> effect in Queue)
            {
                waveSource = effect(waveSource);
            }
            return waveSource;
        }

        public delegate void EffectQueueEventHandler(object changed);
        public event EffectQueueEventHandler QueueChanged;

        protected virtual void OnQueueChanged()
        {
            if (QueueChanged != null)
            {
                QueueChanged(Queue);
            }
        }

    }
}
