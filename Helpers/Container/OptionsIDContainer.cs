using TuneMusix.Model;

namespace TuneMusix.Helpers.Container
{
    public class OptionsIDContainer
    {
        public Options options;
        public long id;

        public OptionsIDContainer(Options options, long id)
        {
            this.options = options;
            this.id = id;
        }
    }
}
