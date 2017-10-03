using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Attributes;
using TuneMusix.Helpers;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

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
        #region database methods
        //////////////////////////database methods///////////////////////////////////////
        ///DataBaseMethods should only be used to load tracks into the DataModel when////
        ///initializing the prorgam as they avoid all checks for duplicates etc./////////
        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// DatabaseMethod for adding tracks.
        /// Should only be used for loading from the database as it avoids all checks
        /// </summary>
        /// <param name="tracks"></param>
        [DatabaseMethod]
        public void AddTracksDB(List<Track> tracks)
        {
            foreach (Track track in tracks)
            {
                track.IsModified = false;
                TrackList.Add(track);
            }
            OnDataModelChanged();
        }
        /// <summary>
        /// DatabaseMethod for Inserting Folders into the DataModel.
        /// </summary>
        /// <param name="folders"></param>
        [DatabaseMethod]
        public void AddRootFoldersDB(List<Folder> folders)
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
        public void AddPlaylistsDB(List<Playlist> playlists)
        {
            foreach (Playlist playlist in playlists)
            {
                playlist.IsModified = false;
                Playlists.Add(playlist);
            }
            OnDataModelChanged();
        }
        [DatabaseMethod]
        public void AddEffectsToQueueDB(List<BaseEffect> effectList)
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
        public void AddTrackToPlaylistDB(Track track,Playlist playlist)
        {
            playlist.AddTrack(track);
            OnDataModelChanged();
        }

        //////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        /// Adds a Track to the Tracklist after checking if it is already contained.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool AddTrack(Track track)
        {
            bool contained = false;
            foreach (Track t in TrackList)
            {
                if (track.sourceURL.Equals(t.sourceURL))
                {
                    contained = true;
                }
            }
            if (!contained)
            {
                TrackList.Add(track);
                OnDataModelChanged();
                return true;
            }
            return false;
        }

        public void SaveOptions(double IDgenStand)
        {
            DBManager.UpdateOptions(IDGenerator.IDCounter++, Options.Instance);
        }

        /// <summary>
        /// Checks if a folder is already contained in the list, or if it
        /// is a parent/child of an existing folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public void AddRootFolder(Folder folder)
        {
            if (!RootFolders.Contains(folder))
            {
                RootFolders.Add(folder);

                TempFolderList = new List<Folder>();
                TempTrackList = new List<Track>();
                TempFolderList.Add(folder);
                AddFolderContent(folder);
                OnDataModelChanged();
                DBManager.AddAll(TempFolderList);
                DBManager.AddAll(TempTrackList);
                TempFolderList = null;
                TempTrackList = null;
            }
        }

        //Lists used by the AddFolderContent Method to
        //add their content to the database/datamodel.
        private List<Folder> TempFolderList;
        private List<Track> TempTrackList;

        /// <summary>
        /// A method to get all the content from a folder.
        /// </summary>
        /// <param name="folder"></param>
        private void AddFolderContent(Folder folder)
        {
            foreach (Track t in folder.Tracklist)
            {
                this.AddTrack(t);
                this.TempTrackList.Add(t);
            }
            foreach (Folder f in folder.Folderlist)
            {
                this.AddFolderContent(f);
                this.TempFolderList.Add(f);
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
                DBManager.AddPlaylist(playlist);
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
            DBManager.AddAllPlaylistTracks(playlist,tracklist);
            playlist.AddTracks(checkedTracks);
        }
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
            Console.WriteLine("POSITION: " + position);
            if (EffectQueue.Contains(effect))
            {
                int pos1 = EffectQueue.IndexOf(effect);
                Logger.Log("Moved effect from queue position " + pos1 + " to position " + position + ".");
                EffectQueue.Move(pos1,position-1);            
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

    }
}
