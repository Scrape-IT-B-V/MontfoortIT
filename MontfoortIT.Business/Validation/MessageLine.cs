using System;
using System.Collections.Generic;
using System.Text;

namespace MontfoortIT.Business.Validation
{
    /// <summary>
    /// Contains validation messages
    /// </summary>
    [ Serializable ]
    public class MessageLine
    {
        private MessageLevel _level;
        
        /// <summary>
        /// Gets the level of the message
        /// </summary>
        public MessageLevel Level
        {
            get { return _level; }
        }

        private string _message;
        private string _propertyName;

        /// <summary>
        /// Returns the message
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// The property name that raised the exception
        /// </summary>
        public string PropertyName
        {
            get {
                return _propertyName;
            }
            private set {
                _propertyName = value;
            }
        }


        /// <summary>
        /// Creates a new message line
        /// </summary>
        /// <param name="level">The level of the message</param>
        /// <param name="propertyName"></param>
        /// <param name="message">The message</param>
        public MessageLine(MessageLevel level, string propertyName, string message)
        {
            _level = level;
            _message = message;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Constructs a new message line with a format
        /// </summary>
        /// <param name="level"></param>
        /// <param name="propertyName"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public MessageLine(MessageLevel level, string propertyName, string messageFormat, params object[] args)
            :this(level, propertyName, string.Format(messageFormat, args))
        {}

        /// <summary>
        /// Converts a message to a normal string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Level.ToString());
            str.Append("\t\t");
            str.Append(Message);
            return str.ToString();
        }
    }
}
