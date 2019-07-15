using System.Collections.Generic;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist : ItemContainer<Track>
    {
        public readonly long ID;
        public bool IsModified { get; set; }

        public Playlist(string name, long ID) : base(name)
        {
            this.ID = ID;
        }

        public Playlist(string name, Track track, long ID) : base(name)
        {
            this.ID = ID;
            if (track != null)
            {
                Itemlist.Add(track);
            }
        }

        public Playlist(string name, List<Track> tracks, long ID) : base(name)
        {
            this.ID = ID;
            foreach (Track track in tracks)
            {
                if (track != null)
                {
                    Itemlist.Add(track);
                }
            }
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                IsModified = true;
            }
        }

        public override bool Add(Track track)
        {
            if (base.Add(track))
            {
                IsModified = true;
                return true;
            }
            return false;
        }

        public override int AddRange(IEnumerable<Track> items)
        {
            int added = base.AddRange(items);
            if(added > 0)
            {
                IsModified = true;
            }
            return added;
        }

    }
}
