namespace MontfoortIT.Business.DataInterfaces.Security
{
    /// <summary>
    /// The data transfer object for user
    /// </summary>
    public interface IUserDto: IDto
    {
        /// <summary>
        /// If this entity is deleted
        /// </summary>
        bool Deleted { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        string Email {get;set;}
    }
}