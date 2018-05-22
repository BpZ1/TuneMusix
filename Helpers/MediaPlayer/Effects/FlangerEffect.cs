using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class FlangerEffect : BaseEffect
    {

        private DmoFlangerEffect flanger;
        private bool isInitialized;
        private float delay = 2;
        private float depth = 100;
        private float feedback = -50;
        private float frequency = 0.25f;
        private float wet_DryMix = 50;
        private int waveForm = 1;
        
        public FlangerEffect()
        {
            IsActive = true;
            isInitialized = false;
        }

        public FlangerEffect(float delay,float depth,float feedback,float frequency,float wetDryMix,int waveForm)
        {
            IsActive = true;
            this.delay = delay;
            this.depth = depth;
            this.feedback = feedback;
            this.frequency = frequency;
            wet_DryMix = wetDryMix;
            this.waveForm = waveForm;
            isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(createFlanger);
            }
            else
            {
                return waveSource;
            }
          
        }

        private DmoFlangerEffect createFlanger(IWaveSource waveSource)
        {
            flanger = new DmoFlangerEffect(waveSource);
            isInitialized = true;
            flanger.Delay = delay;
            flanger.Depth = depth;
            flanger.Feedback = feedback;
            flanger.Frequency = frequency;
            flanger.WetDryMix = wet_DryMix;
            flanger.Waveform = (FlangerWaveform)waveForm;
            return flanger;
        }

        public float Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.Delay = delay;
                }
            }
        }
        public float Depth
        {
            get { return depth; }
            set
            {
                depth = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.Depth = depth;
                }
            }
        }
        public float Feedback
        {
            get { return feedback; }
            set
            {
                feedback = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.Delay = feedback;
                }
            }
        }
        public float Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.Frequency = frequency;
                }
            }
        }
        public float Wet_DryMix
        {
            get { return wet_DryMix; }
            set
            {
                wet_DryMix = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.WetDryMix = wet_DryMix;
                }
            }
        }
        public int WaveForm
        {
            get { return waveForm; }
            set
            {
                waveForm = value;
                IsModified = true;
                if (isInitialized)
                {
                    flanger.Waveform = (FlangerWaveform)waveForm;
                }
            }
        }

    }
}
