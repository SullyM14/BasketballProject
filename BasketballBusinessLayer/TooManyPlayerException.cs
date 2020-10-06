using System;
using System.Runtime.Serialization;

namespace BasketballBusinessLayer
{
    [Serializable]
    public class TooManyPlayerException : Exception
    {
        public TooManyPlayerException()
        {
        }

        public TooManyPlayerException(string message) : base(message)
        {
        }

        public TooManyPlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooManyPlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}