using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DarkSoulsII.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbContext Context;
        internal DbSet<T> DbSet;

        public Repository()
        {
            Context = new DarkSoulsIIContext();
            DbSet = Context.Set<T>();
        }

        /// <summary>
        /// Overload to allow for DI
        /// </summary>
        /// <param name="context">The DbContext to use with this repository</param>
        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, params string[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">Linq expression used to filter the result set</param>
        /// <param name="orderBy">Linq function used to order the results</param>
        /// <param name="includeProperties">Parameter array of any navigation properties to eagerly load</param>
        /// <returns></returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeProperties)
        {
            IQueryable<T> query = GetQueryable(filter, includeProperties);
            query = orderBy != null ? orderBy(query) : query;
            List<T> result = query.ToList();
            return result;
        }

        /// <summary>
        /// Paged query allowing one filter in the form of a String
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="totalResults"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(string orderBy,
                                       out long totalResults,
                                       Expression<Func<T, bool>> filter = null,
                                       int page = 1,
                                       int pageSize = 10,
                                       params string[] includeProperties)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                ArgumentException ex = new ArgumentException("Order By is required for paged results.");
                throw ex;
            }

            if (page < 1)
            {
                page = 1;
            }

            IQueryable<T> query = DbSet;

            int skip = (page - 1) * pageSize;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            totalResults = query.Count();

            foreach (string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            query = query.OrderBy(orderBy).Skip(skip).Take(pageSize);

            List<T> result = query.ToList();

            return result;
        }

        /// <summary>
        /// Paged query allowing one filter
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="totalResults"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                       out long totalResults,
                                       Expression<Func<T, bool>> filter = null,
                                       int page = 1,
                                       int pageSize = 25,
                                       params string[] includeProperties)
        {
            if (orderBy == null)
            {
                ArgumentException ex = new ArgumentException("Order By is required for paged results.");
                throw ex;
            }

            if (page < 1)
            {
                page = 1;
            }

            IQueryable<T> query = DbSet;

            int skip = (page - 1) * pageSize;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            totalResults = query.Count();

            foreach (string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            query = orderBy(query).Skip(skip).Take(pageSize);

            List<T> result = query.ToList();

            return result;
        }

        /// <summary>
        /// Paged query allowing a list of filters 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="totalResults"></param>
        /// <param name="filters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPaged(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                       out long totalResults,
                                       List<Expression<Func<T, bool>>> filters,
                                       int page = 1,
                                       int pageSize = 25,
                                       params string[] includeProperties)
        {
            if (orderBy == null)
            {
                ArgumentException ex = new ArgumentException("Order By is required for paged results.");
                throw ex;
            }

            if (page < 1)
            {
                page = 1;
            }

            IQueryable<T> query = DbSet;

            int skip = (page - 1) * pageSize;

            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            totalResults = query.Count();

            foreach (string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            query = orderBy(query).Skip(skip).Take(pageSize);

            List<T> result = query.ToList();

            return result;
        }

        public T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            var entity = Context.Entry(entityToUpdate);
            entity.State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Not sure if this should be used, better to use the UnitOfWork ExecuteNonQuery/SqlCommand instead?
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int SqlCommand(string sqlStatement, params object[] parameters)
        {
            //((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreCommand(sqlStatement, parameters);
            return Context.Database.ExecuteSqlCommand(sqlStatement, parameters);
        }


    }
}