namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class BoolSqlValue : SqlValue
    {
        public BoolSqlValue() : base( BoolTypeName, typeof( bool ) ) { }

        public BoolSqlValue( string name ) : base( name, BoolTypeName, typeof( bool ) ) { }

        protected override string GetTypePostfix()
        {
            return string.Empty;
        }

        private const string BoolTypeName = "BOOL";
    }
}
