using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers
{
    class IDGenerator
    {
        public static long IDCounter = 0;

        public long GetID(long init)
        {
            IDCounter = init;
            IDCounter++;
            return IDCounter - 1;
        }
    }
}
