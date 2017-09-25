using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class CompressorViewModel
    {
        private Options options = Options.Instance;

        public RelayCommand SetDefaultCompressor { get; set; }

        public CompressorViewModel()
        {
            SetDefaultCompressor = new RelayCommand(_setDefaultCompressor);
        }

        public bool CompressorActive
        {
            get { return options.Compressor; }
            set { options.Compressor = value; }
        }
        public float AttackCompressor
        {
            get { return options.AttackCompressor; }
            set { options.AttackCompressor = value; }
        }
        public float GainCompressor
        {
            get { return options.GainCompressor; }
            set { options.GainCompressor = value; }
        }
        public float PreDelayCompressor
        {
            get { return options.PreDelayCompressor; }
            set { options.PreDelayCompressor = value; }
        }
        public float RatioCompressor
        {
            get { return options.RatioCompressor; }
            set { options.RatioCompressor = value; }
        }
        public float ReleaseCompressor
        {
            get { return options.ReleaseCompressor; }
            set { options.ReleaseCompressor = value; }
        }
        public float TreshholdCompressor
        {
            get { return options.TreshholdCompressor; }
            set { options.TreshholdCompressor = value; }
        }
        private void _setDefaultCompressor(object argument)
        {
            AttackCompressor = 10;
            GainCompressor = 0;
            PreDelayCompressor = 4;
            RatioCompressor = 3;
            ReleaseCompressor = 200;
            TreshholdCompressor = -20;
    }
    }
}
