using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Model;

namespace TuneMusix.ViewModel.Effects
{
    class EqualizerViewModel
    {
        Options options = Options.Instance;
        public bool EqualizerActive
        {
            get { return options.Equalizer; }
            set { options.Equalizer = value; }
        }

        public float FilterChannel1
        {
            get { return options.Channelfilter[0]; }
            set { options.Channelfilter[0] = value; }
        }
        public float FilterChannel2
        {
            get { return options.Channelfilter[1]; }
            set { options.Channelfilter[1] = value; }
        }
        public float FilterChannel3
        {
            get { return options.Channelfilter[2]; }
            set { options.Channelfilter[2] = value; }
        }
        public float FilterChannel4
        {
            get { return options.Channelfilter[3]; }
            set { options.Channelfilter[3] = value; }
        }
        public float FilterChannel5
        {
            get { return options.Channelfilter[4]; }
            set { options.Channelfilter[4] = value; }
        }
        public float FilterChannel6
        {
            get { return options.Channelfilter[5]; }
            set { options.Channelfilter[5] = value; }
        }
        public float FilterChannel7
        {
            get { return options.Channelfilter[6]; }
            set { options.Channelfilter[6] = value; }
        }
        public float FilterChannel8
        {
            get { return options.Channelfilter[7]; }
            set { options.Channelfilter[7] = value; }
        }
        public float FilterChannel9
        {
            get { return options.Channelfilter[8]; }
            set { options.Channelfilter[8] = value; }
        }
        public float FilterChannel10
        {
            get { return options.Channelfilter[9]; }
            set { options.Channelfilter[9] = value; }
        }
    }
}
