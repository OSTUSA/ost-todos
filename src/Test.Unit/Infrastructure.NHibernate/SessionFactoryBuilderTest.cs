using Infrastructure.NHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace Test.Unit.Infrastructure.NHibernate
{
    [TestFixture]
    public class SessionFactoryBuilderTest
    {
        protected SessionFactoryBuilder Builder;

        [SetUp]
        public void Init()
        {
            Builder = new SessionFactoryBuilder();
        }

        [Test]
        public void GetFactory_with_key_should_return_factory_by_function()
        {
            var factory = Builder.GetFactory("factory", BuildFactory);
            Assert.IsInstanceOf<ISessionFactory>(factory);
        }

        [Test]
        public void GetFactory_with_key_only_should_return_factory_if_already_called()
        {
            var factory = Builder.GetFactory("factory", BuildFactory);
            var singleCopy = Builder.GetFactory("factory");
            Assert.AreSame(factory, singleCopy);
        }

        [Test]
        public void GetFactory_with_key_only_returns_null_if_key_does_not_exist()
        {
            var nullFactory = Builder.GetFactory("nope");
            Assert.IsNull(nullFactory);
        }

        protected static ISessionFactory BuildFactory()
        {
            return new Mock<ISessionFactory>().Object;
        }
    }
}
