using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using MontfoortIT.Business.Validation;

namespace MontfoortIT.Business.Validation
{

    /// <summary>
    /// Is thrown when saved but entity is not valid
    /// </summary>
    [global::System.Serializable]
    public class ValidationException : Exception
    {
        private IEnumerable<MessageLine> _validationMessages;
        /// <summary>
        /// Contains the validation messages
        /// </summary>
        public IEnumerable<MessageLine> ValidationMessages
        {
            get { return _validationMessages; }
        }
        
        internal ValidationException(string message, IEnumerable<MessageLine> validationMessages)
            :base(message)
        {
            _validationMessages = validationMessages;
        }

        internal ValidationException(string messageFormat, object[] args, IEnumerable<MessageLine> validationMessages)
            : this(string.Format(messageFormat, args), validationMessages)
        { }

        /// <summary>
        /// Contains the validation messages
        /// </summary>
        public override string Message
        {
            get
            {
                StringBuilder message = new StringBuilder();
                message.Append(base.Message);
                message.Append("\n\n");
                foreach (MessageLine val in ValidationMessages)
                {
                    message.AppendLine(val.ToString());                    
                }
                return message.ToString();
            }
        }

        
    }
}
