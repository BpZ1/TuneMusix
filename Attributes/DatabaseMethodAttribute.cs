using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class DatabaseMethodAttribute : Attribute
    {
        public string message { get; set; }

    }
}
