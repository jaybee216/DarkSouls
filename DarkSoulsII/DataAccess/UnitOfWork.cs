using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DarkSoulsII.DataAccess
{
    public class UnitOfWork:IUnitOfWork
    {
        private bool _disposed;
        private readonly DbContext _context;
        private readonly IDictionary<string, object> _repositoryDict;

        public UnitOfWork()
        {
            _context = new DarkSoulsIIContext();
            _repositoryDict = new Dictionary<string, object>();
        }

        /// <summary>
        /// Overload to allow for DI
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositoryDict = new Dictionary<string, object>();
        }

        public DbContext Context
        {
            get { return _context; }
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositoryDict.ContainsKey(GetShortTypeString<T>()))
            {
                var repo = new Repository<T>(_context);
                _repositoryDict.Add(GetShortTypeString<T>(), repo);
            }
            return _repositoryDict[GetShortTypeString<T>()] as Repository<T>;
        }

        public IEnumerable<T> SqlQuery<T>(string query, params object[] args)
        {
            return ((IObjectContextAdapter)_context).ObjectContext.ExecuteStoreQuery<T>(query, args);
        }

        public void SqlCommand(string command, params object[] args)
        {
            ((IObjectContextAdapter)_context).ObjectContext.ExecuteStoreCommand(command, args);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool HasValidationErrors()
        {
            return _context.GetValidationErrors().Any();
        }

        /// <summary>
        /// Gets the type and assembly for this Model.  Strips out any version info for compatability issues.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetShortTypeString<T>() where T : class
        {
            var type = typeof(T);

            return type.FullName + ", " + type.Assembly.FullName;
        }

        #region Implementation of IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}