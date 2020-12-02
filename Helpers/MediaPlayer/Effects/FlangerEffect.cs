using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class FlangerEffect : BaseEffect
    {
        private DmoFlangerEffect _flanger;
        private bool _isInitialized;
        private float _delay = 2;
        private float _depth = 100;
        private float _feedback = -50;
        private float _frequency = 0.25f;
        private float _wet_DryMix = 50;
        private int _waveForm = 1;

        public FlangerEffect() : base( EffectType.Flanger )
        {
            IsActive = true;
            _isInitialized = false;
        }

        public FlangerEffect( float delay, float depth, float feedback, float frequency, float wetDryMix, int waveForm ) : base( EffectType.Flanger )
        {
            IsActive = true;
            this._delay = delay;
            this._depth = depth;
            this._feedback = feedback;
            this._frequency = frequency;
            _wet_DryMix = wetDryMix;
            this._waveForm = waveForm;
            _isInitialized = false;
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
                return waveSource.AppendSource( createFlanger );

            return waveSource;
        }

        private DmoFlangerEffect createFlanger( IWaveSource waveSource )
        {
            _flanger = new DmoFlangerEffect( waveSource );
            _isInitialized = true;
            _flanger.Delay = _delay;
            _flanger.Depth = _depth;
            _flanger.Feedback = _feedback;
            _flanger.Frequency = _frequency;
            _flanger.WetDryMix = _wet_DryMix;
            _flanger.Waveform = ( FlangerWaveform ) _waveForm;
            return _flanger;
        }

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                Delay.ToString(),
                Depth.ToString(),
                Feedback.ToString(),
                Frequency.ToString(),
                Wet_DryMix.ToString(),
                WaveForm.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() < 6 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            Delay = GetFloatValue( values.ElementAt( 0 ) );
            Depth = GetFloatValue( values.ElementAt( 1 ) );
            Feedback = GetFloatValue( values.ElementAt( 2 ) );
            Frequency = GetFloatValue( values.ElementAt( 3 ) );
            Wet_DryMix = GetFloatValue( values.ElementAt( 4 ) );
            WaveForm = GetIntValue( values.ElementAt( 5 ) );
        }

        public float Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.Delay = _delay;
                }
            }
        }
        public float Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.Depth = _depth;
                }
            }
        }
        public float Feedback
        {
            get { return _feedback; }
            set
            {
                _feedback = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.Delay = _feedback;
                }
            }
        }
        public float Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.Frequency = _frequency;
                }
            }
        }
        public float Wet_DryMix
        {
            get { return _wet_DryMix; }
            set
            {
                _wet_DryMix = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.WetDryMix = _wet_DryMix;
                }
            }
        }
        public int WaveForm
        {
            get { return _waveForm; }
            set
            {
                _waveForm = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _flanger.Waveform = ( FlangerWaveform ) _waveForm;
                }
            }
        }

    }
}
