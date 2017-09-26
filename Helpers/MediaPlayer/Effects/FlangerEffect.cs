using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class FlangerEffect
    {

        private DmoFlangerEffect _flanger;
        private float _delay = 2;
        private float _depth = 100;
        private float _feedback = -50;
        private float _frequency = 0.25f;
        private float _wet_DryMix = 50;
        private Options options = Options.Instance;

        public FlangerEffect()
        {
            options.DelayFlangerChanged += _onDelayChanged;
            options.DepthFlangerChanged += _onDepthChanged;
            options.FeedbackFlangerChanged += _onFeedbackChanged;
            options.FrequencyFlangerChanged += _onFrequencyChanged;
            options.Wet_DryMixFlangerChanged += _onWet_DryMixChanged;
        }

        public FlangerEffect(float delay,float depth,float feedback,float frequency,float wetDryMix)
        {
            _delay = delay;
            _depth = depth;
            _feedback = feedback;
            _frequency = frequency;
            _wet_DryMix = wetDryMix;

            options.DelayFlangerChanged += _onDelayChanged;
            options.DepthFlangerChanged += _onDepthChanged;
            options.FeedbackFlangerChanged += _onFeedbackChanged;
            options.FrequencyFlangerChanged += _onFrequencyChanged;
            options.Wet_DryMixFlangerChanged += _onWet_DryMixChanged;
        }

        public IWaveSource Apply(IWaveSource waveSource)
        {
            return waveSource.AppendSource(_createFlanger);
        }

        private DmoFlangerEffect _createFlanger(IWaveSource waveSource)
        {
            _flanger = new DmoFlangerEffect(waveSource);
            return _flanger;
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
        private void _onWet_DryMixChanged(object source)
        {
            _wet_DryMix = (float)source;
        }
    }
}
