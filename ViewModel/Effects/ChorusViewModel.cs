using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class ChorusViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand SetDefaultChorus { get; set; }

        public ChorusViewModel()
        {
            SetDefaultChorus = new RelayCommand(_setDefaultChorus);
        }

        public bool ChorusActive
        {
            get { return options.Chorus; }
            set { options.Chorus = value; }
        }

        public float DelayChorus
        {
            get { return options.DelayChorus; }
            set { options.DelayChorus = value; }
        }
        public float DepthChorus
        {
            get { return options.DepthChorus; }
            set { options.DepthChorus = value; }
        }
        public float FeedbackChorus
        {
            get { return options.FeedbackChorus; }
            set { options.FeedbackChorus = value; }
        }
        public int WaveFormChorus
        {
            get { return options.WaveFormChorus; }
            set { options.WaveFormChorus = value; }
        }
        public float FrequencyChorus
        {
            get { return options.FrequencyChorus; }
            set { options.FrequencyChorus = value; }
        }
        public float Wet_DryMixChorus
        {
            get { return options.Wet_DryMixChorus; }
            set { options.Wet_DryMixChorus = value; }
        }

        private void _setDefaultChorus(object argument)
        {       
            DelayChorus = 16;
            DepthChorus = 10;
            FeedbackChorus = 25;
            WaveFormChorus = 0;
            FrequencyChorus = 1.1f;
            Wet_DryMixChorus = 50;
    }
    }
}
