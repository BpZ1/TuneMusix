using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables;
using TuneMusix.Data.Database.Tables.TableValues;

namespace TuneMusix.Tests.DatabaseTests.TableTests
{
    internal class TableStub : TableBase<DatabaseItemStub, TableStub>
    {
        public TableStub() : base( TableName ) { }

        public string FirstValueName = "TestValue1";

        public string SecondValueName = "TestValue2";

        public string ThirdValueName = "TestValue3";

        public string ForeignTableName = "TableName";

        public string ForeignValueName = "ForeignValueName";

        public static string TableName = "TestTable";

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            return new List<SqlValue>()
            {
                new VarCharSqlValue( FirstValueName, 20 ),
                new VarCharSqlValue( SecondValueName, 20 ),
                new IntSqlValue( ThirdValueName )
            };
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>()
            {
                new PrimaryKey( FirstValueName ),
                new ForeignKey( SecondValueName, ForeignTableName, ForeignValueName )
                {
                    OnDeleteCascade = true
                }
            };
        }

        protected override DatabaseItemStub CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            var id = dbReader.GetString( 0 );
            var name = dbReader.GetString( 1 );
            var number = dbReader.GetInt32( 2 );
            return new DatabaseItemStub()
            {
                Id = id,
                Name = name,
                Number = number
            };
        }

        public override void AddCommandParameter( DatabaseItemStub insertionObject, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, insertionObject.Id );
            command.Parameters.AddWithValue( Values[1].Name, insertionObject.Name );
            command.Parameters.AddWithValue( Values[2].Name, insertionObject.Number );
        }

        public override void Delete( DatabaseItemStub deletionItem )
        {
            Delete( Values[0], deletionItem.Id );
        }
    }
}
