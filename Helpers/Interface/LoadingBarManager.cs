using System;

namespace TuneMusix.Helpers.Interface
{
    /// <summary>
    /// Manager for loading bar events.
    /// </summary>
    public class LoadingBarManager
    {

        #region constructor and instance accessor
        private static volatile LoadingBarManager instance;
        private static object lockObject = new Object();
        public static LoadingBarManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new LoadingBarManager();
                        }
                    }
                }
                return instance;
            }
        }
        private LoadingBarManager(){ }

        #endregion

        private int progress;
        private string message;
        /// <summary>
        /// Sets the value of the loading bar.
        /// </summary>
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnProgressChanged();
            }
        }
        /// <summary>
        /// Sets the message of the loadingbar.
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnInfoTextChanged();
            }
        }
        /// <summary>
        /// Displays the loading bar and the given message.
        /// </summary>
        /// <param name="message"></param>
        public void StartLoading(string message)
        {
            Progress = 0;
            Message = message;
            OnLoadingStarted();
        }
        /// <summary>
        /// Hides the loading bar.
        /// </summary>
        public void EndLoading()
        {
            OnLoadingFinished();
        }


        public delegate void LoadingChangedEventHandler(object obj);
        public event LoadingChangedEventHandler ProgressChanged;
        public event LoadingChangedEventHandler LoadingStarted;
        public event LoadingChangedEventHandler InfoTextChanged;
        public event LoadingChangedEventHandler LoadingFinished;
        protected virtual void OnProgressChanged()
        {
            if (ProgressChanged != null)
                ProgressChanged(Progress);
        }
        protected virtual void OnLoadingStarted()
        {
            if (LoadingStarted != null)
                LoadingStarted(message);
        }
        protected virtual void OnLoadingFinished()
        {
            if (LoadingFinished != null)
                LoadingFinished(this);
        }
        protected virtual void OnInfoTextChanged()
        {
            if (InfoTextChanged != null)
                InfoTextChanged(message);
        }
    }
}
