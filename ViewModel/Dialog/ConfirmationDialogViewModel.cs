﻿using System.Windows;
using System.Windows.Input;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;

namespace TuneMusix.ViewModel.Dialog
{
    /// <summary>
    /// ViewModel for the confirmation dialog.
    /// </summary>
    class ConfirmationDialogViewModel : DialogViewModelBase
    {

        public ConfirmationDialogViewModel()
        {
            yesCommand = new RelayCommand(onYesClick);
            noCommand = new RelayCommand(onNoClick);
            cancelCommand = new RelayCommand(onCancelClick);
        }

        private ICommand yesCommand = null;
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        private ICommand cancelCommand = null;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }

        private void onYesClick(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
        }

        private void onNoClick(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }

        private void onCancelClick(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Undefined);
        }
    }
}
