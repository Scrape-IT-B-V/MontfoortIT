using System;
using System.Threading.Tasks;
using MontfoortIT.Business.DataInterfaces.Security;
using MontfoortIT.Business.Validation;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// Implementation of the user object
    /// </summary>
    public class User<TUt,TDto> : Base<TDto>, IUser
        where TDto : IUserDto
    {
        /// <summary>
        /// The login Id
        /// </summary>
        public string UserName
        {
            get { return DataObject.UserName; }
            private set
            {
                DataObject.UserName = value;
                Changed();
            }
        }

        public string Email
        {
            get { return DataObject.Email; }
            set
            {
                DataObject.Email = value;
                Changed();
            }
        }

        /// <summary>
        /// Not used with a normal user
        /// </summary>
        public virtual bool IsApproved
        {
            get { return true; }
            set {  }
        }


        /// <summary>
        /// Removes the role of this user
        /// </summary>
        public Task RemoveRoleAsync(string roleName)
        {
            return SecurityConnection.RemoveRoleAsync(UserName, roleName);            
        }
        /// <summary>
        /// Adds a role to this user
        /// </summary>
        public Task AddRole(string roleName)
        {
            return SecurityConnection.AddRoleAsync(UserName, roleName);            
        }


        /// <summary>
        /// Constructor for a new user (also used for injection)
        /// </summary>
        protected User(ISecurityConnection securityConnection, TDto dataObject) :base(securityConnection,dataObject)
        { }

        /// <summary>
        /// Validates the user entity
        /// </summary>
        protected override void Validate()
        {
            base.Validate();
            if (string.IsNullOrEmpty(UserName))
            {
                AddValidationMessage(MessageLevel.Error, "UserName", Resources.ValidationMessages.User_UserNameRequired);
            }
        }

        public Guid GuidID
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns a representation of the user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserName;
        }      
        
    }
}
