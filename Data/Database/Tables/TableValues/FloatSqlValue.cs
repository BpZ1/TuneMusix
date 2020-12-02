namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class FloatSqlValue : SqlValue
    {
        public FloatSqlValue() : base( FloatTypeName, typeof( float ) ) { }

        public FloatSqlValue( string name ) : base( name, FloatTypeName, typeof( float ) ) { }

        protected override string GetTypePostfix()
        {
            return string.Empty;
        }

        private const string FloatTypeName = "FLOAT";
    }
}
