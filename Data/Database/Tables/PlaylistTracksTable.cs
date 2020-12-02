using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    public class PlaylistTracksTable : TableBase<PlaylistTrack, PlaylistTracksTable>
    {
        public PlaylistTracksTable() : base( DatabaseTableNames.PlaylistTracks ) { }

        public override void AddCommandParameter( PlaylistTrack playlistTrack, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, playlistTrack.TrackID );
            command.Parameters.AddWithValue( Values[1].Name, playlistTrack.PlaylistID );
        }

        public void Delete( Track track )
        {
            Delete( Values[0], track.Id );
        }

        public override void Delete( PlaylistTrack playlistTrack )
        {
            Delete( Values[0], playlistTrack.TrackID, Values[1], playlistTrack.PlaylistID );
        }

        protected override PlaylistTrack CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            var trackId = dbReader.GetString( 0 );
            var playlistId = dbReader.GetString( 1 );
            return new PlaylistTrack( trackId, playlistId );
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            return new List<SqlValue>()
            {
                new VarCharSqlValue( "trackID", 40 ),
                new VarCharSqlValue( "playlistID", 40 )
            };
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>()
            {
                new ForeignKey( Values[0].Name, DatabaseTableNames.Tracks, TrackTable.Instance.PrimaryKey.Name )
                {
                    OnDeleteCascade = true
                },
                new ForeignKey( Values[1].Name, DatabaseTableNames.Playlists, PlaylistTable.Instance.PrimaryKey.Name )
                {
                    OnDeleteCascade = true
                }
            };
        }
    }
}
