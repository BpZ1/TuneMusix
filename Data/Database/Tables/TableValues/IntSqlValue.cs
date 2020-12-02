namespace TuneMusix.Data.Database.Tables.TableValues
{
    public class IntSqlValue : SqlValue
    {
        public bool IsUnsigned { get; set; }

        public IntSqlValue( bool isUnsigned ) : base( IntTypeName, typeof( int ) )
        {
            IsUnsigned = isUnsigned;
        }

        public IntSqlValue( string name ) : base( name, IntTypeName, typeof( int ) ) { }

        public IntSqlValue( string name, bool isUnsigned ) : this( name )
        {
            IsUnsigned = isUnsigned;
        }

        private static string IntTypeName = "INT";

        protected override string GetTypePostfix()
        {
            return IsUnsigned ? "UNSIGNED" : null;
        }
    }
}
