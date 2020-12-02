namespace TuneMusix.Tests.DatabaseTests.TableTests
{
    internal class DatabaseItemStub
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public int Number { get; set; }

        public override bool Equals( object obj )
        {
            if ( !( obj is DatabaseItemStub databaseItem ) )
            {
                return false;
            }
            if ( Name.Equals( databaseItem.Name ) && Id.Equals( databaseItem.Id ) && Number == databaseItem.Number )
            {
                return true;
            }
            return false;
        }
    }
}
