using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;


namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class ChorusEffect
    {
        private DmoChorusEffect _chorus;
        private float _delay = 16;
        private float _depth = 10;
        private float _feedback = 25;
        private float _frequency = 1.1f;
        private int _waveForm = 0;
        private float _phase = 3;
        private float _wet_DryMix = 50;
        private Options options = Options.Instance;

        public ChorusEffect()
        {
            options.DelayChorusChanged += _onDelayChanged;
            options.DepthChorusChanged += _onDepthChanged;
            options.FeedbackChorusChanged += _onFeedbackChanged;
            options.FrequencyChorusChanged += _onFrequencyChanged;
            options.PhaseChorusChanged += _onPhaseChanged;
            options.WaveFormChorusChanged += _onWaveFormChanged;
            options.FrequencyChorusChanged += _onFrequencyChanged;
            options.Wet_DryMixChorusChanged += _onWet_DryMixChanged;
        }
        public ChorusEffect(float delay,float depth,float feedback,float frequency,float phase,float wet_DryMix)
        {
            _delay = delay;
            _depth = depth;
            _feedback = feedback;
            _frequency = frequency;
            _phase = phase;
            _wet_DryMix = wet_DryMix;
        }

        public IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.AppendSource(_createChorus);
        }

        private DmoChorusEffect _createChorus(IWaveSource waveSource)
        {
            _chorus = new DmoChorusEffect(waveSource);
            return _chorus;
        }

        private void _onDelayChanged(object source)
        {
            _delay = (float)source;
        }
        private void _onDepthChanged(object source)
        {
            _depth = (float)source;
        }
        private void _onFeedbackChanged(object source)
        {
            _feedback = (float)source;
        }
        private void _onFrequencyChanged(object source)
        {
            _frequency = (float)source;
        }
        private void _onWaveFormChanged(object source)
        {
            _waveForm = (int)source;
        }
        private void _onPhaseChanged(object source)
        {
            _phase = (float)source;
        }
        private void _onWet_DryMixChanged(object source)
        {
            _wet_DryMix = (float)source;
        }
    }
}
