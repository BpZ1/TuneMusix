using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.SQLDatabase
{
    public class SQLLoader
    {
        DataModel dataModel = DataModel.Instance;
        Options options = Options.Instance;
        SQLManager DBmanager = new SQLManager();
        IDGenerator IDgen = IDGenerator.Instance;

        public void LoadFromDB(object sender, DoWorkEventArgs e)
        {
            var watch = new Stopwatch();
            watch.Start();


            //Load options         
            long idgen = DBmanager.GetIDCounterStand();
            if (idgen == 0 || idgen == 1)
            {
                idgen = 2;
            }
            IDgen.Initialize(idgen);
            Debug.WriteLine("Options loading...");
            DBmanager.GetOptions();
            Debug.WriteLine("Options loaded!");
          
            //Load folders
            Debug.WriteLine("Folders loading...");
            List<Folder> FolderList = DBmanager.GetFolders();
            List<Folder> RootList = new List<Folder>();
            Debug.WriteLine("Folders loaded");

            //load tracks
            Debug.WriteLine("Loading Tracks...");
            List<Track> tracklist = DBmanager.GetTracks();
            dataModel.AddTracksDB(tracklist);
            Debug.WriteLine("Tracks loaded!");
            FolderSort(FolderList);
            TrackSort(FolderList);
            foreach (Folder folder in FolderList)
            {
                if (folder.FolderID == 1)
                {
                    RootList.Add(folder);
                }
            }
            dataModel.AddRootFoldersDB(RootList);

            //load playlists
            Debug.WriteLine("Loading Playlists...");
            dataModel.AddPlaylistsDB(DBmanager.GetPlaylists());          
            foreach (Playlist playlist in dataModel.Playlists)
            {
                List<PlaylistTrack> playlistTracks = DBmanager.GetPlaylistTracks(playlist);
                foreach (PlaylistTrack pt in playlistTracks)
                {
                    foreach (Track track in dataModel.TrackList)
                    {
                        if (track.ID == pt.TrackID)
                        {
                            dataModel.AddTrackToPlaylistDB(track,playlist);
                        }
                    }
                }
            }
            Debug.WriteLine("Playlists loaded!");
            Debug.WriteLine("Loading finished");

            watch.Stop();
            Debug.WriteLine("Time: "+ watch.ElapsedMilliseconds);

        }


        private List<Folder> FolderSort(List<Folder> FolderList)
        {
            List<Folder> templist = FolderList;
            foreach (Folder a in templist)
            {
                a.IsModified = false;
                foreach (Folder b in templist)
                {
                    if (a.ID == b.FolderID)
                    {
                        a.InsertFolder(b);
                    }
                }
            }
            return templist;
        }
        private List<Folder> TrackSort(List<Folder> FolderList)
        {
            List<Folder> tempFolderList = FolderList;
            foreach (Track track in dataModel.TrackList)
            {
                foreach (Folder folder in FolderList)
                {
                    if (track.FolderID == folder.ID)
                    {
                        folder.AddTrack(track);
                        track.IsModified = false;
                    }
                }
            }
            return tempFolderList;
        }
     
    }
}
