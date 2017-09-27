using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Streams.Effects;
using TuneMusix.Model;

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

        public CompressorEffect()
        {
            _isInitialized = false;
        }
        public CompressorEffect(
            float attack,
            float gain,float preDelay,
            float ratio,
            float release,
            float trashold)
        {
            _attack = attack;
            _gain = gain;
            _preDelay = preDelay;
            _ratio = ratio;
            _release = release;
            _treshold = trashold;
            _isInitialized = false;
        }


        public override IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.AppendSource(_createCompressor);
        }
        private DmoCompressorEffect _createCompressor(IWaveSource waveSource)
        {
            _compressor = new DmoCompressorEffect(waveSource);
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
                {
                    _compressor.Release = _release;
                }
            }
        }
        public float Treshhold
        {
            get { return _treshold; }
            set
            {
                _treshold = value;
                IsModified = true;
                if (_isInitialized)
                {
                    _compressor.Threshold = _treshold;
                }
            }
        }



    }
}
