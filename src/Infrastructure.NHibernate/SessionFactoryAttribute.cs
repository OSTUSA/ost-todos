using System;

namespace Infrastructure.NHibernate
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SessionFactory : Attribute
    {
        public string SessionFactoryName { get; private set; }

        public SessionFactory(string factory)
        {
            SessionFactoryName = factory;
        }
    }
}
