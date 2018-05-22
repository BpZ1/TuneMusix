using System;
using CSCore;
using CSCore.Streams.Effects;
using TuneMusix.Model;


namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class ChorusEffect : BaseEffect
    {
        private DmoChorusEffect chorus;
        private bool isInitialized;
        private float delay = 16;
        private float depth = 10;
        private float feedback = 25;
        private float frequency = 1.1f;
        private int waveForm = 0;
        private int phase = 3;
        private float wet_DryMix = 50;

        public ChorusEffect()
        {
            IsActive = true;
            isInitialized = false;
        }
        public ChorusEffect(float delay,float depth,float feedback,float frequency,int phase,float wet_DryMix)
        {
            IsActive = true;
            this.delay = delay;
            this.depth = depth;
            this.feedback = feedback;
            this.frequency = frequency;
            this.phase = phase;
            this.wet_DryMix = wet_DryMix;
            isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(createChorus);
            }
            else
            {
                return waveSource;
            }
         
        }

        private DmoChorusEffect createChorus(IWaveSource waveSource)
        {
            chorus = new DmoChorusEffect(waveSource);
            isInitialized = true;
            chorus.Delay = delay;
            chorus.Depth = depth;
            chorus.Feedback = feedback;
            chorus.Frequency = frequency;
            chorus.Phase = (ChorusPhase)phase;
            chorus.WetDryMix = wet_DryMix;
            return chorus;
        }

        //getter and setter

        public float Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                IsModified = true;
                if (isInitialized)
                {
                    chorus.Delay = delay;
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
                    chorus.Depth = depth;
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
                    chorus.Feedback = feedback;
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
                    chorus.Frequency = frequency;
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
                    if(value == 1)
                    {                 
                        chorus.Waveform = ChorusWaveform.WaveformSin;
                    }
                    else
                    {
                        chorus.Waveform = ChorusWaveform.WaveformTriangle;
                    }                 
                }
            }
        }
        public int Phase
        {
            get { return phase; }
            set
            {
                phase = value;
                IsModified = true;
                if (isInitialized)
                {
                    chorus.Phase = (ChorusPhase)phase;
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
                    chorus.WetDryMix = wet_DryMix;
                }
            }
        }
    }
}
