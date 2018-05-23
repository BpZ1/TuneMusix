using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EqualizerEffect : BaseEffect
    {

        private Equalizer equalizer;
        private bool isInitialized;
        private double[] channelFilter;

        public EqualizerEffect()
        {
            channelFilter = new double[10];
            isInitialized = false;
        }

        public EqualizerEffect(double[] channelFilter)
        {
            if(channelFilter.Length == 10)
            {
                this.channelFilter = channelFilter;
            }
            isInitialized = false;

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
            equalizer = Equalizer.Create10BandEqualizer(waveSource);
            isInitialized = true;
            int i = 0;
            foreach(double value in channelFilter)
            {
                equalizer.SampleFilters[i].AverageGainDB = value;
                i++;
            }        
            return equalizer;
        }
        #region getter and setter
        public double ChannelFilter0
        {
            get { return channelFilter[0]; }
            set
            {
                channelFilter[0] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[0].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter1
        {
            get { return channelFilter[1]; }
            set
            {
                channelFilter[1] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[1].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter2
        {
            get { return channelFilter[2]; }
            set
            {
                channelFilter[2] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[2].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter3
        {
            get { return channelFilter[3]; }
            set
            {
                channelFilter[3] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[3].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter4
        {
            get { return channelFilter[4]; }
            set
            {
                channelFilter[4] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[4].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter5
        {
            get { return channelFilter[5]; }
            set
            {
                channelFilter[5] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[5].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter6
        {
            get { return channelFilter[6]; }
            set
            {
                channelFilter[6] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[6].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter7
        {
            get { return channelFilter[7]; }
            set
            {
                channelFilter[7] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[7].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter8
        {
            get { return channelFilter[8]; }
            set
            {
                channelFilter[8] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[8].AverageGainDB = value;
                }
            }
        }
        public double ChannelFilter9
        {
            get { return channelFilter[9]; }
            set
            {
                channelFilter[9] = value;
                IsModified = true;
                if (isInitialized)
                {
                    equalizer.SampleFilters[9].AverageGainDB = value;
                }
            }
        }
#endregion
    }
}
