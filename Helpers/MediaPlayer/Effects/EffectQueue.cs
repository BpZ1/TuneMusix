using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{ 
    public class EffectQueue
    {

        public EqualizerEffect EqualizerEff { get; set; }
        public CompressorEffect Compressor { get; set; }
        public FlangerEffect Flanger { get; set; }

        public List<Func<IWaveSource,IWaveSource>> Queue;

        public EffectQueue()
        {
            Queue = new List<Func<IWaveSource, IWaveSource>>();
        }

        public void AddFlanger(FlangerEffect flanger)
        {
            Func<IWaveSource, IWaveSource> func = flanger.Apply;
            Queue.Add(func);
        }
        public void AddEqualizer(EqualizerEffect equalizer)
        {
            Func<IWaveSource, IWaveSource> func = equalizer.Apply;
            Queue.Add(func);
        }
        public void Add10BandEqualizer()
        {
            Func<IWaveSource, IWaveSource> func = _create10BandEqualizer;
        }
        public void AddChorus(ChorusEffect chorus)
        {
            Func<IWaveSource, IWaveSource> func = chorus.Apply;
            Queue.Add(func);
        }

        private IWaveSource _create10BandEqualizer(IWaveSource waveSource)
        {
            return waveSource
                .ToSampleSource()
                .AppendSource(Equalizer.Create10BandEqualizer)
                .ToWaveSource();
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




    }
}
