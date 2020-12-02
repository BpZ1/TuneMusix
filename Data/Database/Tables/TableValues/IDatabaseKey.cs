namespace TuneMusix.Data.Database.Tables.TableValues
{
    public interface IDatabaseKey
    {
        string Name { get; set; }

        string GetQuery();
    }
}
