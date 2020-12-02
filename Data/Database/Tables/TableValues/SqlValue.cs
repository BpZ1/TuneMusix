using System;

namespace TuneMusix.Data.Database.Tables.TableValues
{
    public abstract class SqlValue
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public bool IsUnique { get; set; }

        public bool NotNull { get; set; }

        public Type Type { get; private set; }

        protected abstract string GetTypePostfix();

        public SqlValue( string typeName, Type type )
        {
            TypeName = typeName;
            Type = type;
        }

        public SqlValue( string name, string typeName, Type type ) : this( typeName, type )
        {
            Name = name;
        }

        public string GetTableDefinitionValue()
        {
            var name = $"{Name} ";
            var typePostfix = GetTypePostfix();
            var type = string.IsNullOrEmpty( typePostfix ) ? $"{TypeName} " : $"{TypeName} {typePostfix} ";
            var postfix1 = IsUnique ? "UNIQUE " : string.Empty;
            var postfix2 = NotNull ? "NOT NULL " : string.Empty;
            return string.Concat( name, type, postfix1, postfix2 );
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
