using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Push the changes being tracked by the DbContext to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Check to see if any of the models being tracked by the DbContext have validation errors
        /// based on the validation attributes that are decorating the class and properties
        /// </summary>
        /// <returns>True if there are validation errors.  False otherwise</returns>
        bool HasValidationErrors();

        /// <summary>
        /// Get the repository that maps to the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The repository that handles this entity</returns>
        IRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Executes the passed TSQL command or sproc and returns the results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="args">The parameters.</param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string query, params object[] args);

        /// <summary>
        /// Executes the passed TSQL command with no result set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="args">The parameters.</param>
        /// <returns></returns>
        void SqlCommand(string command, params object[] args);
    }
}
