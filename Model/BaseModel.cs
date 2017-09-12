using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for basic functions of model-classes
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        //basic ViewModelBase
        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        
    }
}
