using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace MontfoortIT.Business.DataInterfaces
{
    /// <summary>
    /// Classes that hold functionality like gets
    /// </summary>
    /// <typeparam name="T">The types where a helper works on</typeparam>
    public interface ITable<T>
        where T:IDto
    {

        IAsyncEnumerable<T> GetAllAsync();

        /// <summary>
        /// Gets the total count
        /// </summary>
        /// <returns></returns>
        Task<int> GetAllCountAsync();

        ///<summary>
        /// Gets all items out of the table
        ///</summary>
        ///<returns></returns>
        IAsyncEnumerable<T> GetAllAsync(int startRowIndex, int maximumRows);

        ///<summary>
        /// Gets all items out of the table
        ///</summary>
        ///<returns></returns>
        IAsyncEnumerable<T> GetAllAsync(int startRowIndex, int maximumRows, string sortExpression);

        /// <summary>
        /// Gets an entity by id
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int pId);
        

        /// <summary>
        /// Saves or adds a new entity
        /// </summary>
        /// <param name="dataObject"></param>
        Task SaveAsync(T dataObject);

        /// <summary>
        /// Deletes an item
        /// </summary>
        ///<param name="dataObject">The object that needs to be deleted</param>
        Task DeleteAsync(T dataObject);

    }
}