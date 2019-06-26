using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// The delay iterative search algorithm works as follows:<br></br>
    /// Every consecutive search that includes the first previous search term will
    /// wait for a set amount of time until the input is finished before searching.
    /// If the term is not included, a new search will be started.
    /// 
    /// </summary>
    public class DelayIterativeSearch
    {
        private Track[] _itemlist;
        private List<Track> _successlist = new List<Track>();
        private List<Track> _failurelist = new List<Track>();
        private bool _itemslistChanged;
        /// <summary>
        /// The previous value that the user entrerd.
        /// </summary>
        private string _oldSearchValue;
        /// <summary>
        /// Value that the user entered for search.
        /// </summary>
        private string _currentSearchValue;
        //This value is the next to be searched after the current.
        private string _queuedSearchValue; 
        private BackgroundWorker _worker = new BackgroundWorker();
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public DelayIterativeSearch(Track[] list)
        {
            if (list == null)
                throw new ArgumentNullException("SearchService initialized with null");

            //Timer for the delay between input and search
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(200);
            _dispatcherTimer.Tick += StartSearchTask;

            _worker.WorkerReportsProgress = false;
            _worker.DoWork += new DoWorkEventHandler(Search);
            _worker.RunWorkerCompleted += OnWorkerCompleted;
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
                OnSearchTaskCompleted(new List<Track>(_itemlist));
            }
            else
            {
                _dispatcherTimer.Stop();
                _currentSearchValue = value;
                //Start the timer that waits for new input                    
                _dispatcherTimer.Start();
            }         
        }
        
        /// <summary>
        /// Starts the search task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSearchTask(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            if (_currentSearchValue != null || _queuedSearchValue != null)
            {
                if(_queuedSearchValue != null) //Check if there is a newer search value.
                {
                    //Update the current value.
                    _currentSearchValue = _queuedSearchValue;
                    _queuedSearchValue = null;
                }

                if (!_worker.IsBusy) //If not busy start next task.
                {                   
                    _worker.RunWorkerAsync(_currentSearchValue);
                }
                else //Queue current value for later.
                {
                    _queuedSearchValue = _currentSearchValue;
                    _currentSearchValue = null;
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
            if(_queuedSearchValue != null)
            {
                StartSearchTask(null, null);
                _queuedSearchValue = null;               
            }
        }
        /// <summary>
        /// The list that is to be searched.
        /// </summary>
        public Track[] Itemlist
        {
            set
            {
                _itemlist = value;
                _itemslistChanged = true;
            }
        }

        private void Search(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            var value = (string)e.Argument;

            if (value == null)
                throw new ArgumentNullException("Search value is null");

            //If the list was changed and needs to be completely searched
            if (_itemslistChanged)
            {
                NewSearch(value);
            }
            else
            {
                //In case value is the same as the previous
                if (_oldSearchValue.Equals(value))
                {
                    e.Result = _successlist;
                    return;
                }                

                //If the value is the old value + more.
                //only need to search the old filtered list.
                else if (_oldSearchValue.Contains(value))
                {
                    List<Track> newFailureList = new List<Track>(_failurelist);
                    foreach (Track item in _failurelist)
                    {
                        if (item.Contains(value))
                        {
                            _successlist.Add(item);
                            newFailureList.Remove(item);
                        }
                    }
                    _failurelist = newFailureList;
                }
                //If the new value is just the last value - some
                //Only need to search the failure list and add
                //the items to the success list.
                else if (value.Contains(_oldSearchValue))
                {                    
                    List<Track> newSuccessList = new List<Track>(_successlist);
                    foreach (Track item in _successlist)
                    {
                        if (!item.Contains(value))
                        {
                            newSuccessList.Remove(item);
                            _failurelist.Add(item);
                        }
                    }
                    _successlist = newSuccessList;
                }
                else //The new value is not and does not contain a
                {
                    NewSearch(value);
                }
            }
            _oldSearchValue = value;
            e.Result = _successlist;
        }
        /// <summary>
        /// Gets called when the itemlist is new or has changed.
        /// </summary>
        /// <param name="value"></param>
        private void NewSearch(string value)
        {
            _successlist = new List<Track>();
            _failurelist = new List<Track>();
            foreach (Track item in _itemlist)
            {
                if (item.Contains(value))
                {
                    _successlist.Add(item);
                }
                else
                {
                    _failurelist.Add(item);
                }
            }
        _itemslistChanged = false;
        }
    }
}
