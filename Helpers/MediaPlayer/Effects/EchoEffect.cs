using CSCore;
using CSCore.Streams.Effects;

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
       

        public EchoEffect()
        {
            IsActive = true;
            _isInitialized = false;
        }
        public EchoEffect(float feedback,float leftDelay,bool panDelay,float wet_DryMix,float rightDelay)
        {
            IsActive = true;
            _feedback = feedback;
            _leftDelay = leftDelay;
            _panDelay = panDelay;
            _wet_DryMix = wet_DryMix;
            _rightDelay = rightDelay;
            _isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.AppendSource(_createEcho);
        }

        private DmoEchoEffect _createEcho(IWaveSource waveSource)
        {
            _echo = new DmoEchoEffect(waveSource);
            _isInitialized = true;
            _echo.Feedback = _feedback;
            _echo.LeftDelay = _leftDelay;
            _echo.PanDelay = _panDelay;
            _echo.WetDryMix = _wet_DryMix;
            _echo.RightDelay = _rightDelay;
            return _echo;
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
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
                if (_isInitialized)
                {
                    _echo.WetDryMix = _wet_DryMix;
                }
            }
        }

    }
}
