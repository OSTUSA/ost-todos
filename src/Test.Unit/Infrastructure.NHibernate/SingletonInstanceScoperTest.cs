using System.Collections.Generic;
using Infrastructure.NHibernate;
using NUnit.Framework;

namespace Test.Unit.Infrastructure.NHibernate
{
    [TestFixture]
    public class SingletonInstanceScoperTest
    {
        protected SingletonInstanceScoper<string> Scoper { get; set; }

        [SetUp]
        public void Init()
        {
            Scoper = new SingletonInstanceScoper<string>();
        }

        [Test]
        public void GetDictionary_should_return_typed_dictionary()
        {
            var dict = Scoper.GetDictionary();

            Assert.IsInstanceOf<Dictionary<string, string>>(dict);
        }

        [Test]
        public void GetScopedInstance_should_accept_builder_func_and_add_result_to_dictionary()
        {
            Scoper.GetScopedInstance("mystringbuilder", () => "new string");

            var dict = Scoper.GetDictionary();

            Assert.AreEqual("new string", dict["mystringbuilder"]);
        }

        [Test]
        public void ClearInstance_should_clear_by_key()
        {
            Scoper.GetScopedInstance("anotherbuilder", () => "another string");
            var dict = Scoper.GetDictionary();
            Assert.AreEqual("another string", dict["anotherbuilder"]);
            Scoper.ClearInstance("anotherbuilder");
            Assert.False(dict.Contains("anotherbuilder"));
        }
    }
}
