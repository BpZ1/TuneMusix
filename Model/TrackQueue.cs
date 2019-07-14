using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Util;

namespace TuneMusix.Model
{
    public class TrackQueue
    {
        private ObservableList<Track> _queue;
        private Track _currentTrack;
        private bool _trackQueueIsShuffled;
        private int _queueIndex;

        public TrackQueue()
        {
            _queue = new ObservableList<Track>();
        }


        public delegate void TrackQueueEventHandler(object sender, object argument);
        public event TrackQueueEventHandler TrackQueueChanged;
        public event TrackQueueEventHandler CurrentTrackChanged;

        protected virtual void OnTrackQueueChanged(object sender, object argument)
        {
            TrackQueueChanged?.Invoke(sender, argument);
        }

        protected virtual void OnCurrentTrackChanged(object sender, object argument)
        {
            CurrentTrackChanged?.Invoke(sender, argument);
        }

        public int QueueIndex
        {
            get { return _queueIndex; }
            set
            {
                if (value > _queue.Count - 1 || value < 0)
                    throw new IndexOutOfRangeException("The given index is not in the range of the current queue.");

                _queueIndex = value;
                SetCurrentTrack(_queue[value]);
            }
        }

        public ObservableList<Track> Queue
        {
            get { return this._queue; }
            set
            {
                if (value != null)
                {
                    if (_queue != null)
                    {
                        if (!ListUtil.UnorderedEqual<Track>(value, _queue))
                            _trackQueueIsShuffled = false;
                    }
                    this._queue = value;

                    //Shuffle track queue if shuffle is activated
                    if (Options.Instance.Shuffle && !_trackQueueIsShuffled)
                        Shuffle();

                    if (value.Count > 0)
                    {
                        CurrentTrack = value.First();
                    }
                    else
                    {
                        CurrentTrack = null;
                    }
                } 
                if(!(value.Count == 0))
                {
                    this.QueueIndex = 0;
                }
                else
                {
                    _queueIndex = 0;
                    SetCurrentTrack(null);
                }

                OnTrackQueueChanged(this, Queue);
            }
        }

        /// <summary>
        /// Increases the queue index by 1.
        /// If the index is at the end of the queue it will be set to 0 again.
        /// </summary>
        public void IncrementQueueIndex()
        {
            if (_queue.Count > 0)
            {
                //Check if the queue index is at the end of the queue
                if(_queueIndex + 1 == _queue.Count)
                {
                    QueueIndex = 0;
                }
                else
                {
                    QueueIndex = _queueIndex + 1;
                }
            }
        }
        /// <summary>
        /// Decreases the queue index by 1.
        /// If the queue index is at 0 before this method is called
        /// the index will be set to the number of elements in the list.
        /// </summary>
        public void DecrementQueueIndex()
        {
            if(_queue.Count > 0)
            {
                //Check if the queue is at position 0.
                if(_queueIndex == 0)
                {
                    QueueIndex = _queue.Count - 1;
                }
                else
                {
                    QueueIndex = _queueIndex - 1;
                }
            }
        }
        /// <summary>
        /// Sets the current track.
        /// If a current track exists, its IsCurrentTrack
        /// value is set to false and the new one's to true.
        /// The track is then updated and set as current.
        /// </summary>
        /// <param name="track"></param>
        private void SetCurrentTrack(Track track)
        {
            if (_currentTrack != null)
                _currentTrack.IsCurrentTrack = false;

            if (track != null)
            {
                //If the queue does not contain the track the track will be set as the only item of the queue.
                if (!_queue.Contains(track))
                {
                    _queue.Clear();
                    _queue.Add(track);
                }

                track.IsCurrentTrack = true;
                //Update the track and check if it is valid.
                FileParser parser = new FileParser();
                if (parser.UpdateTrack(track))
                {
                    track.IsValid = true;
                }
                else
                {
                    track.IsValid = false;
                }

                if (track.IsModified)
                    SaveTrack(track);
            }
            _currentTrack = track;
            OnCurrentTrackChanged(this, _currentTrack);
        }

