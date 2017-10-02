﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;

namespace TuneMusix.ViewModel.OptionsViewModel
{
    class OptionsOverviewViewModel
    {
        private Options options = Options.Instance;
        private DataModel dataModel = DataModel.Instance;

        public RelayCommand Apply { get; set; }
        public RelayCommand Cancel { get; set; }

        public OptionsOverviewViewModel()
        {
            Apply = new RelayCommand(_apply);
            Cancel = new RelayCommand(_cancel);
        }

        private void _apply(object argument)
        {           
            if (options.IsModified)
            {
                SQLManager manager = new SQLManager();               
                manager.UpdateOptions(IDGenerator.GetID(false),options);
                manager.UpdateEffectQueue(dataModel.EffectQueue.ToList<BaseEffect>());
                options.IsModified = false;
            }
        }

        private void _cancel(object argument)
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            if (options.IsModified)
            {

                //open window to ask if sure.
            }
            else
            {
                optionsWindow.Close();
            }
        }
    }
}
