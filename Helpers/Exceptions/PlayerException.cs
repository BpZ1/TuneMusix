using System;

namespace TuneMusix.Helpers.Exceptions
{
    class PlayerException : Exception
    {
        public PlayerException() { }

        public PlayerException(String message) : base(message) { }

        public PlayerException(String message, Exception inner) : base(message, inner) { } 
    }
}