        public Track CurrentTrack
        {
            get { return this._currentTrack; }
            set
            {
                SetCurrentTrack(value);
                //Update the index
                _queueIndex = _queue.IndexOf(value);
            }
        }

        private void SaveTrack(Track track)
        {
            Database.Instance.Insert(track);
            track.IsModified = false;
        }

        /// <summary>
        /// Shuffles the current trackqueue. 
        /// </summary>
        public void Shuffle()
        {
            Debug.WriteLine("Shuffling");
            _trackQueueIsShuffled = true; //has to be set before setting the queue to avoid loop.
            //Set the index of the tracks, to remember the original position             
            if (_queue == null) return;
            int index = 0;
            foreach (Track track in _queue)
            {
                track.Index = index;
                index++;
            }

            //Shuffle the queue
            List<Track> shuffledQueue = _queue.ToList<Track>();
            ListUtil.Shuffle<Track>(shuffledQueue);
            _queue = new ObservableList<Track>(shuffledQueue);
            _queueIndex = _queue.IndexOf(_currentTrack);
            OnTrackQueueChanged(this, Queue);
        }

        /// <summary>
        /// Unshuffles the tracks by returning them to their original order.
        /// </summary>
        public void UnShuffle()
        {
            Debug.WriteLine("Unshuffling");
            _trackQueueIsShuffled = false;//has to be set before setting the queue to avoid loop.
            //Get the current queue
            List<Track> tempList = _queue.ToList<Track>();
            //sort the queue after index
            IEnumerable<Track> sortedList =
                from track in tempList
                orderby track.Index
                select track;
            //Set queue to the sorted list
            _queue = new ObservableList<Track>(sortedList);
            _queueIndex = _queue.IndexOf(_currentTrack);
            OnTrackQueueChanged(this, Queue);
        }

        public void ChangeTrackQueuePosition(Track track, int position)
        {
            if (track == null)
                throw new ArgumentNullException();

            if (_queue.Contains(track))
            {
                int pos1 = _queue.IndexOf(track);
                Logger.Log("Moved track from queue position " + pos1 + " to position " + position + ".");
                if (position == _queue.Count)//If the new position is at the end of the list
                {
                    _queue.Move(pos1, position - 1);
                    if (track.IsCurrentTrack)
                        QueueIndex = position - 1;
                }
                else
                {
                    _queue.Move(pos1, position);
                    if (track.IsCurrentTrack)
                        QueueIndex = position;
                }
                OnTrackQueueChanged(this, Queue);
            }
        }

        private bool RemoveTrack(Track track)
        {
            if (Queue.Remove(track))
            {
                //If the current track is the one to be removed
                if (CurrentTrack == track)
                {
                    //If there is more than one track in the queue just switch to the next track.
                    if (_queue.Count > 1)
                    {
                        IncrementQueueIndex();
                    }
                    else
                    {
                        CurrentTrack = null;
                    }
                }            
                return true;
            }
            return false;
        }

        public bool Remove(Track track)
        {
            if (RemoveTrack(track))
            {
                OnTrackQueueChanged(this, Queue);
                return true;
            }
            return false;
        }

        public int RemoveRange(IEnumerable<Track> tracks)
        {
            int counter = 0;
            foreach(Track track in tracks)
            {
                if (RemoveTrack(track))
                    counter++;
            }
            if(counter > 0)
            {
                OnTrackQueueChanged(this, Queue);
            }
            return counter;
        }

        public bool Add(Track track)
        {
            if (!Queue.Contains(track))
            {
                Queue.Add(track);
                OnTrackQueueChanged(this, Queue);
                return true;
            }
            return false;
        }

        public int Add(IEnumerable<Track> tracks)
        {
            int counter = 0;
            foreach(Track track in tracks)
            {
                if (!Queue.Contains(track))
                {
                    Queue.Add(track);
                    counter++;
                }
            }
            if(counter > 0)
            {
                OnTrackQueueChanged(this, Queue);
            }          
            return counter;
        }
    }
}
