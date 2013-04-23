using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model.Users;
using NHibernate.Linq;
using NHibernate.Proxy;
using NUnit.Framework;

namespace Test.Integration.Infrastructure.NHibernate.Repositories.NHibernateRepositoryTest
{
    [TestFixture]
    public class NHibernateRepositoryTest : RepositoryTestBase<User>
    {
        [Test]
        public void Store_should_persist_new_User()
        {
            var user = Mother.SimpleUser;
            Repo.Store(user);
            var fetched = Session.Query<User>().FirstOrDefault(u => u.Email == "b@s.com");
            Assert.True(fetched.Id > 0);
            Assert.AreEqual("b@s.com", fetched.Email);
            Assert.AreEqual("brian", fetched.Name);
            Assert.AreEqual(user.Password, fetched.Password);
        }

        [Test]
        public void Store_should_update_existing_User()
        {
            var user = Mother.SimpleUser;
            Repo.Store(user);
            Session.Clear();
            var fetched = Session.Get<User>(user.Id);
            fetched.Name = "Bryan";
            Repo.Store(fetched);
            Session.Clear();
            var updated = Session.Get<User>(fetched.Id);
            Assert.AreEqual("Bryan", updated.Name);
        }

        [Test]
        public void FindBy_should_return_list_of_matching_criteria()
        {
            using (var trans = Session.BeginTransaction())
            {
                Session.Save(Mother.SimpleUser);
                Session.Save(Mother.AnotherSimpleUser);
                trans.Commit();
            }
            Session.Clear();
            var found = Repo.FindBy(x => x.Name == "brian");
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("brian", found[0].Name);
        }

        [Test]
        public void Get_should_return_user_by_id()
        {
            var user = Mother.SimpleUser;
            using (var trans = Session.BeginTransaction())
            {
                Session.Save(user);
                trans.Commit();
            }
            Session.Clear();
            var fetched = Repo.Get(user.Id);
            Assert.IsInstanceOf<User>(fetched);
        }

        [Test]
        public void Get_should_return_null_if_user_not_found()
        {
            var fetched = Repo.Get((long)-1);
            Assert.IsNull(fetched);
        }

        [Test]
        public void FindBy_should_return_empty_list_if_no_items_found()
        {
            var fetched = Repo.FindBy(x => x.Email == "doesntexist@email.com");
            CollectionAssert.IsEmpty(fetched);
        }

        [Test]
        public void GetAll_should_return_empty_list_if_no_records()
        {
            var fetched = Repo.GetAll();
            CollectionAssert.IsEmpty(fetched);
        }

        [Test]
        public void GetAll_should_return_list_of_all_records()
        {
            using (var trans = Session.BeginTransaction())
            {
                Session.Save(Mother.SimpleUser);
                Session.Save(Mother.AnotherSimpleUser);
                trans.Commit();
            }
            Session.Clear();
            var all = Repo.GetAll();
            Assert.AreEqual(2, all.Count);
            Assert.AreEqual("brian", all[0].Name);
            Assert.AreEqual("scaturro", all[1].Name);
        }

        [Test]
        public void Load_should_return_a_proxy()
        {
            using (var trans = Session.BeginTransaction())
            {
                Session.Save(Mother.SimpleUser);
                trans.Commit();
            }
            Session.Clear();
            var loaded = Repo.Load(Mother.SimpleUser.Id);
            Assert.IsInstanceOf<INHibernateProxy>(loaded);
        }

        [Test]
        public void Delete_should_delete_entity()
        {
            using (var trans = Session.BeginTransaction())
            {
                Session.Save(Mother.SimpleUser);
                Session.Save(Mother.AnotherSimpleUser);
                trans.Commit();
            }
            Session.Clear();
            var brian = Session.Get<User>((long)1);
            Repo.Delete(brian);
            var all = Session.Query<User>().ToList();
            Assert.AreEqual(1, all.Count);
        }
    }
}