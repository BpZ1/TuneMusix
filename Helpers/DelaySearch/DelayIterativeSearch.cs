using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    public class DelayIterativeSearch
    {
        private Track[] itemlist;
        private List<Track> successlist = new List<Track>();
        private List<Track> failurelist = new List<Track>();
        private bool itemslistChanged;
        /// <summary>
        /// The previous value that the user entrerd.
        /// </summary>
        private string oldSearchValue;
        /// <summary>
        /// Value that the user entered for search.
        /// </summary>
        private string currentSearchValue;
        //This value is the next to be searched after the current.
        private string queuedSearchValue; 
        private BackgroundWorker worker = new BackgroundWorker();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public DelayIterativeSearch(Track[] list)
        {
            if (list == null)
                throw new ArgumentNullException("SearchService initialized with null");

            //Timer for the delay between input and search
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(200);
            dispatcherTimer.Tick += startSearchTask;

            worker.WorkerReportsProgress = false;
            worker.DoWork += new DoWorkEventHandler(search);
            worker.RunWorkerCompleted += OnWorkerCompleted;
            Itemlist = list;
        }

        #region events
        public delegate void SearchEventHandler(object obj);
        public event SearchEventHandler SearchTaskCompleted;

        /// <summary>
        /// Gets called to Send the result to the calling class.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void OnSearchTaskCompleted(object result)
        {
            if (SearchTaskCompleted != null)
                SearchTaskCompleted(result);
        }
        #endregion

        /// <summary>
        /// Creates a new search task and queues it after a certain
        /// amount of time expired.
        /// </summary>
        /// <param name="value"></param>
        public void QueueSearchTask(string value)
        {
            if (value == null)
                throw new ArgumentNullException("Search value is null");

            //If the search value is empty return the whole list.
            if (value.Equals(""))
            {
                OnSearchTaskCompleted(new List<Track>(itemlist));
            }
            else
            {
                dispatcherTimer.Stop();
                currentSearchValue = value;
                //Start the timer that waits for new input                    
                dispatcherTimer.Start();
            }         
        }
        
        /// <summary>
        /// Starts the search task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSearchTask(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            if (currentSearchValue != null || queuedSearchValue != null)
            {
                if(queuedSearchValue != null) //Check if there is a newer search value.
                {
                    //Update the current value.
                    currentSearchValue = queuedSearchValue;
                    queuedSearchValue = null;
                }

                if (!worker.IsBusy) //If not busy start next task.
                {                   
                    worker.RunWorkerAsync(currentSearchValue);
                }
                else //Queue current value for later.
                {
                    queuedSearchValue = currentSearchValue;
                    currentSearchValue = null;
                }
            }      
        }
        /// <summary>
        /// Returns the result via event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                DialogService.WarnMessage("Searching error", e.Error.Message);

            if(e.Result != null)
            {
                OnSearchTaskCompleted(e.Result);
            }

            //If there is a new queued item start new search.
            if(queuedSearchValue != null)
            {
                startSearchTask(null, null);
                queuedSearchValue = null;               
            }
        }
        /// <summary>
        /// The list that is to be searched.
        /// </summary>
        public Track[] Itemlist
        {
            set
            {
                itemlist = value;
                itemslistChanged = true;
            }
        }

        private void search(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            var value = (string)e.Argument;

            if (value == null)
                throw new ArgumentNullException("Search value is null");

            //If the list was changed and needs to be completely searched
            if (itemslistChanged)
            {
                newSearch(value);
            }
            else
            {
                //In case value is the same as the previous
                if (oldSearchValue.Equals(value))
                {
                    e.Result = successlist;
                    return;
                }                

                //If the value is the old value + more.
                //only need to search the old filtered list.
                else if (oldSearchValue.Contains(value))
                {
                    List<Track> newFailureList = new List<Track>(failurelist);
                    foreach (Track item in failurelist)
                    {
                        if (item.Contains(value))
                        {
                            successlist.Add(item);
                            newFailureList.Remove(item);
                        }
                    }
                    failurelist = newFailureList;
                }
                //If the new value is just the last value - some
                //Only need to search the failure list and add
                //the items to the success list.
                else if (value.Contains(oldSearchValue))
                {                    
                    List<Track> newSuccessList = new List<Track>(successlist);
                    foreach (Track item in successlist)
                    {
                        if (!item.Contains(value))
                        {
                            newSuccessList.Remove(item);
                            failurelist.Add(item);
                        }
                    }
                    successlist = newSuccessList;
                }
                else //The new value is not and does not contain a
                {
                    newSearch(value);
                }
            }
            oldSearchValue = value;
            e.Result = successlist;
        }
        /// <summary>
        /// Gets called when the itemlist is new or has changed.
        /// </summary>
        /// <param name="value"></param>
        private void newSearch(string value)
        {
            successlist = new List<Track>();
            failurelist = new List<Track>();
            foreach (Track item in itemlist)
            {
                if (item.Contains(value))
                {
                    successlist.Add(item);
                }
                else
                {
                    failurelist.Add(item);
                }
            }
        itemslistChanged = false;
        }
    }
}
