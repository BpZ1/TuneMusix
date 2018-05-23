using CSCore;
using CSCore.Streams.Effects;

namespace TuneMusix.Helpers.MediaPlayer.Effects
{
    public class EchoEffect : BaseEffect
    {
        private DmoEchoEffect echo;
        private bool isInitialized;
        private float feedback = 50;
        private float leftDelay = 500;
        private bool panDelay = false;
        private float wet_DryMix = 50;
        private float rightDelay = 500;
       

        public EchoEffect()
        {
            IsActive = true;
            isInitialized = false;
        }
        public EchoEffect(float feedback,float leftDelay,bool panDelay,float wet_DryMix,float rightDelay)
        {
            IsActive = true;
            this.feedback = feedback;
            this.leftDelay = leftDelay;
            this.panDelay = panDelay;
            this.wet_DryMix = wet_DryMix;
            this.rightDelay = rightDelay;
            isInitialized = false;
        }

        public override IWaveSource Apply(IWaveSource waveSource)
        {
            if(IsActive)
                return waveSource.AppendSource(createEcho);

            return waveSource;
        }

        private DmoEchoEffect createEcho(IWaveSource waveSource)
        {
            echo = new DmoEchoEffect(waveSource);
            isInitialized = true;
            echo.Feedback = feedback;
            echo.LeftDelay = leftDelay;
            echo.PanDelay = panDelay;
            echo.WetDryMix = wet_DryMix;
            echo.RightDelay = rightDelay;
            return echo;
        }

        #region properties
        public float Feedback
        {
            get { return feedback; }
            set
            {
                feedback = value;
                IsModified = true;
                if (isInitialized)
                {
                    echo.Feedback = feedback;
                }
            }
        }
        public float LeftDelay
        {
            get { return leftDelay; }
            set
            {
                leftDelay = value;
                IsModified = true;
                if (isInitialized)
                {
                    echo.LeftDelay = leftDelay;
                }
            }
        }
        public float RightDelay
        {
            get { return rightDelay; }
            set
            {
                rightDelay = value;
                IsModified = true;
                if (isInitialized)
                {
                    echo.RightDelay = rightDelay;
                }
            }
        }
        public bool PanDelay
        {
            get { return panDelay; }
            set
            {
                panDelay = value;
                IsModified = true;
                if (isInitialized)
                {
                    echo.PanDelay = panDelay;
                }
            }
        }
        public float WetDryMix
        {
            get { return wet_DryMix; }
            set
            {
                wet_DryMix = value;
                IsModified = true;
                if (isInitialized)
                {
                    echo.WetDryMix = wet_DryMix;
                }
            }
        }
        #endregion
    }
}
