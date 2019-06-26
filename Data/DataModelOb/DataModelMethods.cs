﻿using System;
using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;
using System.Linq;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Helpers.MediaPlayer;
using System.Diagnostics;
using TuneMusix.Helpers.Util;

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
            _loader = new SQLLoader();
            _loader.LoadFromDB();
            OnDataModelChanged();
        }
        public void RemoveTrackFromQueue(Track track)
        {
            //If the track is currently playing
            if(CurrentTrack == track)
                AudioControls.Instance.PlayNext();

            TrackQueue.Remove(track);
        }
        /// <summary>
        /// Deletes a track from the folder, tracklist and database.
        /// </summary>
        /// <param name="track"></param>
        public void Delete(List<Track> tracks)
        {
            bool queueChanged = false;

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
                        queueChanged = true;
                    }
                }
                TrackList.Remove(track);
                foreach (Playlist playlist in Playlists)
                {
                    //delete only from the object because database has foreign keys.
                    playlist.Itemlist.Remove(track);                                
                }
                track.Dispose();
                OnDataModelChanged();
            }
            //Notify that queue has changed
            if (queueChanged)
                OnTrackQueueChanged();

            _database.Delete(tracks);
        }
        /// <summary>
        /// Deletes a single Playlist. The tracks will not be deleted.
        /// </summary>
        /// <param name="playlist"></param>
        public void Delete(Playlist playlist)
        {         
            if(Playlists.Remove(playlist))
            {
                _database.Delete(playlist);
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
            _database.Delete(folder);
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
            bool queueChanged = false;

            foreach (Track track in folder.Itemlist)
            {
                TrackList.Remove(track);
                if(TrackQueue != null)
                {
                    //check if the track is in the current queue
                    if (TrackQueue.Remove(track))
                    {
                        if (CurrentTrack == track)
                        {
                            CurrentTrack = null;
                        }
                        queueChanged = true;
                    }
                }             
            }
            if (queueChanged)
                OnTrackQueueChanged();

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
                if (track.SourceURL.Equals(t.SourceURL))
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

        #region insertion methods
        /// <summary>
        /// Adds a Track to the Tracklist after checking if it is already contained.
        /// Should only be used for small quantities.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool Add(Track track)
        {
            if(track != null)
            {
                if (!Contains(track))
                {
                    TrackList.Add(track);
                    _database.Insert(track);
                    OnDataModelChanged();
                    return true;
                }
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
            List<Track> uniqueTracks = new List<Track>();
            foreach (Track t in trackList)
            {
                if(t != null)
                {
                    //Check if Track is already loaded
                    if (!Contains(t))
                    {
                        uniqueTracks.Add(t);
                    }
                }               
            }
            if (uniqueTracks.Count > 0)
            {
                //Add tracks to database
                _database.Insert(uniqueTracks);
                TrackList.AddRange(uniqueTracks);
                OnDataModelChanged();
                DialogService.NotificationMessage(uniqueTracks.Count + " tracks have been added.");
            }
            if(trackCount > uniqueTracks.Count)
                DialogService.NotificationMessage((trackCount - uniqueTracks.Count) + "could not be added because they already exist.");

            return uniqueTracks.Count;
        }
        /// <summary>
        /// Creates Album and Interpret container if they don't already exist.
        /// </summary>
        /// <param name="track"></param>
        private void CreateContainer(Track track)
        {
            foreach(Album album in Albumlist)
            {
                if (track.Album.ToLower().Equals(album.Name.ToLower()))
                {
                    album.Add(track);
                    track.AlbumContainer = album;
                }
                else
                {
                    Album newAlbum = new Album(track.Album);
                    newAlbum.Add(track);
                    track.AlbumContainer = newAlbum;
                    Albumlist.Add(newAlbum);                 
                }               
            }
            OnAlbumlistChanged();

            foreach(Interpret interpret in Interpretlist)
            {
                if (track.Interpret.ToLower().Equals(interpret.Name.ToLower()))
                {
                    interpret.Add(track);
                    track.InterpretContainer = interpret;
                }
                else
                {
                    Interpret newInterpret = new Interpret(track.Interpret);
                    newInterpret.Add(track);
                    track.InterpretContainer = newInterpret;
                    Interpretlist.Add(newInterpret);
                }
            }
            OnInterpretlistChanged();
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
                        tracks.AddRange(f.Itemlist);
                    }
                    Console.WriteLine("Tracks added: " + tracks.Count);
                    folder.FolderID = 1;
                    _database.Insert(folders);
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
        /// Adds a playlist to the datamodel.
        /// The playlist has to be manually saved to 
        /// the database by the user.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Playlist AddPlaylist(string name)
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
                _database.Insert(playlist);
                OnDataModelChanged();
                return playlist;
            }
            else
            {
                DialogService.NotificationMessage("The name was already given to another playlist.");
                return null;
            }          
        }

        /// <summary>
        /// Adds a List of tracks to a playlist.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <param name="playlist"></param>
        public void AddTracksToPlaylist(List<Track> tracklist,Playlist playlist)
        {
            foreach(Track track in tracklist)
            {
                playlist.Add(track);
            }
            OnDataModelChanged();
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

        /// <summary>
        /// Removes a list of tracks from a playlist.
        /// </summary>
        /// <param name="tracklist"></param>
        /// <param name="playlist"></param>
        public void RemoveTracksFromPlaylist(List<Track> tracklist, Playlist playlist)
        {
            foreach (Track track in tracklist)
            {
                if (playlist.Itemlist.Contains(track))
                {
                    playlist.Itemlist.Remove(track);
                }
                else
                {
                    tracklist.Remove(track);
                }
            }
            _database.Delete(playlist, tracklist);
        }
        /// <summary>
        /// Removes the track from the playlist and the connection of both from the database.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="playlist"></param>
        public void RemoveTrackFromPlaylist(Track track, Playlist playlist)
        {
            if (playlist.Itemlist.Remove(track))
            {
                _database.Delete(playlist, track);
            }
        }

        /// <summary>
        /// Saves all playlists to the database and sets modified to false;
        /// </summary>
        /// <param name="playlists"></param>
        public void SavePlaylists(List<Playlist> playlists)
        {
            foreach (Playlist playlist in playlists)
            {
                _database.Insert(playlist);
                playlist.IsModified = false;
            }
        }
        /// <summary>
        /// Saves all tracks to the database and sets modified to false;
        /// </summary>
        /// <param name="tracklist"></param>
        public void SaveTracks(List<Track> tracklist)
        {
            _database.Insert(tracklist);
            foreach (Track track in tracklist)
            {
                track.IsModified = false;
            }
        }
        /// <summary>
        /// Saves all folders to the database and sets modified to false;
        /// </summary>
        /// <param name="folderlist"></param>
        public void SaveFolders(List<Folder> folderlist)
        {
            _database.Insert(folderlist);
            foreach(Folder folder in folderlist)
            {
                folder.IsModified = false;
            }
        }
        /// <summary>
        /// Saves the folder to the database and sets IsModified to false.
        /// </summary>
        /// <param name="track"></param>
        public void SaveTrack(Track track)
        {      
            _database.Insert(track);
            track.IsModified = false;
        }

        /// <summary>
        /// Shuffles the current trackqueue. 
        /// </summary>
        public void ShuffleTrackQueue()
        {
            Debug.WriteLine("Shuffling");
            _trackQueueIsShuffled = true; //has to be set before setting the queue to avoid loop.
            //Set the index of the tracks, to remember the original position             
            if (_trackQueue == null) return;
            int index = 0;
            foreach (Track track in _trackQueue)
            {
                track.Index = index;
                index++;
            }

            //Shuffle the queue
            List<Track> shuffledQueue = TrackQueue.ToList<Track>();
            ListUtil.Shuffle<Track>(shuffledQueue);
            _trackQueue = new ObservableList<Track>(shuffledQueue);
            QueueIndex = _trackQueue.IndexOf(_currentTrack);
            OnTrackQueueChanged();
        }
        /// <summary>
        /// Unshuffles the tracks by returning them to their original order.
        /// </summary>
        public void UnShuffleTrackQueue()
        {
            Debug.WriteLine("Unshuffling");
            _trackQueueIsShuffled = false;//has to be set before setting the queue to avoid loop.
            //Get the current queue
            List<Track> tempList = TrackQueue.ToList<Track>();
            //sort the queue after index
            IEnumerable<Track> sortedList =
                from track in tempList
                orderby track.Index
                select track;
            //Set queue to the sorted list
            _trackQueue = new ObservableList<Track>(sortedList);
            QueueIndex = _trackQueue.IndexOf(_currentTrack);
            OnTrackQueueChanged(); 
        }

        public void ChangeTrackQueuePosition(Track track, int position)
        {
            if (track == null)
                throw new ArgumentNullException();

            if (TrackQueue.Contains(track))
            {
                int pos1 = TrackQueue.IndexOf(track);
                Logger.Log("Moved track from queue position " + pos1 + " to position " + position + ".");
                if (position == TrackQueue.Count)//If the new position is at the end of the list
                {
                    TrackQueue.Move(pos1, position - 1);
                    if (track.IsCurrentTrack)
                        QueueIndex = position - 1;
                }
                else
                {
                    TrackQueue.Move(pos1, position);
                    if (track.IsCurrentTrack)
                        QueueIndex = position;
                }
                OnTrackQueueChanged();
            }           
        }
    }
}
