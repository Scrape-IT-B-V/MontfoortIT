using System.Collections.Generic;

namespace MontfoortIT.Business.DataInterfaces.Security
{
    /// <summary>
    /// Describes an helper class for the user dto
    /// </summary>
    public interface IUserTable<T>:ITable<T>
        where T : IUserDto
    {
        

        /// <summary>
        /// Gets an user by username
        /// </summary>
        /// <param name="userName">The username to look for</param>
        /// <returns>null or a user</returns>
        T GetByUserName(string userName);
    }
}