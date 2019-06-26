using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EqualizerEffect : BaseEffect
    {

        private Equalizer _equalizer;
        private bool _isInitialized;
        private double[] _channelFilter;

        public EqualizerEffect()
        {
            _channelFilter = new double[10];
            _isInitialized = false;
        }

        public EqualizerEffect(double[] channelFilter)
        {
            if(channelFilter.Length == 10)
            {
                this._channelFilter = channelFilter;
            }
            _isInitialized = false;

        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {     
            if(IsActive)
                return waveSource.ToSampleSource().
                    AppendSource(createEqualizer).
                    ToWaveSource();

            return waveSource;
        }

        private Equalizer createEqualizer(ISampleSource waveSource)
        {
            _equalizer = Equalizer.Create10BandEqualizer(waveSource);
            _isInitialized = true;
            int i = 0;
            foreach(double value in _channelFilter)
            {
                _equalizer.SampleFilters[i].AverageGainDB = value;
                i++;
            }        
            return _equalizer;
        }
        #region getter and setter
        public double ChannelFilter0
        {
            get { return _channelFilter[0]; }
            set
            {
                _channelFilter[0] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[0].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter1
        {
            get { return _channelFilter[1]; }
            set
            {
                _channelFilter[1] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[1].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter2
        {
            get { return _channelFilter[2]; }
            set
            {
                _channelFilter[2] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[2].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter3
        {
            get { return _channelFilter[3]; }
            set
            {
                _channelFilter[3] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[3].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter4
        {
            get { return _channelFilter[4]; }
            set
            {
                _channelFilter[4] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[4].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter5
        {
            get { return _channelFilter[5]; }
            set
            {
                _channelFilter[5] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[5].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter6
        {
            get { return _channelFilter[6]; }
            set
            {
                _channelFilter[6] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[6].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter7
        {
            get { return _channelFilter[7]; }
            set
            {
                _channelFilter[7] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[7].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter8
        {
            get { return _channelFilter[8]; }
            set
            {
                _channelFilter[8] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[8].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter9
        {
            get { return _channelFilter[9]; }
            set
            {
                _channelFilter[9] = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _equalizer.SampleFilters[9].AverageGainDB = value;
                }
            }
        }
#endregion
    }
}
