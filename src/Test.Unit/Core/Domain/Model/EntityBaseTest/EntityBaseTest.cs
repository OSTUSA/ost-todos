using System.Reflection;
using NUnit.Framework;

namespace Test.Unit.Core.Domain.Model.EntityBaseTest
{
    [TestFixture]
    public class EntityBaseTest
    {
        [Test]
        public void IsNew_should_return_true_if_Id_not_set()
        {
            var entity = new TestEntity();
            Assert.IsTrue(entity.IsNew());
        }

        [Test]
        public void IsNew_should_return_false_if_Id_set()
        {
            var entity = new TestEntity();
            SetEntityId(entity, 1);
            Assert.IsFalse(entity.IsNew());
        }

        [Test]
        public void Equals_should_return_true_if_entity_is_new_and_compared_to_self()
        {
            var entity = new TestEntity();
            Assert.True(entity.Equals(entity));
        }

        [Test]
        public void Equals_should_return_false_if_both_entities_are_new()
        {
            var entity = new TestEntity();
            var entity2 = new TestEntity();
            Assert.False(entity.Equals(entity2));
        }

        [Test]
        public void Equals_should_return_true_if_both_entities_have_same_Id()
        {
            var entity = new TestEntity();
            var entity2 = new TestEntity();
            SetEntityId(entity, 1);
            SetEntityId(entity2, 1);
            Assert.True(entity.Equals(entity2));
        }

        [Test]
        public void Equals_should_return_false_if_entities_have_different_Id()
        {
            var entity = new TestEntity();
            var entity2 = new TestEntity();
            SetEntityId(entity, 1);
            SetEntityId(entity2, 2);
            Assert.False(entity.Equals(entity2));
        }

        [Test]
        public void Equals_should_return_false_if_other_entity_is_null()
        {
            var entity = new TestEntity();
            Assert.False(entity.Equals(null));
        }

        [Test]
        public void GetHashCode_returns_hash_code_of_Id()
        {
            var entity = new TestEntity();
            long id = 2;
            SetEntityId(entity, id);
            Assert.AreEqual(id.GetHashCode(), entity.GetHashCode());
        }

        [Test]
        public void GetHashCode_uses_hash_code_that_was_generated_pre_id_set()
        {
            var entity = new TestEntity();
            var hash = entity.GetHashCode();
            SetEntityId(entity, 1);
            Assert.AreEqual(hash, entity.GetHashCode());
        }

        private static void SetEntityId(TestEntity entity, long id)
        {
            var idProperty = entity.GetType()
                                   .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
            var setter = idProperty.SetMethod;
            setter.Invoke(entity, new object[] { id });
        }
    }
}
