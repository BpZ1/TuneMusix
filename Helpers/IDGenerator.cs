using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Exceptions;

namespace TuneMusix.Helpers
{
    public class IDGenerator
    {
        public static long IDCounter;
        private static bool IsInit = false;

        private static IDGenerator instance;

        public static IDGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IDGenerator();
                }
                return instance;
            }
        }


        public void Initialize(long init)
        {
            IDCounter = init;
            IsInit = true;
        }

        public static long GetID()
        {
            if (IsInit)
            {
                IDCounter++;
                DataModel dm = DataModel.Instance;
                dm.SaveOptions();
                return IDCounter - 1;
            }
            throw new ClassNotInitializedException("IDGenerator can only be used after it was initialized.");
        }
    }
}
