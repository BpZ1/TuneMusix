using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMPLib;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    class AudioControlsTests
    {
        public WindowsMediaPlayer player = new WindowsMediaPlayer();
        AudioControls audioControls = AudioControls.Instance;

    }
}
