using System;

namespace TuneMusix.Helpers
{
    public abstract class CreatableSingleton<T> where T : new()
    {
        public static T Create()
        {
            if ( _instance != null )
            {
                throw new Exception( "Can't create a singleton that already exists." );
            }
            _instance = new T();
            return _instance;
        }

        public static bool CheckIfExists()
        {
            return _instance != null;
        }

        public static T Instance
        {
            get
            {
                if ( _instance == null )
                {
                    throw new Exception( "Can't get instance of singleton that does not exist." );
                }
                return _instance;
            }
        }

        private static T _instance;
    }
}
