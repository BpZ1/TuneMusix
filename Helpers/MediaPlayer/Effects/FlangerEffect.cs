using CSCore;
using CSCore.Streams.Effects;

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
        
        public FlangerEffect()
        {
            _isInitialized = false;

        }

        public FlangerEffect(float delay,float depth,float feedback,float frequency,float wetDryMix,int waveForm)
        {
            _delay = delay;
            _depth = depth;
            _feedback = feedback;
            _frequency = frequency;
            _wet_DryMix = wetDryMix;
            _waveForm = waveForm;
            _isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(_createFlanger);
            }
            else
            {
                return waveSource;
            }
          
        }

        private DmoFlangerEffect _createFlanger(IWaveSource waveSource)
        {
            _flanger = new DmoFlangerEffect(waveSource);
            _isInitialized = true;
            _flanger.Delay = _delay;
            _flanger.Depth = _depth;
            _flanger.Feedback = _feedback;
            _flanger.Frequency = _frequency;
            _flanger.WetDryMix = _wet_DryMix;
            _flanger.Waveform = (FlangerWaveform)_waveForm;
            return _flanger;
        }

        public float Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                IsModified = true;
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
                {
                    _flanger.WetDryMix = _wet_DryMix;
                }
            }
        }

    }
}
