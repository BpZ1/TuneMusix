using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    public class FileParser
    {

        private bool workDone = false;
        private int size;
        private const int SPLITNUMBER = 50; //Number of tracks per thread
        private ConcurrentBag<Track> trackList;

        public FileParser()
        {
            trackList = new ConcurrentBag<Track>();
        }

        private void StartTrackCreation(List<string> urls)
        {
            
            List<Track> tempTrackList = new List<Track>();
            //Callback called by the thread once it did its work
            Action<List<Track>> resultCallback = (result) =>
            {
                foreach (Track t in result)
                {
                    this.trackList.Add(t);
                }
                if (trackList.Count == this.size)
                {
                    tempTrackList = trackList.ToList<Track>();
                    workDone = true;
                }
            };

            //workload for the threadpool
            WaitCallback createTracks = (data) =>
            {
                List<Track> tracks = new List<Track>();
                List<string> urlData = (List<string>)data;
                Console.WriteLine("Working thread: " + Thread.CurrentThread.ManagedThreadId);
                foreach (string url in urlData)
                {
                    tracks.Add(GetAudioData(url));
                }
                resultCallback(tracks);
            };
            ThreadPool.QueueUserWorkItem(createTracks, urls);
        }

        public void CreateTracks(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Starting on Thread: " + Thread.CurrentThread.ManagedThreadId);
            List<string> urls = (List<string>)e.Argument;
            this.size = urls.Count;
            //If size is too big, list is split
            if (size > SPLITNUMBER)
            {
                IEnumerable<List<string>> splitLists = ListUtil.SplitList(urls, SPLITNUMBER);
                foreach (List<string> list in splitLists)
                {
                    StartTrackCreation(list);
                }
                while (!workDone)
                {
                    Thread.Sleep(100);
                }
                e.Result = this.trackList.ToList<Track>();
            }
            else
            {
                List<Track> resultList = new List<Track>();
                foreach (string s in urls)
                {
                    resultList.Add(GetAudioData(s));
                }
                e.Result = resultList;
            }
        }


        /// <summary>
        /// Reads the Data from a given URL for Audiofiles
        /// </summary>
        /// <param name="url">URL of the Audiofile</param>
        /// <returns>Track</returns>
        public Track GetAudioData(string url)
        {
            IDGenerator IDgen = IDGenerator.Instance;

            try
            {
                if (url == null) return null;
                Track track = new Track(url, IDGenerator.GetID(false));
                byte[] b = new byte[128];

                TagLib.File f = TagLib.File.Create(url);
                track.Title = f.Tag.Title;
                track.Interpret = f.Tag.FirstAlbumArtist;
                track.Album = f.Tag.Album;
                track.Comm = f.Tag.Comment;
                track.Genre = f.Tag.FirstGenre;
                track.Year = (int)f.Tag.Year;
              
                return track;
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.LogException(ex);
                return null;
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogException(ex);
                return null;
            }
            catch (IOException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// Adds all Files and Folders contained in the given URL to the DataModel.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public Folder GetFolderData(string URL)
        {
            //Add new root Folder
            string[] URLs = URL.Split('\\');
            Folder root = new Folder(URLs.Last(), URL, IDGenerator.GetID(false));

            string[] files = Directory.GetFiles(URL);
            string[] dirs = Directory.GetDirectories(URL);
            //Add all files in the firectory
            foreach (string url in files)
            {
                string extention = Path.GetExtension(url);
                if (extention.Equals(".mp3"))
                {
                    Track track = GetAudioData(url);
                    if(track != null)
                    {                       
                        root.AddTrack(track);
                        track.IsModified = false;
                    }               
                }
            }
            //Recursion for subdirectories
            foreach (string dir in dirs)
            { 
                //Add subfolder to root folder
                Folder fold = GetFolderData(dir);
                root.AddFolder(fold);
                fold.IsModified = false;
            }
            DataModel.Instance.SaveOptions(IDGenerator.IDCounter);      
            return root;
        }

    }
}
