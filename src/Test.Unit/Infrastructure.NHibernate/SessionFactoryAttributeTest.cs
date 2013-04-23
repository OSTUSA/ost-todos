using Infrastructure.NHibernate;
using NUnit.Framework;

namespace Test.Unit.Infrastructure.NHibernate
{
    [TestFixture]
    public class SessionFactoryAttributeTest
    {
        [Test]
        public void Constructor_should_set_factory_name()
        {
            var attr = new SessionFactory("MyFactory");
            Assert.AreEqual("MyFactory", attr.SessionFactoryName);
        }
    }
}
