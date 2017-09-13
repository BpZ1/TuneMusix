﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.ViewModel
{
    class FolderPageViewModel : ViewModelBase
    {

        DataModel dataModel;

        //constructor
        public FolderPageViewModel()
        {
            dataModel = DataModel.Instance;
        }

        //getter and setter
        public ObservableCollection<Folder> RootFolders
        {
            get{ return dataModel.RootFolders; }
        }

        public void OnRootFolderlistChanged(object argument)
        {

        }




    }
}