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

        public void AddEffect(BaseEffect effect)
        {
            Func<IWaveSource, IWaveSource> func = effect.Apply;
            Queue.AddLast(func);
            _modified = true;
            OnQueueChanged();
        }
       
        /// <summary>
        /// Adds an 10 band equalizer at the end of the queue.
        /// </summary>
        public void Add10BandEqualizer()
        {
            Func<IWaveSource, IWaveSource> func = _create10BandEqualizer;
            Queue.AddLast(func);
            _modified = true;
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
                _modified = true;
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

        public bool IsModified
        {
            get { return _modified; }
            set
            {
                _modified = value;
            }
        }

    }
}
