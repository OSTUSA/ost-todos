using System;

namespace Infrastructure.Migrations.Runner
{
    [Serializable()]
    public class EmptyConnectionStringException : System.Exception
    {
        public EmptyConnectionStringException() : base() { }
        public EmptyConnectionStringException(string message) : base(message) { }
        public EmptyConnectionStringException(string message, System.Exception inner) : base(message, inner) { }

        protected EmptyConnectionStringException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
