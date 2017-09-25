using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class FlangerViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand SetDefaultFlanger { get; set; }

        public FlangerViewModel()
        {
            SetDefaultFlanger = new RelayCommand(_setDefaultFlanger);
        }

        public bool FlangerActive
        {
            get { return options.Flanger; }
            set { options.Flanger = value; }
        }
        public float DelayFlanger
        {
            get { return options.DelayFlanger; }
            set { options.DelayFlanger = value; }
        }
        public float DepthFlanger
        {
            get { return options.DepthFlanger; }
            set { options.DepthFlanger = value; }
        }
        public float FeedbackFlanger
        {
            get { return options.FeedbackFlanger; }
            set { options.FeedbackFlanger = value; }
        }
        public float FrequencyFlanger
        {
            get { return options.FrequencyFlanger; }
            set { options.FrequencyFlanger = value; }
        }
        public float Wet_DryMixFlanger
        {
            get { return options.Wet_DryMixFlanger; }
            set { options.Wet_DryMixFlanger = value; }
        }

        private void _setDefaultFlanger(object argument)
        {
            DelayFlanger = 2;
            DepthFlanger = 100;
            FeedbackFlanger = -50;
            FrequencyFlanger = 0.25f;
            Wet_DryMixFlanger = 50;
        }
    }
}
