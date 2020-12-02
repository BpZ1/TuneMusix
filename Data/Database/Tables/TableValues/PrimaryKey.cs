namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class PrimaryKey : IDatabaseKey
    {
        public string Name { get; set; }

        public PrimaryKey( string name )
        {
            Name = name;
        }

        public string GetQuery()
        {
            return $"{PrimaryKeyPrefix} ({Name})";
        }

        private const string PrimaryKeyPrefix = "PRIMARY KEY";
    }
}
