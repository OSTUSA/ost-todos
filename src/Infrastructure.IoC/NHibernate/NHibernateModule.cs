using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.NHibernate;
using Infrastructure.NHibernate.Mapping.Users;
using Infrastructure.NHibernate.Repositories;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Infrastructure.IoC.NHibernate
{
    public class NHibernateModule : NinjectModule
    {
        public static Func<ISessionFactory> DefaultFactory = ConfigureDefaultFactory;

        public Dictionary<string, ISessionFactory> Factories { get; private set; }

        protected static SessionFactoryBuilder Builder = new SessionFactoryBuilder();

        public NHibernateModule(Dictionary<string, ISessionFactory> factories)
        {
            Factories = factories;
            Factories["Default"] = Builder.GetFactory("Default", DefaultFactory);
        }

        public NHibernateModule() : this(new Dictionary<string, ISessionFactory>())
        {
            
        }

        public void Add(string key, Func<ISessionFactory> factory)
        {
            Factories[key] = Builder.GetFactory(key, factory);
        }

        public override void Load()
        {
            foreach (var factory in Factories)
            {
                var kvp = factory;
                Bind<ISession>()
                    .ToMethod(ctx => this.GetSession(kvp.Key))
                    .When(req => IsFactory(req, kvp.Key))
                    .InRequestScope();
            }
            Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));
        }

        protected static ISessionFactory ConfigureDefaultFactory()
        {
            return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection")))
                    .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<UserMap>())
                    .BuildConfiguration()
                    .CurrentSessionContext<WebSessionContext>().BuildSessionFactory();
        }

        protected ISession GetSession(string factory)
        {
            if(!Factories.ContainsKey(factory))
                throw new InvalidFactoryException(string.Format("Invalid factory \"{0}\" provided", factory));

            return Factories[factory].OpenSession();
        }

        private static bool IsFactory(IRequest request, string factoryName)
        {
            Type requestType = request.Target.Member.ReflectedType;
            var attrs = requestType.GetCustomAttributes(true);
            var factoryAttr = attrs.FirstOrDefault(a => a.GetType() == typeof (SessionFactory)) as SessionFactory ?? null;
            var factory = (factoryAttr == null) ? "Default" : factoryAttr.SessionFactoryName;
            return factory == factoryName;
        }
    }
}
