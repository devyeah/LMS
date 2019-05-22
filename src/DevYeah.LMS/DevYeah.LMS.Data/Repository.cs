using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevYeah.LMS.Data.Interfaces;
using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace DevYeah.LMS.Data
{
    public abstract class Repository<T> : IRepository<T>
        where T : Model
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public T Get(Guid key) => _dbContext.Set<T>().Where(p => p.Id == key).Single();
        
        public void Add(T entity) => _dbContext.Add<T>(entity);
        
        public T Find(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).FirstOrDefault();
        
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression);
        
        public void Update(T entity) => _dbContext.Update<T>(entity);
        
        public void Delete(T entity) => _dbContext.Remove<T>(entity);

        public void SaveChanges() => _dbContext.SaveChanges();
    }
}
