using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.MediaPlayer;

namespace TuneMusix.Data.DataModelOb
{
    partial class DataModel
    {
        /// <summary>
        /// Loads data from the database in a backgroundworker.
        /// Creates the database if it does not exist
        /// </summary>
        public void DatabaseStartupLoading()
        {
            loader = new SQLLoader();
            loader.LoadFromDB();
            OnDataModelChanged();
            AudioControls.Instance.LoadEffects();
        }

        /// <summary>
        /// Deletes a track from the folder, tracklist and database.
        /// </summary>
        /// <param name="track"></param>
        public void Delete(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {
                if (TrackQueue != null)
                {
                    //check if the track is in the current queue
                    if (TrackQueue.Remove(track))
                    {
                        if(CurrentTrack == track)
                        {
                            CurrentTrack = null;
                        }
                    }
                }
                TrackList.Remove(track);
                foreach (Playlist playlist in Playlists)
                {
                    //delete only from the object because database has foreign keys.
                    playlist.Tracklist.Remove(track);                                
                }
                track.Dispose();
                OnDataModelChanged();
            }
            database.Delete(tracks);
        }

        /// <summary>
        /// Deletes a single Playlist. The tracks will not be deleted.
        /// </summary>
        /// <param name="playlist"></param>
        public void Delete(Playlist playlist)
        {         
            if(Playlists.Remove(playlist))
            {
                database.Delete(playlist);
                if (CurrentPlaylist == playlist)
                {
                    CurrentPlaylist = null;
                }
            }        
        }
        /// <summary>
        ///  Deletes a folder and all of its content from the datamodel/database
        /// </summary>
        /// <param name="folder"></param>
        public void Delete(Folder folder)
        {
            database.Delete(folder);
            DeleteFolderTracks(folder);

            //Delete reference from container
            if (folder.Container != null)
            {
                folder.Container.Folderlist.Remove(folder);
            }
            if (folder.Container == null)
            {
                RootFolders.Remove(folder);
            }
            OnDataModelChanged();
        }
        /// <summary>
        /// Recursive method for deletion of tracks in folders.
        /// </summary>
        /// <param name="folder"></param>
        private void DeleteFolderTracks(Folder folder)
        {
            foreach (Track track in folder.Tracklist)
            {
                TrackList.Remove(track);
                if (TrackQueue.Remove(track))
                {
                    if (CurrentTrack == track)
                    {
                        CurrentTrack = null;
                    }
                }
            }
            foreach (Folder f in folder.Folderlist)
            {
                DeleteFolderTracks(f);
            }
        }
        /// <summary>
        /// Checks if the folder is already contained in the model.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool Contains(Folder folder)
        {
            foreach (Folder f in RootFolders)
            {
                if (f.URL.Equals(folder.URL))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if the track is already contained in the model.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Contains(Track track)
        {
            foreach (Track t in TrackList)
            {
                if (track.sourceURL.Equals(t.sourceURL))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks for a given url if the folder is already contained as root folder.
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <returns></returns>
        public bool Contains(string folderUrl)
        {
            foreach (Folder f in RootFolders)
            {
                if (f.URL.Equals(folderUrl))
                    return true;
            }
            return false;
        }
        #region database loading methods
        //////////////////////////database methods///////////////////////////////////////
        ///DataBaseMethods should only be used to load tracks into the DataModel when////
        ///initializing the prorgam as they avoid all checks for duplicates etc./////////
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Inserts a list of tracks into the datamodel.
        /// If boolean is set to false, duplicate checks are avoided.
        /// </summary>
        /// <param name="tracks"></param>
        public void AddTracks_NoDatabase(List<Track> tracks, bool check)
        {
            foreach (Track track in tracks)
            {
                if (check)
                {
                    if (!Contains(track))
                        TrackList.Add(track);
                }
                else
                {
                    track.IsModified = false;
                    TrackList.Add(track);
                }          
            }
            OnDataModelChanged();
        }
        /// <summary>
        /// DatabaseMethod for Inserting Folders into the DataModel.
        /// </summary>
        /// <param name="folders"></param>
        public void AddRootFolders_NoDatabase(List<Folder> folders)
        {
            foreach (Folder folder in folders)
            {
                RootFolders.Add(folder);
            }
            OnDataModelChanged();
        }
        /// <summary>
        /// DatabaseMethod for Inserting Playlists into the DataModel.
        /// </summary>
        /// <param name="playlists"></param>
        public void AddPlaylists_NoDatabase(List<Playlist> playlists)
        {
            foreach (Playlist playlist in playlists)
            {
                playlist.IsModified = false;
                Playlists.Add(playlist);
            }
            OnDataModelChanged();
        }
        public void AddEffectsToQueue_NoDatabase(List<BaseEffect> effectList)
        {
            foreach (BaseEffect effect in effectList)
            {
                EffectQueue.Add(effect);
            }
            OnEffectQueueChanged();
        }
        /// <summary>
        /// Adds a track to a playlist and triggers the event.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="playlist"></param>
        public void AddTrackToPlaylist_NoDatabase(Track track,Playlist playlist)
        {
            playlist.AddTrack(track);
            OnDataModelChanged();
        }

        //////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Saves the options object in the database
        /// </summary>
        public void SaveOptions()
        {
            database.UpdateOptions(IDGenerator.IDCounter++, Options.Instance);
            Logger.Log("Options saved to database");
        }

        #region insertion methods
        /// <summary>
        /// Adds a Track to the Tracklist after checking if it is already contained.
        /// Should only be used for small quantities.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool Add(Track track)
        {
            if (!Contains(track))
            {
                TrackList.Add(track);
                database.Insert(track);
                OnDataModelChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds all tracks to the model and database.
        /// Should be used for large quantities.
        /// Returns the number of successfully added tracks.
        /// </summary>
        /// <param name="trackList"></param>
        public int Add(List<Track> trackList)
        {
            int trackCount = trackList.Count;
            int added = 0;
            List<Track> uniqueTracks = new List<Track>();
            foreach (Track t in trackList)
            {
                //Check if Track is already loaded
                if (!Contains(t))
                {
                    TrackList.Add(t); //add track to model
                    uniqueTracks.Add(t);
                    added++;
                }
            }
            if (added > 0)
            {
                //Add tracks to database
                database.Insert(uniqueTracks);
                OnDataModelChanged();
                DialogService.NotificationMessage(added + " tracks have been added.");
            }
            if(trackCount > added)
                DialogService.NotificationMessage((trackCount - added) + "could not be added because they already exist.");

            return added;
        }

        /// <summary>
        /// Checks if a folder is already contained in the list, or if it
        /// is a parent/child of an existing folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void Add(Folder folder)
        {
            if (!Contains(folder))
            {
                bool isSubfolder = false;
                foreach(Folder f in RootFolders)
                {
                    if (folder.URL.Contains(f.URL))
                        isSubfolder = true;
                }
                if (!isSubfolder)
                {
                    List<Track> tracks = new List<Track>();
                    List<Folder> folders = GetSubFolders(folder, false);
                    folders.Add(folder);
                    foreach (Folder f in folders)
                    {
                        tracks.AddRange(f.Tracklist);
                    }
                    Console.WriteLine("Tracks added: " + tracks.Count);
                    database.Insert(folders);
                    RootFolders.Add(folder);
                    Add(tracks);
                }
                else
                {
                    DialogService.NotificationMessage("This folder is already contained in another folder.");
                }            
            }
            else
            {
                DialogService.NotificationMessage("Folder already exists.");
            }
        }

        /// <summary>
        /// Adds a playlist to the datamodel and the database.
        /// </summary>
        /// <param name="name"></param>
        public void AddPlaylist(string name)
        {
            bool contained = false;
            foreach (Playlist pl in Playlists)
            {
                if (pl.Name.Equals(name))
                {
                    contained = true;
                }
            }
            if (!contained)
            {

                Playlist playlist = new Playlist(name,IDGenerator.GetID(true));
                Playlists.Add(playlist);
                database.Insert(playlist);
            }
            else
            {
                DialogService.NotificationMessage("The name was already given to another playlist.");
            }          
        }

        /// <summary>
        /// Adds a List of tracks to a playlist.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <param name="playlist"></param>
        public void AddTracksToPlaylist(List<Track> tracklist,Playlist playlist)
        {
            List<Track> checkedTracks = new List<Track>();
            foreach (Track track in tracklist)
            {
                if (!playlist.Tracklist.Contains(track))
                {
                    checkedTracks.Add(track);
                }
            }
            if(checkedTracks.Count > 0)
            {
                database.Insert(playlist, checkedTracks);
                playlist.AddTracks(checkedTracks);
            }  
        }
        #endregion

        /// <summary>
        /// Returns a list of all folders.
        /// If bool is set to true it will only return modified folders.
        /// </summary>
        /// <param name="modified"></param>
        /// <returns></returns>
        public List<Folder> GetAllFolders(bool modified)
        {
            List<Folder> folders = new List<Folder>();
            foreach (Folder folder in RootFolders)
            {
                if (modified)
                {
                    if(folder.IsModified) folders.Add(folder);
                }
                else
                {
                    folders.Add(folder);
                }             
                folders.AddRange(GetSubFolders(folder,modified));
            }
            return folders;
        }

        private List<Folder> GetSubFolders(Folder folder,bool modified)
        {
            List<Folder> subFolders = new List<Folder>();
            foreach (Folder f in folder.Folderlist)
            {
                if (modified)
                {
                    if(f.IsModified) subFolders.Add(f);
                }
                else
                {
                    subFolders.Add(f);
                }               
                subFolders.AddRange(GetSubFolders(f,modified));
            }
            return subFolders;
        }

        #region effect methods
        /// <summary>
        /// Removes a list of tracks from a playlist.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <param name="playlist"></param>
        public void RemoveTracksFromPlaylist(List<Track> tracklist,Playlist playlist)
        {
            foreach (Track track in tracklist)
            {
                if (playlist.Tracklist.Contains(track))
                {
                    playlist.Tracklist.Remove(track);
                }
                else
                {
                    tracklist.Remove(track);
                }
            }
            database.Delete(playlist,tracklist);
        }
        /// <summary>
        /// Removes the track from the playlist and the connection of both from the database.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="playlist"></param>
        public void RemoveTrackFromPlaylist(Track track,Playlist playlist)
        {
            if (playlist.Tracklist.Remove(track))
            {
                database.Delete(playlist,track);
            }
        }
        /// <summary>
        /// Adds an effect to the effectqueue.
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffectToQueue(BaseEffect effect)
        {
            EffectQueue.Add(effect);
            effect.EffectActivated += OnEffectQueueItemChanged;
            OnEffectQueueChanged();
        }
        /// <summary>
        /// Removes an effect from the effectqueue.
        /// </summary>
        /// <param name="effect"></param>
        public void RemoveEffectFromQueue(BaseEffect effect)
        {
            if (EffectQueue.Remove(effect))
            {
                effect.EffectActivated -= OnEffectQueueChanged;
                OnEffectQueueChanged();
            }
        }
        public void ChangeEffectListPosition(BaseEffect effect,int position)
        {
            if (EffectQueue.Contains(effect))
            {
                int pos1 = EffectQueue.IndexOf(effect);
                Logger.Log("Moved effect from queue position " + pos1 + " to position " + position + ".");
                if(position == EffectQueue.Count)
                {
                    EffectQueue.Move(pos1, position-1);
                }
                else
                {
                    EffectQueue.Move(pos1, position);
                }                    
            }
            else
            {
                Logger.Log("Added effect on queue position " + position + ".");
                EffectQueue.Insert(position, effect);              
            }
            OnEffectQueueChanged();
        }
        /// <summary>
        /// Called when an effect changes in a way that requires new loading of the queue.
        /// </summary>
        public void OnEffectQueueItemChanged()
        {
            OnEffectQueueChanged();
        }
        #endregion

        

        /// <summary>
        /// Shuffles the current trackqueue without notify. 
        /// </summary>
        public void ShuffleTrackQueue()
        {
            //Set the index of the tracks, to remember the original position             
            if (trackQueue == null) return;
            int index = 0;
            foreach (Track track in trackQueue)
            {
                track.Index = index;
                index++;
            }

            //Shuffle the queue
            ListUtil.Shuffle<Track>(trackQueue);

            RaisePropertyChanged("TrackQueue");
            OnTrackQueueChanged();
        }

        public void UnShuffleTrackQueue()
        {
            //Get the current queue
            List<Track> tempList = TrackQueue.ToList<Track>();
            //sort the queue after index
            IEnumerable<Track> sortedList =
                from track in tempList
                orderby track.Index
                select track;
            //Set queue to the sorted list
            TrackQueue = sortedList.ToList<Track>();

            RaisePropertyChanged("TrackQueue");
            OnTrackQueueChanged();
        }

        #region propertychanged
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }
        #endregion
    }
}
