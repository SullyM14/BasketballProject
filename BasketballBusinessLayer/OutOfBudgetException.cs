using System;
using System.Runtime.Serialization;

namespace BasketballBusinessLayer
{
    [Serializable]
    public class OutOfBudgetException : Exception
    {
        public OutOfBudgetException()
        {
        }

        public OutOfBudgetException(string message) : base(message)
        {
           message = "Player can't be Added. Out of Budget";
        }

        public OutOfBudgetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfBudgetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}