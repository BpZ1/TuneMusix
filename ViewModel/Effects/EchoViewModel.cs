using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class EchoViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand SetDefaultEcho { get; set; }

        public EchoViewModel()
        {
            SetDefaultEcho = new RelayCommand(_setDefaultEcho);
        }

        public bool EchoActive
        {
            get { return options.Echo; }
            set { options.Echo = value; }
        }
        public float FeedbackEcho
        {
            get { return options.FeedbackEcho; }
            set { options.FeedbackEcho = value; }
        }
        public float LeftDelayEcho
        {
            get { return options.LeftDelayEcho; }
            set { options.LeftDelayEcho = value; }
        }
        public float RightDelayEcho
        {
            get { return options.RightDelayEcho; }
            set { options.RightDelayEcho = value; }
        }
        public bool PanDelayEcho
        {
            get { return options.PanDelayEcho; }
            set { options.PanDelayEcho = value; }
        }
        public float Wet_DryMixEcho
        {
            get { return options.Wet_DryMixEcho; }
            set { options.Wet_DryMixEcho = value; }
        }

        private void _setDefaultEcho(object argument)
        {
            FeedbackEcho = 50;
            LeftDelayEcho = 500;
            RightDelayEcho = 500;
            PanDelayEcho = false;
            Wet_DryMixEcho = 50;
    }
    }
}
