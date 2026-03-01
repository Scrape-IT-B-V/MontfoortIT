using System;

namespace MontfoortIT.Business.Security
{
    /// <summary>
    /// The IUser interface
    /// </summary>
    public interface IUser
    {
        string UserName { get;  }

        string Email { get; set; }

        bool IsApproved { get; set; }

        /// <summary>
        /// Returns the id of this company
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Returns the guid id of this user
        /// </summary>
        Guid GuidID { get; }

    }
}
