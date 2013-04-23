using System;

namespace Infrastructure.IoC.NHibernate
{
    [Serializable()]
    public class InvalidFactoryException : System.Exception
    {
        public InvalidFactoryException() : base() { }
        public InvalidFactoryException(string message) : base(message) { }
        public InvalidFactoryException(string message, System.Exception inner) : base(message, inner) { }
 
        protected InvalidFactoryException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
