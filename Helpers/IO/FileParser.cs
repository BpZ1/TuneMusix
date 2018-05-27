using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using TagLib;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// This class contains methods that serve as workload for a BackgroundWorker
    /// to create the tracks and folders.
    /// </summary>
    public class FileParser
    {

        private bool workDone = false;
        private int SPLITNUMBER = 50; //Number of tracks per thread

        public FileParser(){ }

        public FileParser(int splitSize)
        {
            SPLITNUMBER = splitSize;
        }

        /// <summary>
        /// This method is a workload for a backgroundworker and 
        /// sets the result to a list of tracks of the urls given as argument.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateTracks(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            ConcurrentBag<Track> resultList = new ConcurrentBag<Track>();
            List<string> urls = (List<string>)e.Argument;
            int size = urls.Count;
            
            //split the list in parts of a given size
            IEnumerable<List<string>> splitLists = ListUtil.SplitList(urls, SPLITNUMBER);
            int listCount = splitLists.Count<List<string>>();
            int progressCounter = 0;

            //Callback called by the thread once it did its work
            Action<List<Track>> resultCallback = (result) =>
            {
                foreach (Track t in result)
                {
                    resultList.Add(t);
                }
                if (resultList.Count == urls.Count)
                    workDone = true;
            };

            //workload for the threadpool
            WaitCallback createTracks = (data) =>
            {
                List<Track> tracks = new List<Track>();
                List<string> urlData = (List<string>)data;
                foreach (string url in urlData)
                {
                    tracks.Add(getAudioData(url));
                }
                float progress = (float)Interlocked.Increment(ref progressCounter) / (float)listCount * 100;
                worker.ReportProgress((int)Math.Round(progress));
                resultCallback(tracks);
            };

            //start the threads
            foreach(List<string> list in splitLists)
            {
                ThreadPool.QueueUserWorkItem(createTracks, list);
            }

            //wait for the threads to finish
            while (!workDone)
            {
                Thread.Sleep(100);
            }
            Options.Instance.SaveValues();
            e.Result = resultList.ToList<Track>();
        }

        /// <summary>
        /// This method is a workload for a backgroundworker and 
        /// sets the result to the folder of the url given as argument.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        Track audio = getAudioData(file);
                        if (audio != null)
                        {
                            folder.Add(audio);
                            audio.IsModified = false;
                        }
                    }
                }
                resultCallback(folder);
            };
            //start the threads
            foreach (string dirUrl in directoryUrls)
            {
                ThreadPool.QueueUserWorkItem(folderCreation, dirUrl);
            }
            //wait for all threads to finish their work
            while (!workDone)
            {
                Thread.Sleep(100);
            }
            Options.Instance.SaveValues();
            e.Result = folderSort(url, folderList.ToList<Folder>());
        }

        /// <summary>
        /// Reads the Data from a given URL for Audiofiles
        /// </summary>
        /// <param name="url">URL of the Audiofile</param>
        /// <returns>Track</returns>
        private Track getAudioData(string url)
        {
            IDGenerator IDgen = IDGenerator.Instance;
            try
            {
                if (url == null) return null;

                TagLib.File f = TagLib.File.Create(url);
                string title = f.Tag.Title;
                if(title == null){
                    title = Path.GetFileNameWithoutExtension(f.Name);
                }
                else if (title.Equals(""))
                {
                    title = Path.GetFileNameWithoutExtension(f.Name);
                }
                string interpret = f.Tag.FirstAlbumArtist;
                string album = f.Tag.Album;
                string comm = f.Tag.Comment;
                string genre = f.Tag.FirstGenre;
                int year = (int)f.Tag.Year;
                string duration = Converter.TimeSpanToString(f.Properties.Duration);
                Track track = new Track(url, IDGenerator.GetID(false),title, interpret, album, year, comm, genre, duration);
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
            }catch(CorruptFileException ex)
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
                        fold2.Add(fold1);
                        fold1.Container = fold2;
                    }
                    fold2.IsModified = false;
                }
                if (fold1.URL.Equals(rootUrl))
                    root = fold1;

                fold1.IsModified = false;
            }
            return root;
        }

        /// <summary>
        /// Loads all updatable Tags from the file and updates them.
        /// </summary>
        /// <param name="track"></param>
        /// <returns>True if the file was successfully updated.
        /// and False if the file was not found or could not be read.</returns>
        public bool UpdateTrack(Track track)
        {
            try
            {
                if (track == null)
                    throw new ArgumentNullException();

                TagLib.File file = TagLib.File.Create(track.SourceURL);
                string title = file.Tag.Title;
                if (title == null)
                {
                    title = Path.GetFileNameWithoutExtension(file.Name);
                }
                else if (title.Equals(""))
                {
                    title = Path.GetFileNameWithoutExtension(file.Name);
                }
                string interpret = file.Tag.FirstAlbumArtist;
                string album = file.Tag.Album;
                string comm = file.Tag.Comment;
                string genre = file.Tag.FirstGenre;
                int year = (int)file.Tag.Year;

                //Check what need to be updated.
                bool modified = false;

                if (!track.Title.Equals(title) && title != null)
                {
                    track.Title = title;
                    modified = true;
                }

                if (!track.Interpret.Equals(interpret) && interpret != null)
                {
                    track.Interpret = interpret;
                    modified = true;
                }

                if (!track.Album.Equals(album) && album != null)
                {
                    track.Album = album;
                    modified = true;
                }

                if (!track.Comm.Equals(comm) && comm != null)
                {
                    track.Comm = comm;
                    modified = true;
                }

                if (!track.Genre.Equals(genre) && genre != null)
                {
                    track.Genre = genre;
                    modified = true;
                }

                if (!track.Year.Equals(year))
                {
                    track.Year = year;
                    modified = true;
                }


                if (!modified)
                    track.IsModified = false;

            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.LogException(ex);
                DialogService.WarnMessage("File not accessable", ex.Message);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogException(ex);
                DialogService.WarnMessage("File not found", ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                DialogService.WarnMessage("Error while reading file", ex.Message);
                Logger.LogException(ex);
                return false;
            }
            return true;
        }
    }
}
