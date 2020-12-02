using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    public class FolderTable : TableBase<Folder, FolderTable>
    {
        public FolderTable() : base( DatabaseTableNames.Folder ) { }

        protected override Folder CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            string id = dbReader.GetString( 0 );
            string folderid = dbReader.IsDBNull( 1 ) ? null : dbReader.GetString( 1 );
            string url = dbReader.GetString( 2 );
            string name = dbReader.GetString( 3 );
            return new Folder( name, url, id, folderid );
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            List<SqlValue> values = new List<SqlValue>
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
                new VarCharSqlValue( "name", 40 )
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
                new PrimaryKey( Values[0].Name ),
                new ForeignKey( Values[1].Name, DatabaseTableNames.Folder, Values[0].Name )
                {
                    OnDeleteCascade = true
                }
            };
        }

        public override void Delete( Folder folder )
        {
            Delete( PrimaryKey, folder.FolderId );
        }

        public override void AddCommandParameter( Folder folder, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, folder.Id );
            command.Parameters.AddWithValue( Values[1].Name, folder.FolderId );
            command.Parameters.AddWithValue( Values[2].Name, folder.URL );
            command.Parameters.AddWithValue( Values[3].Name, folder.Name );
        }
    }
}
