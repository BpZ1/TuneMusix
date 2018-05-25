using System.Collections.Generic;
using TuneMusix.Helpers.MediaPlayer.Effects;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    interface IDatabase
    {
        
   
        #region deletion methods
        void Delete(List<Track> tracks);

        /// <summary>
        /// deletes a track from the tracks database.
        /// </summary>
        /// <param name="track"></param>
        void Delete(Track track);

        void Delete(List<Folder> folders);

        /// <summary>
        /// Deletes a folder from the folders database.
        /// </summary>
        /// <param name="folder"></param>
        void Delete(Folder folder);

        /// <summary>
        /// Deletes a playlist from the playlists database.
        /// </summary>
        /// <param name="playlist"></param>
        void Delete(Playlist playlist);

        /// <summary>
        /// Deletes a list of tracks from a playlist in the database.
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="tracklist"></param>
        void Delete(Playlist playlist, List<Track> tracks);

        /// <summary>
        /// Deletes a single Track from a playlist in the database.
        /// </summary>
        /// <param name="playlist"></param>
        /// <param name="track"></param>
        void Delete(Playlist playlist, Track track);
        #endregion

        #region insertion methods
        /// <summary>
        /// Adds a given Track to the Database.
        /// </summary>
        /// <param name="track"></param>
        void Insert(Track track);

        void Insert(List<Track> tracklist);

        /// <summary>
        /// Inserts a list of folders into the database.
        /// Inserting a single folder is unefficient because there
        /// is a high propability that the folder contains other folders.
        /// </summary>
        /// <param name="folders"></param>
        void Insert(List<Folder> folders);

        /// <summary>
        /// Adds a playlist to the database.
        /// </summary>
        /// <param name="playlist"></param>
        void Insert(Playlist playlist);

        /// <summary>
        /// Clears the options table and updates it with new values.
        /// </summary>
        /// <param name="IDGenStand"></param>
        /// <param name="options"></param>
        void UpdateOptions(long IDGenStand, Options options);

        /// <summary>
        /// Clears the effect queue table and updates it with new values.
        /// </summary>
        /// <param name="effectQueue"></param>
        void UpdateEffectQueue(List<BaseEffect> effectQueue);
        #endregion

        #region selection methods

        void LoadOptions();
        long GetIDCounterStand();

        /// <summary>
        /// Returns all Tracks contained in the Database.
        /// </summary>
        /// <returns></returns>
        List<Track> GetTracks();
        List<Folder> GetFolders();
        List<Playlist> GetPlaylists();

        /// <summary>
        /// returns all PlaylistTrack objects for a given Playlist.
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        List<PlaylistTrack> GetPlaylistTracks(Playlist playlist);
        List<BaseEffect> GetEffects();

        #endregion
    }
}
