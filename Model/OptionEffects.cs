using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Model
{
    public partial class Options
    {

        

      

        //Equalizer//////////////////////////////
        private bool _equalizer;
        private double[] _channelFilter;
        //getter and setter
        public bool Equalizer
        {
            get { return _equalizer; }
            set
            {
                _equalizer = value;
                OnEqualizerChanged();
            }
        }
        public double[] Channelfilter
        {
            get { return _channelFilter; }
            set
            {
                _channelFilter = value;
                OnChannelFilterChanged();
            }
        }

        //events
        public event OptionsEventHandler EqualizerChanged;
        public event OptionsEventHandler ChannelFilterChanged;
        protected virtual void OnEqualizerChanged()
        {
            if (EqualizerChanged != null)
            {
                EqualizerChanged(Equalizer);
            }
        }
        protected virtual void OnChannelFilterChanged()
        {
            if (ChannelFilterChanged != null)
            {
                ChannelFilterChanged(Channelfilter);
            }
        }

        //distortion/////////////////////////////
        private bool _distortion;
        //def = 15, min = 0, max = 100
        private float _edgeDistortion;
        //def = -18, min = -60, max = 0
        private float _gainDistortion;
        //def = 2400, min = 100, max  = 8000
        private float _bandwidthDistortion;
        //def = 2400, min = 100, max  = 8000
        private float _postEQCenterDistortion;
        //def = 8000, min = 100, max = 8000
        private float _preLowPassDistortion;
        //getter and setter
        public bool Distortion
        {
            get { return _distortion; }
            set
            {
                _distortion = value;
                OnDistortionChanged();
            }
        }     
        public float EdgeDistortion
        {
            get { return _edgeDistortion; }
            set
            {
                _edgeDistortion = value;
                OnEdgeDistortionChanged();
            }
        }     
        public float GainDistortion
        {
            get { return _gainDistortion; }
            set
            {
                _gainDistortion = value;
                OnGainDistortionChanged();
            }
        }     
        public float BandwidthDistortion
        {
            get { return _bandwidthDistortion; }
            set
            {
                _bandwidthDistortion = value;
                OnBandwidthDistortionChangedd();
            }
        }   
        public float PostEQCenterDistortion
        {
            get { return _postEQCenterDistortion; }
            set
            {
                _postEQCenterDistortion = value;
                OnPostEQCenterDistortionChanged();
            }
        }
        public float PreLowPassDistortion
        {
            get { return _preLowPassDistortion; }
            set
            {
                _preLowPassDistortion = value;
                OnPreLowPassDistortionChanged();
            }
        }
        //events
        public event OptionsEventHandler DistortionChanged;
        public event OptionsEventHandler EdgeDistortionChanged;
        public event OptionsEventHandler GainDistortionChanged;
        public event OptionsEventHandler BandwidthDistortionChanged;
        public event OptionsEventHandler PostEQCenterDistortionChanged;
        public event OptionsEventHandler PreLowPassDistortionChanged;
        protected virtual void OnDistortionChanged()
        {
            if (DistortionChanged != null)
            {
                DistortionChanged(Distortion);
            }
        }
        protected virtual void OnEdgeDistortionChanged()
        {
            if (EdgeDistortionChanged != null)
            {
                EdgeDistortionChanged(EdgeDistortion);
            }
        }
        protected virtual void OnGainDistortionChanged()
        {
            if (GainDistortionChanged != null)
            {
                GainDistortionChanged(GainDistortion);
            }
        }
        protected virtual void OnBandwidthDistortionChangedd()
        {
            if (BandwidthDistortionChanged != null)
            {
                BandwidthDistortionChanged(BandwidthDistortion);
            }
        }
        protected virtual void OnPostEQCenterDistortionChanged()
        {
            if (PostEQCenterDistortionChanged != null)
            {
                PostEQCenterDistortionChanged(PostEQCenterDistortion);
            }
        }
        protected virtual void OnPreLowPassDistortionChanged()
        {
            if (PreLowPassDistortionChanged != null)
            {
                PreLowPassDistortionChanged(PreLowPassDistortion);
            }
        }


        //Compressor////////////////////////////
        private bool _compressor;
        //def = 10, min = 0.01, max = 500
        private float _attackCompressor;
        //def = 0, min = -60, max = 60
        private float _gainCompressor;
        //def = 4, min = 0, max = 4
        private float _preDelayCompressor;
        //def = 3, min = 1, max = 100
        private float _ratioCompressor;
        //def = 200, min = 50, max = 3000
        private float _releaseCompressor;
        //def = -20, min = -60, max = 0
        private float _treshholdCompressor;

        public bool Compressor
        {
            get { return _compressor; }
            set
            {
                _compressor = value;
                OnCompressorChanged();
            }
        }
        public float AttackCompressor
        {
            get { return _attackCompressor; }
            set
            {
                _attackCompressor = value;
                OnAttackCompressorChanged();
            }
        }
        public float GainCompressor
        {
            get { return _gainCompressor; }
            set
            {
                _gainCompressor = value;
                OnGainCompressorChanged();
            }
        }
        public float PreDelayCompressor
        {
            get { return _preDelayCompressor; }
            set
            {
                _preDelayCompressor = value;
                OnPreDelayCompressorChanged();
            }
        }
        public float RatioCompressor
        {
            get { return _ratioCompressor; }
            set
            {
                _ratioCompressor = value;
                OnRatioCompressorChanged();
            }
        }
        public float ReleaseCompressor
        {
            get { return _releaseCompressor; }
            set
            {
                _releaseCompressor = value;
                OnReleaseCompressorChanged();
            }
        }
        public float TreshholdCompressor
        {
            get { return _treshholdCompressor; }
            set
            {
                _treshholdCompressor = value;
                OnThreshholdCompressorChanged();
            }
        }

        //events
        public event OptionsEventHandler CompressorChanged;
        public event OptionsEventHandler AttackCompressorChanged;
        public event OptionsEventHandler GainCompressorChanged;
        public event OptionsEventHandler PreDelayCompressorChanged;
        public event OptionsEventHandler RatioCompressorChanged;
        public event OptionsEventHandler ReleaseCompressorChanged;
        public event OptionsEventHandler ThreshholdCompressorChanged;
        protected virtual void OnCompressorChanged()
        {
            if (CompressorChanged != null)
            {
                CompressorChanged(Compressor);
            }
        }
        protected virtual void OnAttackCompressorChanged()
        {
            if (AttackCompressorChanged != null)
            {
                AttackCompressorChanged(AttackCompressor);
            }
        }
        protected virtual void OnGainCompressorChanged()
        {
            if (GainCompressorChanged != null)
            {
                GainCompressorChanged(GainCompressor);
            }
        }
        protected virtual void OnPreDelayCompressorChanged()
        {
            if (PreDelayCompressorChanged != null)
            {
                PreDelayCompressorChanged(PreDelayCompressor);
            }
        }
        protected virtual void OnRatioCompressorChanged()
        {
            if (RatioCompressorChanged != null)
            {
                RatioCompressorChanged(RatioCompressor);
            }
        }
        protected virtual void OnReleaseCompressorChanged()
        {
            if (ReleaseCompressorChanged != null)
            {
                ReleaseCompressorChanged(ReleaseCompressor);
            }
        }
        protected virtual void OnThreshholdCompressorChanged()
        {
            if (ThreshholdCompressorChanged != null)
            {
                ThreshholdCompressorChanged(TreshholdCompressor);
            }
        }


        //Flanger//////////////////////////////
        private bool _flanger;
        //def = 2, min = 0, max = 4
        private float _delayFlanger;
        //def = 100, min = 0, max = 100
        private float _depthFlanger;
        //def = -50, min = -99, max = 99
        private float _feedbackFlanger;
        //def = 0.25, min = 0, max = 10
        private float _frequencyFlanger;
        //def = 50, min = 0, max = 100
        private float _wet_DryMixFlanger;

        public bool Flanger
        {
            get { return _flanger; }
            set
            {
                _flanger  = value;
                OnFlangerChanged();
            }
        }
        public float DelayFlanger
        {
            get { return _delayFlanger; }
            set
            {
                _delayFlanger = value;
                OnDelayFlangerChanged();
            }
        }
        public float DepthFlanger
        {
            get { return _depthFlanger; }
            set
            {
                _depthFlanger = value;
                OnDepthFlangerChanged();
            }
        }
        public float FeedbackFlanger
        {
            get { return _feedbackFlanger; }
            set
            {
                _feedbackFlanger = value;
                OnFeedbackFlangerChanged();
            }
        }
        public float FrequencyFlanger
        {
            get { return _frequencyFlanger; }
            set
            {
                _frequencyFlanger = value;
                OnFrequencyFlangerChanged();
            }
        }
        public float Wet_DryMixFlanger
        {
            get { return _wet_DryMixFlanger; }
            set
            {
                _wet_DryMixFlanger = value;
                OnWet_DryMixFlangerChanged();
            }
        }

        //events
        public event OptionsEventHandler FlangerChanged;
        public event OptionsEventHandler DelayFlangerChanged;
        public event OptionsEventHandler DepthFlangerChanged;
        public event OptionsEventHandler FeedbackFlangerChanged;
        public event OptionsEventHandler FrequencyFlangerChanged;
        public event OptionsEventHandler Wet_DryMixFlangerChanged;
        protected virtual void OnFlangerChanged()
        {
            if (FlangerChanged != null)
            {
                FlangerChanged(Flanger);
            }
        }
        protected virtual void OnDelayFlangerChanged()
        {
            if (DelayFlangerChanged != null)
            {
                DelayFlangerChanged(DelayFlanger);
            }
        }
        protected virtual void OnDepthFlangerChanged()
        {
            if (DepthFlangerChanged != null)
            {
                DepthFlangerChanged(DepthFlanger);
            }
        }
        protected virtual void OnFeedbackFlangerChanged()
        {
            if (FeedbackFlangerChanged != null)
            {
                FeedbackFlangerChanged(FeedbackFlanger);
            }
        }
        protected virtual void OnFrequencyFlangerChanged()
        {
            if (FrequencyFlangerChanged != null)
            {
                FrequencyFlangerChanged(FrequencyFlanger);
            }
        }
        protected virtual void OnWet_DryMixFlangerChanged()
        {
            if (Wet_DryMixFlangerChanged != null)
            {
                Wet_DryMixFlangerChanged(Wet_DryMixFlanger);
            }
        }

        //Chorus////////////////////////////////
        private bool _chorus;
        //def = 16, min = 0, max = 20
        private float _delayChorus;
        //def = 10, min = 0, max = 100
        private float _depthChorus;
        //def = 25, min = -99, max = 99
        private float _feedbackChorus;
        //def = false, min = 0, max = 1, 1 = sinus, 0 = triangle
        private int _waveFormChorus;
        //def = 3, min = 0, max = 4
        private float _phaseChorus;
        //def = 1.1, min = 0, max = 10
        private float _frequencyChorus;
        //def = 50, min = 0, max = 100
        private float _wet_DryMixChorus;

        public bool Chorus
        {
            get { return _chorus; }
            set
            {
                _chorus = value;
                OnChorusChanged();
            }
        }
        public float DelayChorus
        {
            get { return _delayChorus; }
            set
            {
                _delayChorus = value;
                OnDelayChorusChanged();
            }
        }
        public float DepthChorus
        {
            get { return _depthChorus; }
            set
            {
                _depthChorus = value;
                OnDepthChorusChanged();
            }
        }
        public float FeedbackChorus
        {
            get { return _feedbackChorus; }
            set
            {
                _feedbackChorus = value;
                OnFeedbackChorusChanged();
            }
        }
        public int WaveFormChorus
        {
            get { return _waveFormChorus; }
            set
            {
                _waveFormChorus = value;
                OnWaveFormChorusChanged();
            }
        }
        public float PhaseChorus
        {
            get { return _phaseChorus; }
            set
            {
                _phaseChorus = value;
                OnPhaseChorusChanged();
            }
        }
        public float FrequencyChorus
        {
            get { return _frequencyChorus; }
            set
            {
                _frequencyChorus = value;
                OnFrequencyChorusChanged();
            }
        }
        public float Wet_DryMixChorus
        {
            get { return _wet_DryMixChorus; }
            set
            {
                _wet_DryMixChorus = value;
                OnWet_DryMixChorusChanged();
            }
        }

        //events
        public event OptionsEventHandler ChorusChanged;
        public event OptionsEventHandler DelayChorusChanged;
        public event OptionsEventHandler DepthChorusChanged;
        public event OptionsEventHandler FeedbackChorusChanged;
        public event OptionsEventHandler WaveFormChorusChanged;
        public event OptionsEventHandler PhaseChorusChanged;
        public event OptionsEventHandler FrequencyChorusChanged;
        public event OptionsEventHandler Wet_DryMixChorusChanged;
        protected virtual void OnChorusChanged()
        {
            if (ChorusChanged != null)
            {
                ChorusChanged(Chorus);
            }
        }
        protected virtual void OnDelayChorusChanged()
        {
            if (DelayChorusChanged != null)
            {
                DelayChorusChanged(DelayChorus);
            }
        }
        protected virtual void OnDepthChorusChanged()
        {
            if (DepthChorusChanged != null)
            {
                DepthChorusChanged(DepthChorus);
            }
        }
        protected virtual void OnFeedbackChorusChanged()
        {
            if (FeedbackChorusChanged != null)
            {
                FeedbackChorusChanged(FeedbackChorus);
            }
        }
        protected virtual void OnWaveFormChorusChanged()
        {
            if (WaveFormChorusChanged != null)
            {
                WaveFormChorusChanged(WaveFormChorus);
            }
        }
        protected virtual void OnPhaseChorusChanged()
        {
            if (PhaseChorusChanged != null)
            {
                PhaseChorusChanged(_phaseChorus);
            }
        }
        protected virtual void OnFrequencyChorusChanged()
        {
            if (FrequencyChorusChanged != null)
            {
                FrequencyChorusChanged(FrequencyChorus);
            }
        }
        protected virtual void OnWet_DryMixChorusChanged()
        {
            if (Wet_DryMixChorusChanged != null)
            {
                Wet_DryMixChorusChanged(Wet_DryMixChorus);
            }
        }

        //Echo//////////////////////////////////
        private bool _echo;
        //def = 50, min = 0, max = 100
        private float _feedbackEcho;
        //def = 500, min = 1, max = 2000
        private float _leftDelayEcho;
        //def = 500, min = 1, max = 2000
        private float _rightDelayEcho;
        //def = false, min = 0, max = 1
        private bool _panDelayEcho;
        //def = 50, min = 0, max = 100
        private float _wet_DryMixEcho;

        //getter and setter
        public bool Echo
        {
            get { return _echo; }
            set
            {
                _echo = value;
                OnEchoChanged();
            }
        }
        public float FeedbackEcho
        {
            get { return _feedbackEcho; }
            set
            {
                _feedbackEcho = value;
                OnFeedbackEchoChanged();
            }
        }
        public float LeftDelayEcho
        {
            get { return _leftDelayEcho; }
            set
            {
                _leftDelayEcho = value;
                OnLeftDelayEchoChanged();
            }
        }
        public float RightDelayEcho
        {
            get { return _rightDelayEcho; }
            set
            {
                _rightDelayEcho = value;
                OnRightDelayEchoChanged();
            }
        }
        public bool PanDelayEcho
        {
            get { return _panDelayEcho; }
            set
            {
                _panDelayEcho = value;
                OnPanDelayEchoChanged();
            }
        }
        public float Wet_DryMixEcho
        {
            get { return _wet_DryMixEcho; }
            set
            {
                _wet_DryMixEcho = value;
                OnWet_DryMixEchoChanged();
            }
        }

        //events
        public event OptionsEventHandler EchoChanged;
        public event OptionsEventHandler FeedbackEchoChanged;
        public event OptionsEventHandler LeftDelayEchoChanged;
        public event OptionsEventHandler RightDelayEchoChanged;
        public event OptionsEventHandler PandelayEchoChanged;
        public event OptionsEventHandler Wet_DryMixEchoChanged;
        protected virtual void OnEchoChanged()
        {
            if (EchoChanged != null)
            {
                EchoChanged(Echo);
            }
        }
        protected virtual void OnFeedbackEchoChanged()
        {
            if (FeedbackEchoChanged != null)
            {
                FeedbackEchoChanged(FeedbackEcho);
            }
        }
        protected virtual void OnLeftDelayEchoChanged()
        {
            if (LeftDelayEchoChanged != null)
            {
                LeftDelayEchoChanged(LeftDelayEcho);
            }
        }
        protected virtual void OnRightDelayEchoChanged()
        {
            if (RightDelayEchoChanged != null)
            {
                RightDelayEchoChanged(RightDelayEcho);
            }
        }
        protected virtual void OnPanDelayEchoChanged()
        {
            if (PandelayEchoChanged != null)
            {
                PandelayEchoChanged(PanDelayEcho);
            }
        }
        protected virtual void OnWet_DryMixEchoChanged()
        {
            if (Wet_DryMixEchoChanged != null)
            {
                Wet_DryMixEchoChanged(Wet_DryMixEcho);
            }
        }

    }
}
