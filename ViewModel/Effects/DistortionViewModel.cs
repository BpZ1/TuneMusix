using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class DistortionViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand SetDefaultDistortion { get; set; }

        public DistortionViewModel()
        {
            SetDefaultDistortion = new RelayCommand(_setDefaultDistortion);
        }

        public float EdgeDistortion
        {
            get { return options.EdgeDistortion; }
            set { options.EdgeDistortion = value; }
        }

        public float GainDistortion
        {
            get { return options.GainDistortion; }
            set { options.GainDistortion = value; }
        }
        public float BandwidthDistortion
        {
            get { return options.BandwidthDistortion; }
            set { options.BandwidthDistortion = value; }
        }
        public float PostEQCenterDistortion
        {
            get { return options.PostEQCenterDistortion; }
            set { options.PostEQCenterDistortion = value; }
        }
        public float PreLowPassDistortion
        {
            get { return options.PreLowPassDistortion; }
            set { options.PreLowPassDistortion = value; }
        }

        public void _setDefaultDistortion(object argument)
        {
            EdgeDistortion = 15;
            GainDistortion = -18;
            BandwidthDistortion = 2400;
            PostEQCenterDistortion = 2400;
            PreLowPassDistortion = 8000;
        }
    }
}
