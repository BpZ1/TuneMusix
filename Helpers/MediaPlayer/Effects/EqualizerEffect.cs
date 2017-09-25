using CSCore;
using CSCore.Streams.Effects;


namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    class EqualizerEffect
    {

        private Equalizer _equalizer;
        private int[] _channelFilter;
        
        public EqualizerEffect() { }

        public EqualizerEffect(int[] channelFilter)
        {
            _channelFilter = channelFilter;
        }

        public IWaveSource Apply(IWaveSource waveSource)
        {
           return waveSource.ToSampleSource()
                .AppendSource(Equalizer.Create10BandEqualizer, out _equalizer)
                .ToWaveSource();
        }

        public void ChannelFilter(int index,double value)
        {
            if (_equalizer != null)
            {
                EqualizerFilter filter = _equalizer.SampleFilters[index];
                filter.AverageGainDB = value;
            }
        }

    }
}
