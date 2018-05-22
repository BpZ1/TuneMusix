using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class GargleEffect : BaseEffect
    {
        private DmoGargleEffect gargle;
        private bool isInitialized;
        private int rate = 20;
        private int waveShape = 0;


        public GargleEffect()
        {
            IsActive = true;
            isInitialized = false;
        }
        public GargleEffect(int rate, int waveShape)
        {
            IsActive = true;
            this.rate = rate;
            this.waveShape = waveShape;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if (IsActive)
            {
                return waveSource.AppendSource(createGargle);
            }
            else
            {
                return waveSource;
            }
           
        }

        private DmoGargleEffect createGargle(IWaveSource waveSource)
        {
            gargle = new DmoGargleEffect(waveSource);
            isInitialized = true;
            gargle.RateHz = rate;
            gargle.WaveShape = (GargleWaveShape)waveShape;
            return gargle;
        }

        public int Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                IsModified = true;
                if (isInitialized)
                {
                    gargle.RateHz = rate;
                }
            }
        }
        public int WaveShape
        {
            get { return waveShape; }
            set
            {
                waveShape = value;
                IsModified = true;
                if (isInitialized)
                {
                    gargle.WaveShape = (GargleWaveShape)waveShape;
                }
            }
        }

    }
}
