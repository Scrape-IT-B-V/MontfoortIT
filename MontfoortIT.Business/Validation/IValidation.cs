using System.Collections.ObjectModel;

namespace MontfoortIT.Business.Validation
{
    /// <summary>
    /// The interface used for validation
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// If the instance is valid
        /// </summary>
        bool IsValid { get;}

        /// <summary>
        /// Validation messages
        /// </summary>
        ReadOnlyCollection<MessageLine> ValidationMessages { get; }
    }
}
