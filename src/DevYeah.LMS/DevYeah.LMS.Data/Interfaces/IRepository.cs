using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevYeah.LMS.Models;

namespace DevYeah.LMS.Data.Interfaces
{
    public interface IRepository<T> where T : Model
    {
        T Get(Guid key);
        void Add(T entity);
        T Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
