using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MontfoortIT.Business.Validation
{
    /// <summary>
    /// The validator 
    /// </summary>
    public class Validator
    {
        private readonly object _instance;
        private readonly List<MessageLine> _messages;
        private bool _isValid;

        public Validator(object instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _instance = instance;
            _messages = new List<MessageLine>();
        }
        /// <summary>
        /// If the current instance is valid
        /// </summary>
        public bool IsValid
        {
            get {
                return _isValid;
            }
        }

        /// <summary>
        ///  The validation messages that are generated
        /// </summary>
        public ReadOnlyCollection<MessageLine> ValidationMessages
        {
            get {
                return _messages.AsReadOnly();
            }
        }

        internal void Validate()
        {
            _messages.Clear();
            _isValid = true;

            Type type = _instance.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                //Validator validator = ValidationFactory.CreateValidator(GetType());
                //ValidationResults results = new ValidationResults();
                //validator.Validate(this, results);

                // Find only the ValidationAttribute for this property. If you leave off the
                // "typeof(ValidationAttribute)" you would get all custom attributes on this property.
                // Also note the "as ValidationAttribute[]"
                ValidationAttribute[] attribs = property.GetCustomAttributes(
                                                    typeof(ValidationAttribute), true) as ValidationAttribute[];

                // Iterate over attributes and evaluate each DataAnnotation.
                // Note I stop once I find the first failure. You can change this to build
                // a list of all failures in validation for a property.
                bool valueFetched = false;
                object value = null;
                for (int i = 0; i < attribs.Length; i++)
                {
                    ValidationAttribute attrib = attribs[i];
                    
                    if (!valueFetched)
                    {
                        value = property.GetValue(_instance, null);
                        valueFetched = true;
                    }

                    if (!attrib.IsValid(value))
                    {
                        // You can use the ErrorMessage param of the Required, StringLength etc
                        // attribute if you have a very specific error message you want shown
                        // but take a look at not adding it and letting the built in function
                        // FormatErrorMessage do it.
                        AddValidationMessage(MessageLevel.Error, property.Name, attribs[i].FormatErrorMessage(property.Name));
                    }
                }

            }
        }

        /// <summary>
        /// Adds a validation message to this instance
        /// </summary>
        /// <param name="level"></param>
        /// <param name="propertyName"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        protected internal void AddValidationMessage(MessageLevel level, string propertyName, string messageFormat, params object[] args)
        {
            string message;
            if (args == null || args.Length == 0)
                message = messageFormat;
            else 
                message = string.Format(messageFormat, args);

            _messages.Add(new MessageLine(level, propertyName, message));
            
            if (level == MessageLevel.Error)
                _isValid = false;
        }

        /// <summary>
        /// Adds a list of validation messages
        /// </summary>
        /// <param name="messages"></param>
        protected void AddValidationMessages(List<MessageLine> messages)
        {
            if (_isValid)
            {
                // Check if there is an error line within the collection, then this entity needs to be marked as invalid
                MessageLine errorLine = messages.Find(l => l.Level == MessageLevel.Error);
                if (errorLine != null)
                    _isValid = false;
            }

            _messages.AddRange(messages);
        }
    }
}
