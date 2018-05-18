using System.Threading;
using TuneMusix.Data.DataModelOb;
using TuneMusix.Data.SQLDatabase;
using TuneMusix.Exceptions;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Class for generation of unique ids.
    /// </summary>
    public class IDGenerator
    {
        public static long IDCounter;
        private static bool IsInit = false;
        private static object lockObject = new object();

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

        /// <summary>
        /// Returns a unique ID 
        /// </summary>
        /// <param name="saving">Set to true if the current ID should be permanently saved</param>
        /// <returns></returns>
        public static long GetID(bool saving)
        {
            if (IsInit && !saving)
            {
                return Interlocked.Increment(ref IDCounter)-1;
            }
            else if(IsInit && saving)
            {
                lock (lockObject)
                {
                    long id = Interlocked.Increment(ref IDCounter);
                    DataModel dm = DataModel.Instance;
                    dm.SaveOptions();
                    return id - 1;
                }
            }
            throw new ClassNotInitializedException("IDGenerator can only be used after it was initialized.");
        }
    }
}
