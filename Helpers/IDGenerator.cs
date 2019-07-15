using System.Threading;
using TuneMusix.Exceptions;
using TuneMusix.Model;

namespace TuneMusix.Helpers
{
    /// <summary>
    /// Class for generation of unique ids.
    /// </summary>
    public class IDGenerator
    {
        public static long IDCounter;
        private static bool _isInitialized = false;
        private static readonly object _lockObject = new object();

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
            _isInitialized = true;
        }

        /// <summary>
        /// Returns a unique ID 
        /// </summary>
        /// <param name="saving">Set to true if the current ID should be permanently saved</param>
        /// <returns></returns>
        public static long GetID(bool saving)
        {
            if (_isInitialized && !saving)
            {
                return Interlocked.Increment(ref IDCounter)-1;
            }
            else if(_isInitialized && saving)
            {
                lock (_lockObject)
                {
                    long id = Interlocked.Increment(ref IDCounter);
                    Options.Instance.SaveValues();
                    return id - 1;
                }
            }
            throw new ClassNotInitializedException("IDGenerator can only be used after it was initialized.");
        }
    }
}
