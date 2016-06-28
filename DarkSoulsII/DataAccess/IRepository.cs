using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DarkSoulsII.DataAccess
{
    public interface IRepository<T> where T : class
    {
        void Delete(object id);
        void Delete(T entityToDelete);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeProperties);
        T GetById(object id);
        IEnumerable<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, out long totalResults, Expression<Func<T, bool>> filter = null, int page = 1, int pageSize = 25, params string[] includeProperties);
        IEnumerable<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, out long totalResults, List<Expression<Func<T, bool>>> filters, int page = 1, int pageSize = 25, params string[] includeProperties);
        IEnumerable<T> GetPaged(string orderBy, out long totalResults, Expression<Func<T, bool>> filter = null, int page = 1, int pageSize = 25, params string[] includeProperties);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, params string[] includeProperties);
        void Insert(T entity);
        int SqlCommand(string sqlStatement, params object[] parameters);
        void Update(T entityToUpdate);
    }
}
