using System.Collections.Generic;
using TuneMusix.Helpers;
using TuneMusix.Helpers.Dialogs;
using TuneMusix.Model;
using System.Linq;
using System.Diagnostics;
using System;
using System.Threading;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace TuneMusix.Data.DataModelOb
{
    /// <summary>
    /// Contains all Methods that call the parser to
    /// create the initial data.
    /// </summary>
    public partial class DataModel
    {
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Adds Tracks from their url to the datamodel and database.
        /// </summary>
        /// <param name="urls"></param>
        public void AddTracks(List<string> urls)
        {
            sw.Start();
            FileParser fp = new FileParser();
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += onTracksAdded_Completed;
            worker.ProgressChanged += onTracksAdded_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(fp.CreateTracks);
            worker.RunWorkerAsync(urls);
        }

        public void AddTracks(string folderUrl)
        {
            sw.Start();
            FileParser fp = new FileParser();
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += onTracksAdded_Completed;
            worker.ProgressChanged += onTracksAdded_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(fp.CreateFolder);
            worker.RunWorkerAsync(folderUrl);
        }
    
        //is called when the backgroundworker completed his work
        private void onTracksAdded_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Loading of tracks completed.");
            sw.Stop();
            Debug.WriteLine("Loading took " + sw.Elapsed);

            if(e.Error != null)
            {
                Logger.Log("Loading tracks completion error");
                Logger.LogException(e.Error.GetBaseException());
            }
            else if (e.Cancelled)
            {
                DialogService.WarnMessage("Track loading was cancelled!", "");
            }
            else //success
            {
                if(e.Result is List<Track>)
                {
                    List<Track> tracks = (List<Track>)e.Result;
                    if (tracks != null)
                        Add(tracks);
                }
                else
                {
                    Folder folder = (Folder)e.Result;
                    Add(folder);
                }
                  
            }                    
        }

        private void onTracksAdded_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //update loading bar
            Progress = e.ProgressPercentage;
        }
      
    }
}
