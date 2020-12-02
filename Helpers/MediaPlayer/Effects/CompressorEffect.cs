using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class CompressorEffect : BaseEffect
    {
        private DmoCompressorEffect _compressor;
        private bool _isInitialized;
        private float _attack = 10;
        private float _gain = 0;
        private float _preDelay = 4;
        private float _ratio = 3;
        private float _release = 200;
        private float _treshold = -20;

        public CompressorEffect() : base( EffectType.Compressor )
        {
            IsActive = true;
            _isInitialized = false;
        }
        public CompressorEffect(
            float attack,
            float gain, float preDelay,
            float ratio,
            float release,
            float trashold ) : base( EffectType.Compressor )
        {
            IsActive = true;
            this._attack = attack;
            this._gain = gain;
            this._preDelay = preDelay;
            this._ratio = ratio;
            this._release = release;
            _treshold = trashold;
            _isInitialized = false;
        }


        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
            {
                return waveSource.AppendSource( createCompressor );
            }

            return waveSource;
        }
        private DmoCompressorEffect createCompressor( IWaveSource waveSource )
        {
            _compressor = new DmoCompressorEffect( waveSource );
            _isInitialized = true;
            _compressor.Attack = _attack;
            _compressor.Gain = _gain;
            _compressor.Predelay = _preDelay;
            _compressor.Ratio = _ratio;
            _compressor.Release = _release;
            _compressor.Threshold = _treshold;
            return _compressor;
        }

        public float Attack
        {
            get { return _attack; }
            set
            {
                _attack = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Attack = _attack;
                }
            }
        }
        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Gain = _gain;
                }
            }
        }
        public float Predelay
        {
            get { return _preDelay; }
            set
            {
                _preDelay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Predelay = _preDelay;
                }
            }
        }
        public float Ratio
        {
            get { return _ratio; }
            set
            {
                _ratio = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Ratio = _ratio;
                }
            }
        }
        public float Release
        {
            get { return _release; }
            set
            {
                _release = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Release = _release;
                }
            }
        }
        public float Threshold
        {
            get { return _treshold; }
            set
            {
                _treshold = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _compressor.Threshold = _treshold;
                }
            }
        }
        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                Attack.ToString(),
                Gain.ToString(),
                Predelay.ToString(),
                Ratio.ToString(),
                Release.ToString(),
                Threshold.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() <= 6 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            Attack = GetFloatValue( values.ElementAt( 0 ) );
            Gain = GetFloatValue( values.ElementAt( 1 ) );
            Predelay = GetFloatValue( values.ElementAt( 2 ) );
            Ratio = GetFloatValue( values.ElementAt( 3 ) );
            Release = GetFloatValue( values.ElementAt( 4 ) );
            Threshold = GetFloatValue( values.ElementAt( 5 ) );
        }
    }
}
