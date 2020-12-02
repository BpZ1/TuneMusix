using CSCore;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class DistortionEffect : BaseEffect
    {
        private DmoDistortionEffect _distortion;
        private bool _isInitialized;
        private float _edge = 15;
        private float _gain = -18;
        private float _postEQBandwidth = 2400;
        private float _postEQCenterFrequency = 2400;
        private float _preLowPassCutoff = 8000;

        public DistortionEffect() : base( EffectType.Distortion )
        {
            _isInitialized = false;
        }
        public DistortionEffect( float edge, float gain, float postEQBandwidth, float postEQCenterFrequency, float preLowPass ) : base( EffectType.Distortion )
        {
            this._edge = edge;
            this._gain = gain;
            this._postEQBandwidth = postEQBandwidth;
            this._postEQCenterFrequency = postEQCenterFrequency;
            _preLowPassCutoff = preLowPass;
            _isInitialized = false;
        }


        public override IWaveSource Apply( IWaveSource waveSource )
        {
            if ( IsActive )
                return waveSource.AppendSource( createDistortion );

            return waveSource;
        }

        private DmoDistortionEffect createDistortion( IWaveSource waveSource )
        {
            _distortion = new DmoDistortionEffect( waveSource );
            _isInitialized = true;
            _distortion.Edge = _edge;
            _distortion.Gain = _gain;
            _distortion.PostEQBandwidth = _postEQBandwidth;
            _distortion.PostEQCenterFrequency = _postEQCenterFrequency;
            _distortion.PreLowpassCutoff = _preLowPassCutoff;
            return _distortion;
        }

        public float Edge
        {
            get { return _edge; }
            set
            {
                _edge = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _distortion.Edge = _edge;
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
                    _distortion.Gain = _gain;
                }
            }
        }
        public float PostEQBandwidth
        {
            get { return _postEQBandwidth; }
            set
            {
                _postEQBandwidth = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _distortion.PostEQBandwidth = _postEQBandwidth;
                }
            }
        }
        public float PostEQCenterFrequency
        {
            get { return _postEQCenterFrequency; }
            set
            {
                _postEQCenterFrequency = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _distortion.PostEQCenterFrequency = _postEQCenterFrequency;
                }
            }
        }
        public float PreLowPassCutoff
        {
            get { return _preLowPassCutoff; }
            set
            {
                _preLowPassCutoff = value;
                IsModified = true;
                if ( _isInitialized )
                {
                    _distortion.PreLowpassCutoff = _preLowPassCutoff;
                }
            }
        }

        public override IEnumerable<string> GetValues()
        {
            return new List<string>()
            {
                Edge.ToString(),
                Gain.ToString(),
                PostEQBandwidth.ToString(),
                PostEQCenterFrequency.ToString(),
                PreLowPassCutoff.ToString()
             };
        }

        public override void SetValues( IEnumerable<string> values )
        {
            if ( values.Count() <= 5 )
            {
                throw new ArgumentException( "Invalid number of values" );
            }
            Edge = GetFloatValue( values.ElementAt( 0 ) );
            Gain = GetFloatValue( values.ElementAt( 1 ) );
            PostEQBandwidth = GetFloatValue( values.ElementAt( 2 ) );
            PostEQCenterFrequency = GetFloatValue( values.ElementAt( 3 ) );
            PreLowPassCutoff = GetFloatValue( values.ElementAt( 4 ) );
        }
    }
}
