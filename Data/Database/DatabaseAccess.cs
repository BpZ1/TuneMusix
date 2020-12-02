using System;
using System.Data;
using System.Data.SQLite;
using TuneMusix.Data.Database.Tables;
using TuneMusix.Exceptions;

namespace TuneMusix.Data.Database
{
    public class DatabaseAccess : IDisposable
    {

        public static TableManager TableManager { get; private set; }

        public static SQLiteConnection GetConnection()
        {
            if ( _dbConnection == null )
            {
                throw new InvalidOperationException( "The database access needs to be created first." );
            }
            return _dbConnection;
        }

        public static DatabaseAccess Instance
        {
            get
            {
                if ( _instance == null )
                {
                    throw new InvalidOperationException( "DatabaseAccess was not created" );
                }

                return _instance;
            }
        }

        private DatabaseAccess() { }

        public static void Create( string dbFilePath )
        {
            if ( string.IsNullOrEmpty( dbFilePath ) )
            {
                throw new ArgumentException( "The given filePath can't be null" );
            }
            _dbConnection = new SQLiteConnection( $"Data Source={dbFilePath}.db;Version=3;" );
            _beginCommand = new SQLiteCommand( "begin", _dbConnection );
            _endCommand = new SQLiteCommand( "end", _dbConnection );
            OpenDBConnection();
            TableManager = new TableManager();
            _instance = new DatabaseAccess();
        }

        /// <summary>
        /// Opens the connection to the database
        /// </summary>
        private static void OpenDBConnection()
        {
            if ( _dbConnection != null )
            {
                if ( _dbConnection.State != ConnectionState.Open )
                {
                    _dbConnection.Open();
                }
            }
            else
            {
                throw new ConnectionNotSetException( "Connection is null" );
            }
        }

        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        private static void CloseDBConnection()
        {
            if ( _dbConnection != null )
            {
                if ( _dbConnection.State != ConnectionState.Closed )
                {
                    _dbConnection.Close();
                }
            }
        }

        public void Dispose()
        {
            CloseDBConnection();
        }

        private static DatabaseAccess _instance;
        private static SQLiteConnection _dbConnection;
        private static SQLiteDataReader _dbReader;
        private static SQLiteCommand _beginCommand;
        private static SQLiteCommand _endCommand;
    }
}
