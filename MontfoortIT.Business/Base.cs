using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MontfoortIT.Business.DataInterfaces;
using MontfoortIT.Business.Security;
using MontfoortIT.Business.Validation;

namespace MontfoortIT.Business
{
    /// <summary>
    /// The base class for business entities
    /// </summary>
    /// <typeparam name="TTable"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    public abstract class Base<TDto> : IBase, INotifyPropertyChanged, IDataErrorInfo
        where TDto : IDto
    {
        private TDto _dataObject;
        private bool _isValidated;
        private ISecurityConnection _securityConnection;
        private BusinessUser _currentUser;
        
        private Validator _validator;
       
        public ISecurityConnection SecurityConnection
        {
            get
            {
                return _securityConnection;
            }            
        }
        

        /// <summary>
        /// Constructor used for serialization
        /// </summary>
        protected internal Base(ISecurityConnection securityConnection, TDto dataObject)
        {
            _securityConnection = securityConnection;
            _dataObject = dataObject;
        }

        /// <summary>
        /// Returns a reference to the dataobject
        /// </summary>
        protected TDto DataObject
        {
            get
            {
                return _dataObject;
            }
            private set
            {
                _dataObject = value;
            }
                
        }

        /// <summary>
        /// If the current entity is validated
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                EnsureValidated();

                // this.OnPropertyChanged("IsValid");
                if (_validator == null)
                    return true;

                return _validator.IsValid;
            }
        }

        private void EnsureValidated()
        {
            if (!_isValidated)
                Validate();
        }


        /// <summary>
        /// Can be overriden to turn off this feature
        /// </summary>
        protected virtual bool OnlySaveWhenChanged
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Contains messages for validation
        /// </summary>
        protected internal virtual IEnumerable<MessageLine> ValidationMessages
        {
            get 
            {
                if (_validator == null)
                    return new List<MessageLine>();

                return _validator.ValidationMessages; 
            }
        }

        /// <summary>
        /// Returns the id of this company
        /// </summary>
        public virtual int Id
        {
            get
            {
                return DataObject.Id;
            }
        }

        /// <summary>
        /// If this is a new class
        /// </summary>
        public bool IsNew
        {
            get
            {
                return DataObject.IsNew;
            }
        }

        /// <summary>
        /// Used for save functionality
        /// </summary>
        /// <returns></returns>
        internal TDto GetDataObject()
        {
            return DataObject;
        }

        /// <summary>
        /// When a class needs to validate this function is called.
        /// 
        /// To mark a entity as not valid add a validation message with message level error
        /// </summary>
        /// <returns></returns>
        protected virtual void Validate()
        {
            ResetValidation();

            _validator.Validate();

            _isValidated = true;
        }

        /// <summary>
        /// Resets the validation object
        /// </summary>
        protected void ResetValidation()
        {
            if (_validator == null)
                _validator = new Validator(this);
        }

        /// <summary>
        /// Adds a validation message to the list of messages
        /// </summary>
        /// <param name="level"></param>
        /// <param name="propertyName"></param>
        /// <param name="message"></param>
        protected void AddValidationMessage(MessageLevel level, string propertyName, string message)
        {
            _validator.AddValidationMessage(level, propertyName, message);
        }
        
        /// <summary>
        /// Needs to be called if the entity is changed
        /// </summary>        
        protected void Changed()
        {
            _isValidated = false;
        }

        /// <summary>
        /// Compares entities on id or reference(when they are new)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (IsNew)
                return base.Equals(obj);

            if (GetType() != obj.GetType())
                return false;

            Base<TDto> typedObj = (Base<TDto>)obj;
            return Id == typedObj.Id;
        }

        ///<summary>
        ///Serves as a hash function for a particular type. 
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            if(IsNew)
                return base.GetHashCode();

            return Id.GetHashCode();
        }
        
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Warns the developer if this object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            _isValidated = false;

            VerifyPropertyName(propertyName);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            PropertyChangedCompleted(propertyName);
        }

        protected void OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> property)
        {
            var expression = property.Body as MemberExpression;
            var member = expression.Member;

            OnPropertyChanged(member.Name);
        }

        protected virtual void PropertyChangedCompleted(string propertyName)
        {
        }

        protected async Task<BusinessUser> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            _currentUser = await BusinessUser.GetCurrentUserAsync(_securityConnection);
            return _currentUser;
        }

        /////////////////////INotifyPropertyChanged items END\\\\\\\\\\\\\\\\\\\\\\\\


        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. 
        ///                 </param>
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                EnsureValidated();

                string[] messages;
                if (_validator == null)
                    messages = new string[0];
                else
                    messages = _validator.ValidationMessages.Where(w => w.PropertyName == columnName).Select(m=>m.Message).ToArray();
                return string.Join(",", messages);
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        string IDataErrorInfo.Error
        {
            get
            {
                EnsureValidated();

                string[] messages;
                
                if(_validator== null) // Some classes disable the validator
                    messages = new string[0];
                else
                    messages = _validator.ValidationMessages.Select(m => m.Message).ToArray();
                return string.Join(",", messages);
            }
        }

        void IBase.SetDataObject(object dto)
        {
            DataObject = (TDto)dto;
        }
    }
}