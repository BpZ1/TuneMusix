using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TuneMusix.View.Dialog;
using TuneMusix.ViewModel.Dialog;

namespace TuneMusix.Helpers
{
    public static class DialogService
    {
        /// <summary>
        /// Opens a dialog for a notification message.
        /// </summary>
        /// <param name="Message"></param>
        public static void NotificationMessage(string Message)
        {
            
        }

        /// <summary>
        /// Opens a dialog for a warningmessage. 
        /// </summary>
        /// <param name="header">Header for the warnmessage. (Maximum = 35 characters long)</param>
        /// <param name="body"></param>
        public static void WarnMessage(string header,string body)
        {
            if (header.Count<char>()>35)
            {
                throw new ArgumentException("Header for warning messages can only be 35 characters or less.");
            }
            var win = new WarningDialogWindow
            {
                DataContext = new WarningDialog(header,body),
            };
        
            win.Show();
        }

    }
}
