using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EqualizerEffect : BaseEffect
    {

        private Equalizer _equalizer;
        private bool _isInitialized;
        private double[] _channelFilter;

        public EqualizerEffect() : base( EffectType.Equalizer )
        {
            _channelFilter = new double[10];
            _isInitialized = false;
        }

        public EqualizerEffect( double[] channelFilter ) : base( EffectType.Equalizer )
        {
            if ( channelFilter.Length == 10 )
            {
                this._channelFilter = channelFilter;
            }
            _isInitialized = false;

        }

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                ChannelFilter0.ToString(),
                ChannelFilter1.ToString(),
                ChannelFilter2.ToString(),
                ChannelFilter3.ToString(),
                ChannelFilter4.ToString(),
                ChannelFilter5.ToString(),
                ChannelFilter6.ToString(),
                ChannelFilter7.ToString(),
                ChannelFilter8.ToString(),
                ChannelFilter9.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() < 10 )
            {
                throw new ArgumentException( $"Invalid number of values {values.Count()}" );
            }
            ChannelFilter0 = GetDoubleValue( values.ElementAt( 0 ) );
            ChannelFilter1 = GetDoubleValue( values.ElementAt( 1 ) );
            ChannelFilter2 = GetDoubleValue( values.ElementAt( 2 ) );
            ChannelFilter3 = GetDoubleValue( values.ElementAt( 3 ) );
            ChannelFilter4 = GetDoubleValue( values.ElementAt( 4 ) );
            ChannelFilter5 = GetDoubleValue( values.ElementAt( 5 ) );
            ChannelFilter6 = GetDoubleValue( values.ElementAt( 6 ) );
            ChannelFilter7 = GetDoubleValue( values.ElementAt( 7 ) );
            ChannelFilter8 = GetDoubleValue( values.ElementAt( 8 ) );
            ChannelFilter9 = GetDoubleValue( values.ElementAt( 9 ) );
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
            {
                return waveSource.ToSampleSource().
                  AppendSource( createEqualizer ).
                  ToWaveSource();
            }

            return waveSource;
        }

        private Equalizer createEqualizer( ISampleSource waveSource )
        {
            _equalizer = Equalizer.Create10BandEqualizer( waveSource );
            _isInitialized = true;
            int i = 0;
            foreach ( double value in _channelFilter )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
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
                if ( _isInitialized )
                {
                    _equalizer.SampleFilters[9].AverageGainDB = value;
                }
            }
        }
        #endregion
    }
}
