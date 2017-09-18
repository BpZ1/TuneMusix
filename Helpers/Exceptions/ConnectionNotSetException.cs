using System;

namespace TuneMusix.Exceptions
{
    class ConnectionNotSetException : Exception
    {
        public ConnectionNotSetException()
        {
        }

        public ConnectionNotSetException(string message)
        : base(message)
         {
        }

        public ConnectionNotSetException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
