using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MontfoortIT.Business.Validation
{
    internal class CollectionValidator<T> : IValidation
    {
        private readonly IEnumerable<T> _collection;
        private bool? _isValid;
        private List<MessageLine> _validationMessages;

        public bool IsValid
        {
            get
            {
                if (_isValid == null)
                    _isValid = Validate();
                return _isValid.Value;
            }
        }

        public ReadOnlyCollection<MessageLine> ValidationMessages
        {
            get
            {
                if (_isValid == null)
                    _isValid = Validate();

                return _validationMessages.AsReadOnly();
            }
        }

        internal CollectionValidator(IEnumerable<T> collection)
        {
            _collection = collection;
            
        }

        /// <summary>
        /// The validate method
        /// </summary>
        /// <returns></returns>
        protected internal virtual bool Validate()
        {
            bool isValid = true;
            _validationMessages = new List<MessageLine>();

            foreach (T t in _collection)
            {
                Validator validator = new Validator(t);
                validator.Validate();

                if (!validator.IsValid)
                {
                    isValid = false;
                    AddValidationMessage(validator.ValidationMessages);
                }

            }

            return isValid;
        }

        private void AddValidationMessage(IEnumerable<MessageLine> messages)
        {
            _validationMessages.AddRange(messages);

            if(messages.Any(m=>m.Level == MessageLevel.Error))
                _isValid = false;
        }

        
    }
}
