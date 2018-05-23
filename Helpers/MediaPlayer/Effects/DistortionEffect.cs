using System;
using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class DistortionEffect : BaseEffect
    {
        private DmoDistortionEffect distortion;
        private bool isInitialized;
        private float edge = 15;
        private float gain = -18;
        private float postEQBandwidth = 2400;
        private float postEQCenterFrequency = 2400;
        private float preLowPassCutoff = 8000;

        public DistortionEffect()
        {
            isInitialized = false;
        }
        public DistortionEffect(float edge,float gain,float postEQBandwidth,float postEQCenterFrequency,float preLowPass)
        {
            this.edge = edge;
            this.gain = gain;
            this.postEQBandwidth = postEQBandwidth;
            this.postEQCenterFrequency = postEQCenterFrequency;
            preLowPassCutoff = preLowPass;
            isInitialized = false;
        }


        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
                return waveSource.AppendSource(createDistortion);

            return waveSource;
        }

        private DmoDistortionEffect createDistortion(IWaveSource waveSource)
        {
            distortion = new DmoDistortionEffect(waveSource);
            isInitialized = true;
            distortion.Edge = edge;
            distortion.Gain = gain;
            distortion.PostEQBandwidth = postEQBandwidth;
            distortion.PostEQCenterFrequency = postEQCenterFrequency;
            distortion.PreLowpassCutoff = preLowPassCutoff;
            return distortion;
        }

        public float Edge
        {
            get { return edge; }
            set
            {
                edge = value;
                IsModified = true;
                if (isInitialized)
                {
                    distortion.Edge = edge;
                }
            }
        }
        public float Gain
        {
            get { return gain; }
            set
            {
                gain = value;
                IsModified = true;
                if (isInitialized)
                {
                    distortion.Gain = gain;
                }
            }
        }
        public float PostEQBandwidth
        {
            get { return postEQBandwidth; }
            set
            {
                postEQBandwidth = value;
                IsModified = true;
                if (isInitialized)
                {
                    distortion.PostEQBandwidth = postEQBandwidth;
                }
            }
        }
        public float PostEQCenterFrequency
        {
            get { return postEQCenterFrequency; }
            set
            {
                postEQCenterFrequency = value;
                IsModified = true;
                if (isInitialized)
                {
                    distortion.PostEQCenterFrequency = postEQCenterFrequency;
                }
            }
        }
        public float PreLowPassCutoff
        {
            get { return preLowPassCutoff; }
            set
            {
                preLowPassCutoff = value;
                IsModified = true;
                if (isInitialized)
                {
                    distortion.PreLowpassCutoff = preLowPassCutoff;
                }
            }
        }
    }
}
