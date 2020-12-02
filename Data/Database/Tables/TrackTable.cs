using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    public class TrackTable : TableBase<Track, TrackTable>
    {

        public TrackTable() : base( DatabaseTableNames.Tracks ) { }

        public override void Delete( Track track )
        {
            Delete( PrimaryKey, track.Id );
        }

        protected override Track CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            string trackId = dbReader.GetString( 0 );
            string folderId = dbReader.IsDBNull( 1 ) ? null : dbReader.GetString( 1 );
            string url = dbReader.GetString( 2 );
            string title = dbReader.GetString( 3 );
            string interpret = dbReader.GetString( 4 );
            string album = dbReader.GetString( 5 );
            int year = dbReader.GetInt32( 6 );
            string comm = dbReader.GetString( 7 );
            string genre = dbReader.GetString( 8 );
            int rating = dbReader.GetInt32( 9 );
            string duration = dbReader.GetString( 10 );
            return new Track( url, trackId, folderId, title, interpret, album, year, comm, genre, rating, duration );
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>()
            {
                new PrimaryKey( Values[0].Name ),
                new ForeignKey( Values[1].Name,DatabaseTableNames.Folder, FolderTable.Instance.PrimaryKey.Name )
                {
                    OnDeleteCascade = true
                }
            };
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            List<SqlValue> values = new List<SqlValue>()
            {
                new VarCharSqlValue( "ID", 40 )
                {
                    IsUnique = true,
                    NotNull = true
                },
                new VarCharSqlValue( "folderID", 40 ),
                new VarCharSqlValue( "URL", 100 )
                {
                    NotNull = true
                },
                new VarCharSqlValue( "title", 40 )
                {
                    NotNull = true
                },
                new VarCharSqlValue( "interpret", 40 ),
                new VarCharSqlValue( "album", 40 ),
                new IntSqlValue( "releaseyear", true ),
                new VarCharSqlValue( "comm", 50 ),
                new VarCharSqlValue( "genre", 40 ),
                new IntSqlValue( "rating", true ),
                new VarCharSqlValue( "duration", 20 )
            };
            return values;
        }

        public override void AddCommandParameter( Track track, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, track.Id );
            command.Parameters.AddWithValue( Values[1].Name, track.FolderID.Value );
            command.Parameters.AddWithValue( Values[2].Name, track.SourceURL.Value );
            command.Parameters.AddWithValue( Values[3].Name, track.Title.Value );
            command.Parameters.AddWithValue( Values[4].Name, track.Interpret.Value );
            command.Parameters.AddWithValue( Values[5].Name, track.Album.Value );
            command.Parameters.AddWithValue( Values[6].Name, track.Year.Value );
            command.Parameters.AddWithValue( Values[7].Name, track.Comm.Value );
            command.Parameters.AddWithValue( Values[8].Name, track.Genre.Value );
            command.Parameters.AddWithValue( Values[9].Name, track.Rating.Value );
            command.Parameters.AddWithValue( Values[10].Name, track.Duration );
        }
    }
}
