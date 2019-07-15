using TuneMusix.Model;

namespace TuneMusix.Helpers.Container
{
    public class OptionsIDContainer
    {
        public Options Options;
        public long ID;

        public OptionsIDContainer(Options options, long id)
        {
            this.Options = options;
            this.ID = id;
        }
    }
}
