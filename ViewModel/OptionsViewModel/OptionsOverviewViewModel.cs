using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            if (_isModified())
            {
                SQLManager manager = new SQLManager();               
                manager.UpdateOptions(IDGenerator.GetID(false),options);
                manager.UpdateEffectQueue(dataModel.EffectQueue.ToList<BaseEffect>());
                options.IsModified = false;
                foreach (BaseEffect effect in dataModel.EffectQueue)
                {
                    effect.IsModified = false;
                }
            }
        }

        private void _cancel(object argument)
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            if (_isModified())
            {
                MessageBoxResult test = MessageBox.Show("Hallo");

                //open window to ask if sure.
            }
            else
            {
                optionsWindow.Close();
            }
        }


        private bool _isModified()
        {
            if (options.IsModified) return true;
            foreach (BaseEffect effect in dataModel.EffectQueue)
            {
                if (effect.IsModified) return true;
                else return false;
            }
            return false;
        }


    }
}
