using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using TuneMusix.Data.Database.Tables.TableValues;
using TuneMusix.Helpers;

namespace TuneMusix.Data.Database.Tables
{
    public abstract class TableBase<T, H> : CreatableSingleton<H>, IDatabaseTable<T> where H : new()
    {
        protected SQLiteConnection Connection;

        public string Name { get; private set; }

        public SqlValue PrimaryKey => Values[_primaryKeyIndex];

        public List<SqlValue> Values { get; private set; }

        public List<SQLiteCommand> CurrentCommands { get; } = new List<SQLiteCommand>();

        public TableBase( string name )
        {
            Name = name;
            Connection = DatabaseAccess.GetConnection();
            Values = CreateTableValues().ToList();
            ExecuteQuery( GetTableCreationCommand() );
        }

        protected SQLiteCommand GetTableCreationCommand()
        {
            var values = String.Join( ", ", Values.Select( value => value.GetTableDefinitionValue() ) );
            var keyList = GetTableKeys();
            SetPrimaryKey( keyList );
            var keys = String.Join( ", ", keyList.Select( key => key.GetQuery() ) );
            if ( !string.IsNullOrEmpty( keys ) )
            {
                values = String.Join( ", ", values, keys );
            }
            return new SQLiteCommand( $"CREATE TABLE if not exists {Name} ({values});", Connection );
        }

        private void SetPrimaryKey( IEnumerable<IDatabaseKey> keys )
        {
            PrimaryKey primaryKey = null;
            try
            {
                primaryKey = ( PrimaryKey ) keys.Single( key => key is PrimaryKey );
            }
            catch ( ArgumentNullException )
            {
                //No primary key found
                return;
            }
            catch ( InvalidOperationException )
            {
                //No primary key found
                return;
            }
            _primaryKeyIndex = Values.TakeWhile( value => !( string.Compare( value.Name, primaryKey.Name, true ) == 0 ) ).Count();
        }

        protected SQLiteCommand GetDeletionCommand( string condition )
        {
            var deletionCondition = string.IsNullOrEmpty( condition ) ? $"WHERE {condition}" : string.Empty;
            return new SQLiteCommand( $"DELETE FROM {Name} {deletionCondition});", Connection );
        }

        protected SQLiteCommand GetInsertionCommand( string condition )
        {
            var deletionCondition = string.IsNullOrEmpty( condition ) ? $"WHERE {condition}" : string.Empty;
            return new SQLiteCommand( $"INSERT OR REPLACE INTO {Name});", Connection );
        }

        /// <summary>
        /// Creates the selection command for given conditions.
        /// Empty conditions will select all.
        /// </summary>
        /// <param name="conditions">Example: "ID == 5"</param>
        protected SQLiteCommand GetSelectionCommand( params string[] conditions )
        {
            string selectionConditions = string.Empty;
            foreach ( var condition in conditions )
            {
                selectionConditions = string.Concat( selectionConditions, $" WHERE {condition}" );
            }
            return new SQLiteCommand( $"SELECT * FROM {Name} {selectionConditions};", Connection );
        }

        protected abstract IEnumerable<SqlValue> CreateTableValues();

        protected abstract IEnumerable<IDatabaseKey> GetTableKeys();

        /// <param name="id">Primary key of the item to be selected.</param>
        /// <returns>Value with the given id as primary key or null</returns>
        public T Select( string id )
        {
            if ( string.IsNullOrEmpty( PrimaryKey.Name ) || string.IsNullOrEmpty( id ) )
            {
                return default;
            }
            T result = default;
            SQLiteDataReader dbReader = GetSelectionCommand( $"{PrimaryKey.Name} = '{id}'" ).ExecuteReader();
            try
            {
                dbReader.Read();
                result = CreateObjectFromReader( dbReader );
            }
            finally
            {
                dbReader.Close();
            }
            return result;
        }

