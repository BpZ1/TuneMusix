using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EchoEffect : BaseEffect
    {
        private DmoEchoEffect _echo;
        private bool _isInitialized;
        private float _feedback = 50;
        private float _leftDelay = 500;
        private bool _panDelay = false;
        private float _wet_DryMix = 50;
        private float _rightDelay = 500;

        public EchoEffect() : base( EffectType.Echo )
        {
            IsActive = true;
            _isInitialized = false;
        }
        public EchoEffect( float feedback, float leftDelay, bool panDelay, float wet_DryMix, float rightDelay ) : base( EffectType.Echo )
        {
            IsActive = true;
            _feedback = feedback;
            _leftDelay = leftDelay;
            _panDelay = panDelay;
            _wet_DryMix = wet_DryMix;
            _rightDelay = rightDelay;
            _isInitialized = false;
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
            {
                return waveSource.AppendSource( createEcho );
            }

            return waveSource;
        }

        private DmoEchoEffect createEcho( IWaveSource waveSource )
        {
            _echo = new DmoEchoEffect( waveSource );
            _isInitialized = true;
            _echo.Feedback = _feedback;
            _echo.LeftDelay = _leftDelay;
            _echo.PanDelay = _panDelay;
            _echo.WetDryMix = _wet_DryMix;
            _echo.RightDelay = _rightDelay;
            return _echo;
        }

        #region properties
        public float Feedback
        {
            get { return _feedback; }
            set
            {
                _feedback = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _echo.Feedback = _feedback;
                }
            }
        }
        public float LeftDelay
        {
            get { return _leftDelay; }
            set
            {
                _leftDelay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _echo.LeftDelay = _leftDelay;
                }
            }
        }
        public float RightDelay
        {
            get { return _rightDelay; }
            set
            {
                _rightDelay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _echo.RightDelay = _rightDelay;
                }
            }
        }
        public bool PanDelay
        {
            get { return _panDelay; }
            set
            {
                _panDelay = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _echo.PanDelay = _panDelay;
                }
            }
        }
        public float WetDryMix
        {
            get { return _wet_DryMix; }
            set
            {
                _wet_DryMix = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _echo.WetDryMix = _wet_DryMix;
                }
            }
        }
        #endregion

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                Feedback.ToString(),
                LeftDelay.ToString(),
                RightDelay.ToString(),
                WetDryMix.ToString(),
                PanDelay.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() <= 4 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            Feedback = GetFloatValue( values.ElementAt( 0 ) );
            LeftDelay = GetFloatValue( values.ElementAt( 1 ) );
            RightDelay = GetFloatValue( values.ElementAt( 2 ) );
            WetDryMix = GetFloatValue( values.ElementAt( 3 ) );
            PanDelay = GetBoolValue( values.ElementAt( 3 ) );
        }
    }
}
