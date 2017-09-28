using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EqualizerEffect : BaseEffect
    {

        private Equalizer _equalizer;
        private bool _isInitialized;
        private double[] _channelFilter;
        private Options options = Options.Instance;

        public EqualizerEffect()
        {
            _isInitialized = false;
        }

        public EqualizerEffect(double[] channelFilter)
        {
            _channelFilter = channelFilter;
            _isInitialized = false;

        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.ToSampleSource().
                AppendSource(_createEqualizer).
                ToWaveSource();
        }

        private Equalizer _createEqualizer(ISampleSource waveSource)
        {
            _equalizer = Equalizer.Create10BandEqualizer(waveSource);
            _isInitialized = true;
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
            if (_isInitialized)
            {
                _equalizer.SampleFilters[index].AverageGainDB = value;
                IsModified = true;
            }
        }

    }
}
