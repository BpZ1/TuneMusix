using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class ReverbEffect : BaseEffect
    {
        private DmoWavesReverbEffect _reverb;
        private bool _isInitialized;
        private float _highFrequencyRTRatio = 0.001f;
        private float _inGain = 0;
        private float _reverbMix = 0;
        private float _reverbTime = 1000;

        public ReverbEffect() : base( EffectType.Reverb )
        {
            IsActive = true;
            _isInitialized = false;
        }
        public ReverbEffect(
            float highFrequencyRTRatio,
            float inGain,
            float reverbMix,
            float reverbTime ) : base( EffectType.Reverb )
        {
            IsActive = true;
            this._highFrequencyRTRatio = highFrequencyRTRatio;
            this._inGain = inGain;
            this._reverbMix = reverbMix;
            this._reverbTime = reverbTime;
            _isInitialized = false;
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
            {
                return waveSource.AppendSource( CreateFlanger );
            }

            return waveSource;
        }

        private DmoWavesReverbEffect CreateFlanger( IWaveSource waveSource )
        {
            _reverb = new DmoWavesReverbEffect( waveSource );
            _isInitialized = true;
            _reverb.HighFrequencyRTRatio = _highFrequencyRTRatio;
            _reverb.InGain = _inGain;
            _reverb.ReverbMix = _reverbMix;
            _reverb.ReverbTime = _reverbTime;
            return _reverb;
        }

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                HighFrequencyRTRatio.ToString(),
                InGain.ToString(),
                ReverbMix.ToString(),
                ReverbTime.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() < 4 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            HighFrequencyRTRatio = GetFloatValue( values.ElementAt( 0 ) );
            InGain = GetFloatValue( values.ElementAt( 1 ) );
            ReverbMix = GetFloatValue( values.ElementAt( 2 ) );
            ReverbTime = GetFloatValue( values.ElementAt( 3 ) );
        }

        public float HighFrequencyRTRatio
        {
            get { return _highFrequencyRTRatio; }
            set
            {
                _highFrequencyRTRatio = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _reverb.HighFrequencyRTRatio = _highFrequencyRTRatio;
                }
            }
        }
        public float InGain
        {
            get { return _inGain; }
            set
            {
                _inGain = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _reverb.InGain = _inGain;
                }
            }
        }
        public float ReverbMix
        {
            get { return _reverbMix; }
            set
            {
                _reverbMix = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _reverb.ReverbMix = _reverbMix;
                }
            }
        }
        public float ReverbTime
        {
            get { return _reverbTime; }
            set
            {
                _reverbTime = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _reverb.ReverbTime = _reverbTime;
                }
            }
        }
    }
}
