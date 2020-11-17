using System.Collections.Generic;

namespace TuneMusix.Model
{
    /// <summary>
    /// Class for the Playlist model containing a list of tracks
    /// </summary>
    public class Playlist : ItemContainer<Track>
    {
        public readonly long Id;
        public bool IsModified { get; set; }

        public Playlist( string name, long id ) : base( name )
        {
            Id = id;
        }

        public Playlist( string name, Track track, long id ) : base( name )
        {
            this.Id = id;
            if ( track != null )
            {
                Itemlist.Add( track );
            }
        }

        public Playlist( string name, List<Track> tracks, long id ) : base( name )
        {
            this.Id = id;
            foreach ( Track track in tracks )
            {
                if ( track != null )
                {
                    Itemlist.Add( track );
                }
            }
        }

        public new string Name
        {
            get { return base.Name.Value; }
            set
            {
                base.Name.Value = value;
                IsModified = true;
            }
        }

        public override bool Add( Track track )
        {
            if ( base.Add( track ) )
            {
                IsModified = true;
                return true;
            }
            return false;
        }

        public override int AddRange( IEnumerable<Track> items )
        {
            int added = base.AddRange( items );
            if ( added > 0 )
            {
                IsModified = true;
            }
            return added;
        }

    }
}
