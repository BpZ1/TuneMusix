namespace TuneMusix.Model
{
    /// <summary>
    /// This class is only used to rebuild playlists after reading from the database
    /// </summary>
    class PlaylistTrack
    {
        public PlaylistTrack(long TrackID,long PlaylistID)
        {
            this.TrackID = TrackID;
            this.PlaylistID = PlaylistID;
        }

        public long TrackID { get; set; }
        public long PlaylistID { get; set; }
    }
}
