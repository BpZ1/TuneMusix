﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.Helpers.Dialogs
{
    public static class DialogService
    {
        /// <summary>
        /// Opens a dialog for a notification message.
        /// </summary>
        /// <param name="Message"></param>
        public static void NotificationMessage(string message)
        {
            ViewModelMain.Notification(message);
        }

        /// <summary>
        /// Opens a dialog for a warningmessage. 
        /// </summary>
        /// <param name="header">Header for the warnmessage. (Maximum = 35 characters long)</param>
        /// <param name="body"></param>
        public static void WarnMessage(string header, string body)
        {
            if (header.Count<char>()>35)
                throw new ArgumentException("Header for warning messages can only be 35 characters or less.");
            
            var win = new WarningDialogWindow { DataContext = new WarningDialog(header,body), };
        
            win.Show();
        }

        /// <summary>
        /// Opens a dialog waiting for user confirmation and returns the result as <see cref="DialogResult"/>
        /// </summary>
        /// <param name="DialogViewModelBase"></param>
        /// <returns><see cref="DialogResult"/></returns>
        public static DialogResult OpenDialog(DialogViewModelBase vm)
        {
            DialogWindow win = new DialogWindow();
            win.DataContext = vm;
            win.ShowDialog();
            DialogResult result = (win.DataContext as DialogViewModelBase).UserDialogResult;
            return result;
        }
        
    }
}