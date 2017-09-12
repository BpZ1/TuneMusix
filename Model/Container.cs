using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneMusix.Helpers;

namespace TuneMusix.Model
{
    public class Container : BaseModel
    {
        private string name;
        private ObservableCollection<Container> containerList;
        private ObservableCollection<Track> trackList;

        public Container(string name)
        {

            if(name != null && name.Length > 0)
            {
                this.name = name;
                containerList = new ObservableCollection<Container>();
                trackList = new ObservableCollection<Track>();
            }
            else
            {
                //---Need new Exception or maybe no exception
                throw new InvalidOperationException("Name of Container has to be longer then 0");
            }
            
        }

        /// <summary>
        /// Method for Adding a container to a container.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public bool addContainer(Container container)
        {
            ValidationUtil<Container> valiUtil = new ValidationUtil<Container>();
            if(valiUtil.insertValidation(container.name, this.name, container, containerList)){
                containerList.Add(container);
                Logger.log(container.name + " has been added to " + this.name + ".");
                RaisePropertyChanged("containerList");
                return true;
            }else{
                return false;
            }
            
        }

        public bool addTrack(Track track)
        {
            ValidationUtil<Track> valiUtil = new ValidationUtil<Track>();
            if (valiUtil.insertValidation(track.Title, this.name, track, trackList))
            {
                trackList.Add(track);
                Logger.log(track.Title + " has been added to " + this.name + ".");
                RaisePropertyChanged("trackList");
                return true;
            }
            else
            {
                return false;
            }

        }





        /// <summary>
        /// Setter and Getter for the name of the Container
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                if (name != null && name.Length > 0)
                {
                    this.name = value;
                    containerList = new ObservableCollection<Container>();
                    trackList = new ObservableCollection<Track>();
                    RaisePropertyChanged("name");
                }
                else
                {
                    //---Need new Exception or maybe no exception
                    throw new InvalidOperationException("Name of Container has to be longer then 0");
                }
            }
        }




    }
}
