using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }
        protected RepositoryContext2 RepositoryContext2 { get; set; }
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
          
        }
        public RepositoryBase(RepositoryContext2 repositoryContext2)
        {
            RepositoryContext2 = repositoryContext2;

        }

        public IQueryable<T> FindAll()
        {
            return RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public int Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
            RepositoryContext.SaveChanges();
            return 200;
        }
        public int Create2(T entity)
        {
            RepositoryContext2.Set<T>().Add(entity);
            RepositoryContext2.SaveChanges();
            return 200;
        }
        public int Update(T entity)
        {

            RepositoryContext.Set<T>().Update(entity).State = EntityState.Modified;
            RepositoryContext.SaveChanges();
            return 200;
        }

        public int Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
            RepositoryContext.SaveChanges();
            return 200;
        }
    }
}
