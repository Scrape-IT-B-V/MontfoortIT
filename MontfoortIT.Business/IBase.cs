namespace MontfoortIT.Business
{
    /// <summary>
    /// The base interface for business classes
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// Returns the id of this company
        /// </summary>
        int Id { get; }
        bool IsNew { get;  }

        /// <summary>
        /// Is used for initialization of an existing dto
        /// </summary>
        /// <param name="dto"></param>
        void SetDataObject(object dto);
    }
}