using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class GargleEffect : BaseEffect
    {
        private DmoGargleEffect _gargle;
        private bool _isInitialized;
        private int _rate = 20;
        private int _waveShape = 0;


        public GargleEffect()
        {
            IsActive = true;
            _isInitialized = false;
        }
        public GargleEffect(int rate, int waveShape)
        {
            IsActive = true;
            _rate = rate;
            _waveShape = waveShape;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(_createGargle);
            }
            else
            {
                return waveSource;
            }
           
        }

        private DmoGargleEffect _createGargle(IWaveSource waveSource)
        {
            _gargle = new DmoGargleEffect(waveSource);
            _isInitialized = true;
            _gargle.RateHz = _rate;
            _gargle.WaveShape = (GargleWaveShape)_rate;
            return _gargle;
        }

        public int Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                IsModified = true;
                if (_isInitialized)
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
                if (_isInitialized)
                {
                    _gargle.WaveShape = (GargleWaveShape)_waveShape;
                }
            }
        }

    }
}
