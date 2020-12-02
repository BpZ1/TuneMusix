using System.Collections.Generic;
using System.Linq;
using TuneMusix.Data.Database.Tables;
using TuneMusix.Model;

namespace TuneMusix.Data.Database
{
    public class PlaylistReconstructor
    {
        public PlaylistReconstructor( PlaylistTable playlistTable, PlaylistTracksTable playlistTracksTable )
        {
            _playlistTable = playlistTable;
            _playlistTracksTable = playlistTracksTable;
        }

        public IEnumerable<Playlist> CreatePlaylists( IEnumerable<Track> tracks )
        {
            var playlists = _playlistTable.SelectAll();
            var playlistsTracks = _playlistTracksTable.SelectAll().GroupBy( x => x.PlaylistID );

            foreach ( var playlistTrackGroup in playlistsTracks )
            {
                var playlist = playlists.Single( x => x.Id.Equals( playlistTrackGroup.Key ) );
                playlist.AddRange( playlistTrackGroup
                    .Select( playlistTrack => tracks.Single( track => track.Id.Equals( playlistTrack.TrackID ) ) ) );
            }
            return playlists;
        }

        private PlaylistTable _playlistTable;
        private PlaylistTracksTable _playlistTracksTable;
    }
}
