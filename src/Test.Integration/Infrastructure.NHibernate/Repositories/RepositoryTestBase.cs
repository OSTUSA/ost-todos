using Core.Domain.Model;
using Infrastructure.NHibernate.Mapping.Users;
using Infrastructure.NHibernate.Repositories;
using NHibernate;
using NUnit.Framework;

namespace Test.Integration.Infrastructure.NHibernate.Repositories
{
    abstract public class RepositoryTestBase<T> where T : IEntity<T>
    {
        protected IRepository<T> Repo;

        protected ISession Session;

        protected DatabaseTestState TestState
        {
            get
            {
                return new DatabaseTestState("TestConnection");
            }
        }

        [SetUp]
        public void Init()
        {
            Session = TestState.Configure<UserMap>().OpenSession();
            Session.FlushMode = FlushMode.Commit;
            Repo = new NHibernateRepository<T>(Session);
        }

        [TearDown]
        public void Cleanup()
        {
            AfterTest();
            Session.Dispose();
        }

        public virtual void AfterTest()
        {
            
        }
    }
}
