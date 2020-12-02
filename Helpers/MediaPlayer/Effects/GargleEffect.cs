using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class GargleEffect : BaseEffect
    {
        private DmoGargleEffect _gargle;
        private bool _isInitialized;
        private int _rate = 20;
        private int _waveShape = 0;

        public GargleEffect() : base( EffectType.Gargle )
        {
            IsActive = true;
            _isInitialized = false;
        }
        public GargleEffect( int rate, int waveShape ) : base( EffectType.Gargle )
        {
            IsActive = true;
            this._rate = rate;
            this._waveShape = waveShape;
        }

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                Rate.ToString(),
                WaveShape.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() <= 2 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            Rate = GetIntValue( values.ElementAt( 0 ) );
            WaveShape = GetIntValue( values.ElementAt( 1 ) );
        }

        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
            {
                return waveSource.AppendSource( CreateGargle );
            }

            return waveSource;
        }

        private DmoGargleEffect CreateGargle( IWaveSource waveSource )
        {
            _gargle = new DmoGargleEffect( waveSource );
            _isInitialized = true;
            _gargle.RateHz = _rate;
            _gargle.WaveShape = ( GargleWaveShape ) _waveShape;
            return _gargle;
        }

        public int Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _gargle.RateHz = _rate;
                }
            }
        }
        public int WaveShape
        {
            get { return _waveShape; }
            set
            {
                _waveShape = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _gargle.WaveShape = ( GargleWaveShape ) _waveShape;
                }
            }
        }

    }
}
