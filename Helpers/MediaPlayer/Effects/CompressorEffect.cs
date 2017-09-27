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
    public class CompressorEffect
    {
        private DmoCompressorEffect _compressor;
        private float _attack = 10;
        private float _gain = 0;
        private float _preDelay = 4;
        private float _ratio = 3;
        private float _release = 200;
        private float _treshhold = -20;
        private Options options = Options.Instance;

        public CompressorEffect()
        {
            options.AttackCompressorChanged += _onAttackChanged;
            options.GainCompressorChanged += _onGainChanged;
            options.PreDelayCompressorChanged += _onPreDelayChanged;
            options.RatioCompressorChanged += _onRatioChanged;
            options.ReleaseCompressorChanged += _onReleaseChanged;
            options.ThreshholdCompressorChanged += _onTreshholdChanged;

        }
        public CompressorEffect(
            float attack,
            float gain,float preDelay,
            float ratio,
            float release,
            float trashhold)
        {
            _attack = attack;
            _gain = gain;
            _preDelay = preDelay;
            _ratio = ratio;
            _release = release;
            _treshhold = trashhold;
            options.AttackCompressorChanged += _onAttackChanged;
            options.GainCompressorChanged += _onGainChanged;
            options.PreDelayCompressorChanged += _onPreDelayChanged;
            options.RatioCompressorChanged += _onRatioChanged;
            options.ReleaseCompressorChanged += _onReleaseChanged;
            options.ThreshholdCompressorChanged += _onTreshholdChanged;
        }


        public IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.AppendSource(_createCompressor);
        }
        private DmoCompressorEffect _createCompressor(IWaveSource waveSource)
        {
            _compressor = new DmoCompressorEffect(waveSource);
            return _compressor;
        }
        private void _onAttackChanged(object source)
        {
            _attack = (float)source;
        }

        private void _onGainChanged(object source)
        {
            _gain = (float)source;
        }
        private void _onPreDelayChanged(object source)
        {
            _preDelay = (float)source;
        }
        private void _onRatioChanged(object source)
        {
            _ratio = (float)source;
        }
        private void _onReleaseChanged(object source)
        {
            _release = (float)source;
        }
        private void _onTreshholdChanged(object source)
        {
            _treshhold = (float)source;
        }
    }
}
