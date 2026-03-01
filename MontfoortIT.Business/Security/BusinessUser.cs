using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// Business logic for a user
    /// </summary>
    public class BusinessUser: IUser
    {
        
        private readonly IUser _user;
        private readonly RoleCollection _roles;

        public string UserName
        {
            get { return _user.UserName; }
        }

        public int Id
        {
            get { return _user.Id; }
        }

        public string Email
        {
            get { return _user.Email; }
            set { _user.Email = value; }
        }

        public bool IsApproved
        {
            get { return _user.IsApproved; }
            set { _user.IsApproved = value; }
        }

        /// <summary>
        /// Gets the original user without the business user wrapper
        /// </summary>
        public IUser UserIdentity
        {
            get
            {
                return _user;
            }
        }


        public Guid GuidID
        {
            get { return _user.GuidID; }
        }

        public RoleCollection Roles { get { return _roles; } }


        /// <summary>
        /// Constructor for a business user that helps with extra functionality
        /// </summary>
        /// <param name="securityConnection"></param>
        /// <param name="user"></param>
        public BusinessUser(IUser user, IEnumerable<string> roles)
        {
            _user = user;
            _roles = new RoleCollection(user, roles);
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns></returns>
        public static async Task<BusinessUser> GetCurrentUserAsync(ISecurityConnection securityConnection)
        {
            IUser user = await securityConnection.GetCurrentUserAsync();
            
            if (user == null)
                return null;

            var roles = await securityConnection.GetRolesForUserAsync(user.UserName);
            
            return new BusinessUser(user, roles);
        }
        
    }
}
