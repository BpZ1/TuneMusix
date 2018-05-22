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
    public class CompressorEffect : BaseEffect
    {
        private DmoCompressorEffect compressor;
        private bool isInitialized;
        private float attack = 10;
        private float gain = 0;
        private float preDelay = 4;
        private float ratio = 3;
        private float release = 200;
        private float treshold = -20;

        public CompressorEffect()
        {
            IsActive = true;
            isInitialized = false;
        }
        public CompressorEffect(
            float attack,
            float gain,float preDelay,
            float ratio,
            float release,
            float trashold)
        {
            IsActive = true;
            this.attack = attack;
            this.gain = gain;
            this.preDelay = preDelay;
            this.ratio = ratio;
            this.release = release;
            treshold = trashold;
            isInitialized = false;
        }


        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(createCompressor);
            }
            else
            {
                return waveSource;
            }
            
        }
        private DmoCompressorEffect createCompressor(IWaveSource waveSource)
        {
            compressor = new DmoCompressorEffect(waveSource);
            isInitialized = true;
            compressor.Attack = attack;
            compressor.Gain = gain;
            compressor.Predelay = preDelay;
            compressor.Ratio = ratio;
            compressor.Release = release;
            compressor.Threshold = treshold;
            return compressor;
        }

        public float Attack
        {
            get { return attack; }
            set
            {
                attack = value;
                IsModified = true;
                if (isInitialized)
                {
                    compressor.Attack = attack;
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
                    compressor.Gain = gain;
                }
            }
        }
        public float Predelay
        {
            get { return preDelay; }
            set
            {
                preDelay = value;
                IsModified = true;
                if (isInitialized)
                {
                    compressor.Predelay = preDelay;
                }
            }
        }
        public float Ratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                IsModified = true;
                if (isInitialized)
                {
                    compressor.Ratio = ratio;
                }
            }
        }
        public float Release
        {
            get { return release; }
            set
            {
                release = value;
                IsModified = true;
                if (isInitialized)
                {
                    compressor.Release = release;
                }
            }
        }
        public float Treshold
        {
            get { return treshold; }
            set
            {
                treshold = value;
                IsModified = true;
                if (isInitialized)
                {
                    compressor.Threshold = treshold;
                }
            }
        }



    }
}
