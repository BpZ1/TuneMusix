namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class ForeignKey : IDatabaseKey
    {
        public string Name { get; set; }

        public ForeignKey( string name, string referenceTableName, string referenceValueName )
        {
            Name = name;
            ReferenceTableName = referenceTableName;
            ReferenceValueName = referenceValueName;
        }

        public string ReferenceTableName { get; set; }

        public string ReferenceValueName { get; set; }

        public bool OnDeleteCascade { get; set; }

        public string GetQuery()
        {
            var deleteCascade = OnDeleteCascade ? DeleteCascadePrefix : string.Empty;
            return $"{ForeignKeyPrefix} ({Name}) REFERENCES {ReferenceTableName} ({ReferenceValueName}) {deleteCascade}";
        }

        private const string DeleteCascadePrefix = "ON DELETE CASCADE";
        private const string ForeignKeyPrefix = "FOREIGN KEY";
    }
}
