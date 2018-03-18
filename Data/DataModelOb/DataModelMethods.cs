using System;
using System.Collections.Generic;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;
using System.Linq;

namespace TuneMusix.Data.DataModelOb
{
    partial class DataModel
    {
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
            DBManager.Delete(tracks);
        }
        /// <summary>
        /// Deletes a single Playlist. The tracks will not be deleted.
        /// </summary>
        /// <param name="playlist"></param>
        public void Delete(Playlist playlist)
        {         
            if(Playlists.Remove(playlist))
            {
                DBManager.Delete(playlist);
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
            DBManager.Delete(folder);
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
        #region database methods
        //////////////////////////database methods///////////////////////////////////////
        ///DataBaseMethods should only be used to load tracks into the DataModel when////
        ///initializing the prorgam as they avoid all checks for duplicates etc./////////
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Inserts a list of tracks into the datamodel.
        /// If boolean is set to false, duplicate checks are avoided.
        /// </summary>
        /// <param name="tracks"></param>
        [DatabaseMethod]
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
        [DatabaseMethod]
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
        [DatabaseMethod]
        public void AddPlaylists_NoDatabase(List<Playlist> playlists)
        {
            foreach (Playlist playlist in playlists)
            {
                playlist.IsModified = false;
                Playlists.Add(playlist);
            }
            OnDataModelChanged();
        }
        [DatabaseMethod]
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
        [DatabaseMethod]
        public void AddTrackToPlaylist_NoDatabase(Track track,Playlist playlist)
        {
            playlist.AddTrack(track);
            OnDataModelChanged();
        }

        //////////////////////////////////////////////////////////////////////////////
        #endregion

 

        public void SaveOptions(double IDgenStand)
        {
            DBManager.UpdateOptions(IDGenerator.IDCounter++, Options.Instance);
        }

        #region insertion methods
        /// <summary>
        /// Adds a Track to the Tracklist after checking if it is already contained.
        /// Should only be used for small quantities.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool Add(Track track) //NO DATABASE INSERTION
        {
            if (!Contains(track))
            {
                List<Track> trackList = new List<Track>();
                trackList.Add(track);
                TrackList.Add(track);
                DBManager.Insert(trackList);
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
                    TrackList.Add(t);
                    uniqueTracks.Add(t);
                    added++;
                }
            }
            if (added > 0)
            {
                DBManager.Insert(uniqueTracks);
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
                    DBManager.Insert(folders);
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
                DBManager.Insert(playlist);
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
            DBManager.InsertAllPlaylistTracks(playlist,tracklist);
            playlist.AddTracks(checkedTracks);
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
            DBManager.DeletePlaylistTracks(playlist,tracklist);
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
                DBManager.DeletePlaylistTrack(playlist,track);
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
    }
}
