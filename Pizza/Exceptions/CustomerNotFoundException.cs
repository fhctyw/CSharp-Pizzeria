﻿using System.Runtime.Serialization;

namespace Pizza
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
        {
        }

        public CustomerNotFoundException(string? message) : base(message)
        {
        }

        public CustomerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
