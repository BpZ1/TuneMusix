namespace TuneMusix.Model
{
    /// <summary>
    /// This class is only used to rebuild playlists after reading from the database
    /// </summary>
    public class PlaylistTrack
    {
        public string TrackID { get; set; }
        public string PlaylistID { get; set; }

        public PlaylistTrack( string trackId, string playlistId )
        {
            TrackID = trackId;
            PlaylistID = playlistId;
        }
    }
}
