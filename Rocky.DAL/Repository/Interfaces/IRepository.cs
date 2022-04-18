using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rocky.DAL.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includProps = null, bool isTracking = true);

        T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includProps = null, bool isTracking = true);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

        void Save();
    }
}