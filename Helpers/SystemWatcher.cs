using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers
{
    class SystemWatcher
    {
        FileSystemWatcher fsw;

        public SystemWatcher(string url)
        {
            fsw = new FileSystemWatcher(url);
            fsw.IncludeSubdirectories = true;
            
        }

        public void On()
        {

        }

    }
}
