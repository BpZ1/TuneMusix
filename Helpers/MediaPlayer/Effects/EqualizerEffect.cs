using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EqualizerEffect
    {

        private Equalizer _equalizer;
        private double[] _channelFilter;
        private Options options = Options.Instance;

        public EqualizerEffect()
        {
            options.ChannelFilterChanged += _onChannelFilterChanged;
        }

        public EqualizerEffect(double[] channelFilter)
        {
            _channelFilter = channelFilter;
            options.ChannelFilterChanged += _onChannelFilterChanged;
        }

        public IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.ToSampleSource().
                AppendSource(_createEqualizer).
                ToWaveSource();
        }

        private Equalizer _createEqualizer(ISampleSource waveSource)
        {
            _equalizer = Equalizer.Create10BandEqualizer(waveSource);
            int i = 0;
            foreach(double filter in _channelFilter)
            {
                _equalizer.SampleFilters[i].AverageGainDB = _channelFilter[i];
                i++;
            }
            return _equalizer;
        }

        public void ChannelFilter(int index,double value)
        {
            if (_equalizer != null)
            {
                EqualizerFilter filter = _equalizer.SampleFilters[index];
                filter.AverageGainDB = value;
            }
        }

        private void _onChannelFilterChanged(object source)
        {
            _channelFilter = source as double[];
        }
    }
}
