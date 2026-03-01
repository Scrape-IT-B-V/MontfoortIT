using System;

namespace MontfoortIT.Business.Security
{
    ///<summary>
    /// The base class for a default user
    ///</summary>
    public abstract class UserBase : IUser
    {
        private readonly ISecurityConnection _securityConnection;
        private readonly string _userName;
        
        /// <summary>
        /// The login Id
        /// </summary>
        public string UserName
        {
            get { return _userName; }
        }


        public abstract int Id { get; }

        public abstract Guid GuidID { get; }

        public abstract string Email { get; set; }

        public abstract bool IsApproved { get; set; }

        public abstract void Save();

        /// <summary>
        /// Returns a representation of the user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserName;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="securityConnection"></param>
        /// <param name="userName"></param>
        protected UserBase(ISecurityConnection securityConnection, string userName)
        {
            if (securityConnection == null) throw new ArgumentNullException("securityConnection");

            _securityConnection = securityConnection;
            _userName = userName;
        }
    }
}
