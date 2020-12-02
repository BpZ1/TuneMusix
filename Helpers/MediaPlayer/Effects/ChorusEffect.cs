using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class ChorusEffect : BaseEffect
    {
        private DmoChorusEffect _chorus;
        private bool _isInitialized;
        private float _delay = 16;
        private float _depth = 10;
        private float _feedback = 25;
        private float _frequency = 1.1f;
        private int _waveForm = 0;
        private int _phase = 3;
        private float _wet_DryMix = 50;

        public ChorusEffect() : base( EffectType.Chorus )
        {
            IsActive = true;
            _isInitialized = false;
        }
        public ChorusEffect( float delay, float depth, float feedback, float frequency, int waveForm, int phase, float wet_DryMix ) : base( EffectType.Chorus )
        {
            IsActive = true;
            _delay = delay;
            _depth = depth;
            _feedback = feedback;
            _frequency = frequency;
            _waveForm = waveForm;
            _phase = phase;
            _wet_DryMix = wet_DryMix;
            _isInitialized = false;
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
                return waveSource.AppendSource( createChorus );

            return waveSource;
        }

        private DmoChorusEffect createChorus( IWaveSource waveSource )
        {
            _chorus = new DmoChorusEffect( waveSource );
            _isInitialized = true;
            _chorus.Delay = _delay;
            _chorus.Depth = _depth;
            _chorus.Feedback = _feedback;
            _chorus.Frequency = _frequency;
            _chorus.Phase = ( ChorusPhase ) _phase;
            _chorus.WetDryMix = _wet_DryMix;
            return _chorus;
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
                WaveForm.ToString(),
                Phase.ToString()
            };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() <= 5 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }

            Delay = GetFloatValue( values.ElementAt( 0 ) );
            Depth = GetFloatValue( values.ElementAt( 1 ) );
            Feedback = GetFloatValue( values.ElementAt( 2 ) );
            Frequency = GetFloatValue( values.ElementAt( 3 ) );
            Wet_DryMix = GetFloatValue( values.ElementAt( 4 ) );
            WaveForm = GetIntValue( values.ElementAt( 5 ) );
            Phase = GetIntValue( values.ElementAt( 6 ) );
        }

        //getter and setter

        public float Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _chorus.Delay = _delay;
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
                    _chorus.Depth = _depth;
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
                    _chorus.Feedback = _feedback;
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
                    _chorus.Frequency = _frequency;
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
                    if ( value == 1 )
                    {
                        _chorus.Waveform = ChorusWaveform.WaveformSin;
                    }
                    else
                    {
                        _chorus.Waveform = ChorusWaveform.WaveformTriangle;
                    }
                }
            }
        }
        public int Phase
        {
            get { return _phase; }
            set
            {
                _phase = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _chorus.Phase = ( ChorusPhase ) _phase;
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
                    _chorus.WetDryMix = _wet_DryMix;
                }
            }
        }
    }
}
