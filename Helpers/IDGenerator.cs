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
        private bool IsInit = false;

        public void Initialize(long init)
        {
            IDCounter = init;
            IsInit = true;
        }

        public long GetID()
        {
            if (IsInit)
            {
                IDCounter++;
                return IDCounter - 1;
            }
           throw new 
        }
    }
}
