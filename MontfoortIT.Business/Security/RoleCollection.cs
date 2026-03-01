using System;
using System.Collections.Generic;
using System.Security;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// Contains functions that can be executed on the roles
    /// </summary>
    public class RoleCollection:IEnumerable<string>
    {
        private readonly List<string> _roles;

        /// <summary>
        /// Returns the user where this role is for
        /// </summary>
        public IUser User { get; private set; }

        /// <summary>
        /// Creates a new role collection with specific roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        public RoleCollection(IUser user, IEnumerable<string> roles)
        {
            if(roles == null)
                _roles = new List<string>();
            else
                _roles = new List<string>(roles);

            User = user;
        }

        ///<summary>
        /// Demands a specific role
        ///</summary>
        ///<param name="role"></param>
        ///<exception cref="SecurityException"></exception>
        public void Demand(string role)
        {
            if (role == null) throw new ArgumentNullException("role");

            if(!Contains(role))
                throw new SecurityException(
                    string.Format("Current user must be an '{0}' to execute this functioanlity", role));
        }

        /// <summary>
        /// If the current roles collection contains te specific role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool Contains(string role)
        {
            if (role == null) throw new ArgumentNullException("role");
            return _roles.Contains(role);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _roles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _roles.GetEnumerator();
        }
    }
}
