using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    public class PlaylistTable : TableBase<Playlist, PlaylistTable>
    {
        public PlaylistTable() : base( DatabaseTableNames.Playlists ) { }

        public override void AddCommandParameter( Playlist playlist, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, playlist.Id );
            command.Parameters.AddWithValue( Values[1].Name, playlist.Name );
        }

        public override void Insert( IEnumerable<Playlist> playlists )
        {
            foreach ( var playlist in playlists )
            {
                Insert( playlist );
            }
        }

        public override void Delete( Playlist deletionItem )
        {
            Delete( PrimaryKey, deletionItem.Id );
        }

        protected override Playlist CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            var id = dbReader.GetString( 0 );
            var name = dbReader.GetString( 1 );
            return new Playlist( name, id );
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            var values = new List<SqlValue>()
            {
                new VarCharSqlValue( "ID", 40 )
                {
                    NotNull = true
                },
                new VarCharSqlValue( "Name", Playlist.MaxNameLength )
                {
                    NotNull = true
                }
            };
            return values;
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>()
            {
                new PrimaryKey(Values[0].Name)
            };
        }
    }
}
