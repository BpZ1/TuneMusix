namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class VarCharSqlValue : SqlValue
    {
        public int Length { get; set; }

        public VarCharSqlValue( int length ) : base( VarCharTypeName, typeof( string ) )
        {
            Length = length;
        }

        public VarCharSqlValue( string name, int length ) : base( name, VarCharTypeName, typeof( string ) )
        {
            Length = length;
        }

        protected override string GetTypePostfix()
        {
            return $"({Length})";
        }

        private const string VarCharTypeName = "VARCHAR";
    }
}
