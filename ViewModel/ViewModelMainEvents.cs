using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;
using WMPLib;
using WinForms = System.Windows.Forms;

namespace TuneMusix.ViewModel
{
    partial class ViewModelMain
    {
       public void RootFoldersChanged(object source,object obj)
       {
            RaisePropertyChanged("TrackList");
       }
        
      
    }
}
