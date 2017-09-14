using System;

namespace TuneMusix.Exceptions
{
    class ClassNotInitializedException :Exception
    {
        public ClassNotInitializedException()
        {
        }

        public ClassNotInitializedException(string message)
        : base(message)
         {
        }

        public ClassNotInitializedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
