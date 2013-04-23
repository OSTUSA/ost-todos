using System;
using System.Collections.Generic;

namespace Core.Domain.Model
{
    public interface IRepository<T> where T : IEntity<T>
    {
        T Get(object id);

        T Load(object id);

        List<T> GetAll();

        List<T> FindBy(Func<T, bool> predicate);

        T FindOneBy(Func<T, bool> predicate);

        void Store(T entity);

        void Delete(T entity);
    }
}
