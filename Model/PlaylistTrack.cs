namespace TuneMusix.Model
{
    /// <summary>
    /// This class is only used to rebuild playlists after reading from the database
    /// </summary>
    public class PlaylistTrack
    {
        public long TrackID { get; set; }
        public long PlaylistID { get; set; }

        public PlaylistTrack(long trackId,long playlistId)
        {
            this.TrackID = trackId;
            this.PlaylistID = playlistId;
        }
    }
}
