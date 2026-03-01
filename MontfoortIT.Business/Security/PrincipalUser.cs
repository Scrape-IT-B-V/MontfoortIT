using System;
using System.Security;
using System.Security.Principal;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// Implementation of the user object
    /// </summary>
    public class PrincipalUser: UserBase
    {
        private int? _id;

        /// <summary>
        /// Not used for a principal user
        /// </summary>
        public override int Id
        {
            get
            {
                if(_id.HasValue)
                    return _id.Value;
                throw new NotImplementedException();
            }
        }

        public override string Email
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool IsApproved
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Guid GuidID
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not used for a principal user
        /// </summary>
        public override void Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the principal object that is used
        /// </summary>
        public IPrincipal Principal { get; private set; }

        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        public PrincipalUser(ISecurityConnection securityConnection, IPrincipal principal)
            : base(securityConnection, principal.Identity.Name)
        {
            if (principal == null) throw new ArgumentNullException("principal");
            if(!principal.Identity.IsAuthenticated)
                throw new SecurityException("User should be authenticated");

            Principal = principal;
        }

        ///<summary>
        /// Creates a principal user with an id
        ///</summary>
        ///<param name="principal"></param>
        ///<param name="id"></param>
        public PrincipalUser(ISecurityConnection securityConnection, IPrincipal principal, int id)
            :this(securityConnection, principal)
        {
            _id = id;
        }
    }
}
