using System;
using System.Collections;

namespace MontfoortIT.Business.DataInterfaces
{
    
    /// <summary>
    /// The main interface that needs to be implemented
    /// </summary>
    public interface IDto
    {
        /// <summary>
        /// Gets the id of this instance
        /// </summary>
        int Id { get; set; }

        bool IsNew { get; }
    }
}