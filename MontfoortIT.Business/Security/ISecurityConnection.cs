using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// Security connection interface
    /// </summary>
    public interface ISecurityConnection
    {        
        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        Task<IUser> GetCurrentUserAsync();

        /// <summary>
        /// Gets a user by the username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IUser> GetUserAsync(string userName);

        /// <summary>
        /// Gets a user by the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IUser> GetUserAsync(int id);

        /// <summary>
        /// Gets a user by the guidId
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IUser> GetUserAsync(Guid guidId);

        /// <summary>
        /// Adds a user by username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IUser> AddUserAsync(string userName, string password);

        /// <summary>
        /// Adds a user by username and password and email
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IUser> AddUserAsync(string userName, string password, string email);

        /// <summary>
        /// Removes a user by username 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns true if worked</returns>
        Task<bool> RemoveUserAsync(string userName);

        /// <summary>
        /// Gets a list of role names for the current user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetRolesForUserAsync(string userName);

        /// <summary>
        /// Removes the role from a user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        Task RemoveRoleAsync(string userName, string roleName);

        /// <summary>
        /// Adds the role from a user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        Task AddRoleAsync(string userName, string roleName);

        /// <summary>
        /// If the current user has the specific role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> IsUserInRoleAsync(string role);

        /// <summary>
        /// If the current user has the specific role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> IsUserInRoleAsync(string username, string role);

        /// <summary>
        /// Get roles for the current user
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetRolesForUserAsync();

        /// <summary>
        /// Gets a list of users for a specific role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetUsersInRoleAsync(string role);

        /// <summary>
        /// Resets the password for the user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>New generated password</returns>
        Task<string> ResetPasswordAsync(string username);

        /// <summary>
        /// Resets the password to a new specified password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <returns>If the reset password succeeeded</returns>
        Task<bool> ResetPasswordAsync(string userName, string newPassword);

    }
}
