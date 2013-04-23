using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using NHibernate;
using NHibernate.Linq;

namespace Infrastructure.NHibernate.Repositories
{
    public class NHibernateRepository<T> : IRepository<T> where T : IEntity<T>
    {
        protected ISession Session;

        public NHibernateRepository(ISession session)
        {
            Session = session;
        }

        public virtual T Get(object id)
        {
            return Session.Get<T>(id);
        }

        public virtual T Load(object id)
        {
            return Session.Load<T>(id);
        }

        public virtual List<T> GetAll()
        {
            return Session.Query<T>().ToList();
        }

        public virtual List<T> FindBy(Func<T, bool> condition)
        {
            return Session.Query<T>()
                    .Where(condition)
                    .ToList();
        }

        public virtual T FindOneBy(Func<T, bool> condition)
        {
            return Session.Query<T>().FirstOrDefault(condition);
        }

        public virtual void Store(T entity)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public virtual void Delete(T entity)
        {
            using (ITransaction transaction = Session.BeginTransaction())
            {
                Session.Delete(entity);
                transaction.Commit();
            }
        }
    }
}
