using System.Collections.Generic;
using System.Data.SQLite;

namespace TuneMusix.Data.Database
{
    public class DatabaseBase
    {
        protected static SQLiteCommand BeginCommand = new SQLiteCommand( "begin", DatabaseAccess.GetConnection() );
        protected static SQLiteCommand EndCommand = new SQLiteCommand( "end", DatabaseAccess.GetConnection() );

        protected static void ExecuteQuery( IEnumerable<SQLiteCommand> commands )
        {
            BeginCommand.ExecuteNonQuery();
            foreach ( SQLiteCommand command in commands )
            {
                command.ExecuteNonQuery();
            }
            EndCommand.ExecuteNonQuery();
        }

        protected static void ExecuteQuery( SQLiteCommand command )
        {
            BeginCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            EndCommand.ExecuteNonQuery();
        }

        protected static void ClearTable( string tableName )
        {
            SQLiteCommand sqlClearCommand = new SQLiteCommand( $"DELETE FROM {tableName}", DatabaseAccess.GetConnection() );
            SQLiteCommand sqlVacuum = new SQLiteCommand( "VACUUM", DatabaseAccess.GetConnection() );
        }


    }
}
