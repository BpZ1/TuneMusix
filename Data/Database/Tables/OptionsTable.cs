using System.Collections.Generic;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Helpers;
using TuneMusix.Model;

namespace TuneMusix.Data.Database.Tables
{
    internal class OptionsTable : TableBase<Options, OptionsTable>
    {
        public OptionsTable() : base( DatabaseTableNames.Options ) { }

        public override void AddCommandParameter( Options options, SQLiteCommand command )
        {
            command.Parameters.AddWithValue( Values[0].Name, options.Volume );
            command.Parameters.AddWithValue( Values[1].Name, options.Shuffle ? 1 : 0 );
            command.Parameters.AddWithValue( Values[2].Name, ( int ) options.RepeatTrack );
            command.Parameters.AddWithValue( Values[3].Name, options.PrimaryColorIndex );
            command.Parameters.AddWithValue( Values[4].Name, options.AccentColorIndex );
            command.Parameters.AddWithValue( Values[5].Name, options.IsDarkMode ? 1 : 0 );
        }

        public override void Delete( Options deletionItem )
        {
            DeleteAll();
        }

        protected override Options CreateObjectFromReader( SQLiteDataReader dbReader )
        {
            var volume = dbReader.GetInt32( 0 );
            var shuffle = dbReader.GetInt32( 1 ) == 0 ? false : true;
            var repeatType = ( RepeatType ) dbReader.GetInt32( 2 );
            var primaryColorIndex = dbReader.GetInt32( 3 );
            var accentColorIndex = dbReader.GetInt32( 4 );
            var isDarkMode = dbReader.GetInt32( 5 ) == 0 ? false : true;
            var askConfirmation = dbReader.GetInt32( 6 ) == 0 ? false : true;
            Options.Create( volume, shuffle, repeatType, primaryColorIndex, accentColorIndex, isDarkMode, askConfirmation );
            return Options.Instance;
        }

        protected override IEnumerable<SqlValue> CreateTableValues()
        {
            return new List<SqlValue>()
            {
                new IntSqlValue( "volume", true ),
                new IntSqlValue( "shuffle", true ),
                new IntSqlValue( "repeatTrack", true ),
                new IntSqlValue( "primaryColor", true ),
                new IntSqlValue( "accentColor", true ),
                new IntSqlValue( "isDarkMode", true ),
                new IntSqlValue( "askConfirmation", true )
            };
        }

        protected override IEnumerable<IDatabaseKey> GetTableKeys()
        {
            return new List<IDatabaseKey>() { };
        }
    }
}
