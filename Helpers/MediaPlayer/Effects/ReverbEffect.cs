using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class ReverbEffect : BaseEffect
    {
        private DmoWavesReverbEffect _reverb;
        private bool isInitialized;
        private float highFrequencyRTRatio = 0.001f;
        private float inGain = 0;
        private float reverbMix = 0;
        private float reverbTime = 1000;


        public ReverbEffect()
        {
            IsActive = true;
            isInitialized = false;
        }
        public ReverbEffect(
            float highFrequencyRTRatio,
            float inGain,
            float reverbMix,
            float reverbTime)
        {
            IsActive = true;
            this.highFrequencyRTRatio = highFrequencyRTRatio;
            this.inGain = inGain;
            this.reverbMix = reverbMix;
            this.reverbTime = reverbTime;
            isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
                return waveSource.AppendSource(createFlanger);
            
            return waveSource;
        }

        private DmoWavesReverbEffect createFlanger(IWaveSource waveSource)
        {
            _reverb = new DmoWavesReverbEffect(waveSource);
            isInitialized = true;
            _reverb.HighFrequencyRTRatio = highFrequencyRTRatio;
            _reverb.InGain = inGain;
            _reverb.ReverbMix = reverbMix;
            _reverb.ReverbTime = reverbTime;
            return _reverb;
        }

        public float HighFrequencyRTRatio
        {
            get { return highFrequencyRTRatio; }
            set
            {
                highFrequencyRTRatio = value;
                IsModified = true;
                if (isInitialized)
                {
                    _reverb.HighFrequencyRTRatio = highFrequencyRTRatio;
                }
            }
        }
        public float InGain
        {
            get { return inGain; }
            set
            {
                inGain = value;
                IsModified = true;
                if (isInitialized)
                {
                    _reverb.InGain = inGain;
                }
            }
        }
        public float ReverbMix
        {
            get { return reverbMix; }
            set
            {
                reverbMix = value;
                IsModified = true;
                if (isInitialized)
                {
                    _reverb.ReverbMix = reverbMix;
                }
            }
        }
        public float ReverbTime
        {
            get { return reverbTime; }
            set
            {
                reverbTime = value;
                IsModified = true;
                if (isInitialized)
                {
                    _reverb.ReverbTime = reverbTime;
                }
            }
        }




    }
}
