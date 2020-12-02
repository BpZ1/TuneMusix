using System.Collections.Generic;
using System.Data.SQLite;

namespace TuneMusix.Data.Database.Tables
{
    internal interface IDatabaseTable<T>
    {
        string Name { get; }

        List<SQLiteCommand> CurrentCommands { get; }

        T Select( string id );

        IEnumerable<T> SelectAll();

        void Insert( IEnumerable<T> insertionObjects );

        void Insert( T insertionObject );

        void DeleteAll();

        void Delete( T deletionItem );

        void Delete( IEnumerable<T> deletionItems );

        void ExecuteCommands();
    }
}
