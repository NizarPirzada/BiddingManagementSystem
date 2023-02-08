using System;
using System.Linq;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        int Create(T entity);
        int Create2(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
