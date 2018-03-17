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

            //Callback called by the thread once it did its work
            Action<List<Track>> resultCallback = (result) =>
            {
                foreach (Track t in result)
                {
                    this.trackList.Add(t);
                }
                if (trackList.Count == this.size)
                    workDone = true;
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
            Console.WriteLine("Sender is of type: " + sender.GetType().ToString());
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

        public void CreateFolder(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            string url = (string)e.Argument;
            //list of all folder urls
            List<string> directoryUrls = Directory.EnumerateDirectories(url, "*", SearchOption.AllDirectories).ToList<string>();
            directoryUrls.Add(url);

            int folderCount = directoryUrls.Count;
            int folderLoadCounter = 0;

            //list for saving the folders created by the threads
            ConcurrentBag<Folder> folderList = new ConcurrentBag<Folder>();
            
            //gets called after a folder was successfully created
            Action<Folder> resultCallback = (result) =>
            {
                folderList.Add(result);
                float progress = (float)Interlocked.Increment(ref folderLoadCounter) / (float)folderCount * 100;
                Console.WriteLine("Reported: " + progress);
                worker.ReportProgress((int)Math.Round(progress));
                if (folderList.Count == directoryUrls.Count)
                    workDone = true;
            };

            //creates a folder
            WaitCallback folderCreation = (data) =>
            {
                string folderUrl = (string)data;
                Folder folder = this.createFolder(folderUrl);

                //get the files from the folder
                string[] files = Directory.GetFiles(folderUrl);
                foreach (string file in files)
                {
                    string extention = Path.GetExtension(file);
                    if (extention.Equals(".mp3"))
                    {
                        Track audio = GetAudioData(file);
                        if (audio != null)
                        {
                            folder.AddTrack(audio);
                            audio.IsModified = false;
                        }
                    }
                }
                resultCallback(folder);
            };

            foreach (string dirUrl in directoryUrls)
            {
                ThreadPool.QueueUserWorkItem(folderCreation, dirUrl);
            }

            while (!workDone)
            {
                Thread.Sleep(100);
            }
            DataModel.Instance.SaveOptions(IDGenerator.IDCounter);
            e.Result = folderSort(url, folderList.ToList<Folder>());
        }

        /// <summary>
        /// Reads the Data from a given URL for Audiofiles
        /// </summary>
        /// <param name="url">URL of the Audiofile</param>
        /// <returns>Track</returns>
        private Track GetAudioData(string url)
        {
            IDGenerator IDgen = IDGenerator.Instance;

            try
            {
                if (url == null) return null;
                Track track = new Track(url, IDGenerator.GetID(false));
                byte[] b = new byte[128];

                TagLib.File f = TagLib.File.Create(url);
                string title = f.Tag.Title;
                if(title == null){
                    title = track.Name;
                }
                else if (title.Equals(""))
                {
                    title = track.Name;
                }
                track.Title = title;
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


        //creates a folder from a given url
        private Folder createFolder(string url)
        {
            string[] URLs = url.Split('\\');
            Folder folder = new Folder(URLs.Last(), url, IDGenerator.GetID(false));
            return folder;
        }


        //Puts all folders in their matching parent folder
        private Folder folderSort(string rootUrl, List<Folder> folders)
        {
            Folder root = null;
            foreach (Folder fold1 in folders)
            {
                string parent = Directory.GetParent(fold1.URL).FullName;
                foreach(Folder fold2 in folders)
                {
                    if (parent.Equals(fold2.URL))
                    {
                        fold2.AddFolder(fold1);
                        fold1.Container = fold2;
                    }
                }
                if (fold1.URL.Equals(rootUrl))
                    root = fold1;

                fold1.IsModified = false;
            }
            return root;
        }
    }
}