        public IEnumerable<T> SelectAll()
        {
            var folderlist = new List<T>();
            var dbReader = GetSelectionCommand().ExecuteReader();
            try
            {
                while ( dbReader.Read() )
                {
                    folderlist.Add( CreateObjectFromReader( dbReader ) );
                }
            }
            finally
            {
                dbReader.Close();
            }
            return folderlist;
        }

        protected abstract T CreateObjectFromReader( SQLiteDataReader dbReader );

        public virtual void Insert( IEnumerable<T> insertionObjects )
        {
            foreach ( var item in insertionObjects )
            {
                Insert( item );
            }
        }

        public virtual void Insert( T insertionObject )
        {
            var sqlcommand = new SQLiteCommand( CreateInsertQuery(), Connection );
            AddCommandParameter( insertionObject, sqlcommand );
            CurrentCommands.Add( sqlcommand );
        }

        public abstract void AddCommandParameter( T insertionObject, SQLiteCommand command );

        public void DeleteAll()
        {
            var sqlClearCommand = new SQLiteCommand( $"DELETE FROM {Name}", Connection );
            var sqlVacuum = new SQLiteCommand( "VACUUM", Connection );
            ExecuteQuery( sqlClearCommand );
            ExecuteQuery( sqlVacuum );
        }

        public abstract void Delete( T deletionItem );

        protected void Delete( SqlValue valueToCompare, string value )
        {
            SQLiteCommand command = new SQLiteCommand( $"DELETE FROM {Name} WHERE {valueToCompare.Name}=@{valueToCompare.Name};", Connection );
            command.Parameters.AddWithValue( valueToCompare.Name, value );
            CurrentCommands.Add( command );
        }

        protected void Delete( SqlValue firstValueToCompare, string firstValue, SqlValue secondValueToCompare, string secondValue )
        {
            SQLiteCommand command = new SQLiteCommand( $"DELETE FROM {Name} WHERE {firstValueToCompare.Name}=@{firstValueToCompare.Name} AND {secondValueToCompare.Name} = @{secondValueToCompare.Name};", Connection );
            command.Parameters.AddWithValue( firstValueToCompare.Name, firstValue );
            command.Parameters.AddWithValue( secondValueToCompare.Name, secondValue );
            CurrentCommands.Add( command );
        }

        public void Delete( IEnumerable<T> deletionItems )
        {
            foreach ( var item in deletionItems )
            {
                Delete( item );
            }
        }

        protected static void ExecuteQuery( SQLiteCommand command )
        {
            BeginCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            EndCommand.ExecuteNonQuery();
        }

        public virtual void ExecuteCommands()
        {
            BeginCommand.ExecuteNonQuery();
            foreach ( SQLiteCommand command in CurrentCommands )
            {
                command.ExecuteNonQuery();
            }
            EndCommand.ExecuteNonQuery();
            CurrentCommands.Clear();
        }

        protected SQLiteCommand GetInsertionCommand( params object[] values )
        {
            if ( _insertQueryValues == null )
            {
                _insertQueryValues = CreateInsertQuery();
            }
            var query = _insertQueryValues;

            SQLiteCommand command = new SQLiteCommand( query, Connection );

            for ( var i = 0; i < values.Length; i++ )
            {
                command.Parameters.AddWithValue( Values[i].Name, values[i] );
            }
            return command;
        }

        protected string CreateInsertQuery()
        {
            var tables = $"INSERT OR REPLACE INTO {Name}";
            var valueNames = string.Join( ", ", Values.Select( value => value.Name ) );
            var valueNamesCommand = $"({valueNames})";
            var values = string.Join( ", ", Values.Select( value => $"@{value.Name}" ) );
            var valuesCommand = $"VALUES({values});";
            return $"{tables} {valueNamesCommand}{valuesCommand}";
        }

        private int _primaryKeyIndex;
        private string _insertQueryValues;
        protected static SQLiteCommand BeginCommand = new SQLiteCommand( "begin", DatabaseAccess.GetConnection() );
        protected static SQLiteCommand EndCommand = new SQLiteCommand( "end", DatabaseAccess.GetConnection() );
        protected const string InsertionOrReplacePrefix = "INSERT OR REPLACE INTO";
    }
}
