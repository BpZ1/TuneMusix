using System;
using System.Collections.Generic;
using System.Linq;
using TuneMusix.Model;

namespace TuneMusix.Data.Database
{
    public class AlbumReconstructor
    {
        public IEnumerable<Album> CreateAlbums( IEnumerable<Track> tracks )
        {
            var albums = new List<Album>();
            var albumGroups = tracks.GroupBy( track => track.Album.Value, StringComparer.InvariantCultureIgnoreCase );
            foreach ( var albumGroup in albumGroups )
            {
                var albumName = albumGroup.Key != null ? albumGroup.Key : "Unknown";
                var album = new Album( albumName );
                var v = album.AddRange( albumGroup.ToList() );
                albums.Add( album );
            }
            return albums;
        }
    }
}
