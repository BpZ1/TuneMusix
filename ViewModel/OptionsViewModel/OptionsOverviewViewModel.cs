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
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;
using TuneMusix.View.OptionsWindow;
using TuneMusix.ViewModel.Dialog;

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
            Apply = new RelayCommand(apply);
            Cancel = new RelayCommand(cancel);
        }
        /// <summary>
        /// Saves the options
        /// </summary>
        /// <param name="argument"></param>
        private void apply(object argument)
        {           
            if (options.IsModified())
            {
                options.Save();
            }
        }

        private void cancel(object argument)
        {
            OptionsWindowView optionsWindow = argument as OptionsWindowView;
            if (options.IsModified())
            {
                //Open a confirmation dialog to check if the user wants to save his changes.
                DialogResult result = openDialog();
       
                if(result == DialogResult.Yes)
                {
                    options.Save();
                    optionsWindow.Close();
                }
                else if(result == DialogResult.No)
                {
                    Console.Out.WriteLine("NO was pressed");
                    //Restoring previous settings
                    SQLLoader loader = new SQLLoader();
                    loader.LoadOptions();
                    optionsWindow.Close();
                }
                else if(result == DialogResult.Undefined)
                {

                }
            }
            else
            {
                optionsWindow.Close();
            }
        }

        private DialogResult openDialog()
        {
            DialogViewModelBase vm = new ConfirmationDialogViewModel();
            DialogResult result = DialogService.OpenDialog(vm);
            return result;
        }

    }
}
