using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MontfoortIT.Business.DataInterfaces;
using MontfoortIT.Business.Validation;

namespace MontfoortIT.Business
{
    /// <summary>
    /// Contains base functionality for bussiness collections
    /// </summary>
    /// <typeparam name="TBase">The type that the collection stores</typeparam>
    /// <typeparam name="TTable">The data table class that contains base functionality</typeparam>
    /// <typeparam name="TDto">The data definition for the type within the collection</typeparam>
    public abstract class CollectionBase<TBase, TDto> : IList<TBase>, IValidation
        where TBase:Base<TDto>
        where TDto : IDto
    {
        private IList<TDto> _dataObjects; //dataobjects are leading
        private IList<TBase> _businessObjects = null;
        private IRepository<TBase,TDto> _repository;
        private CollectionValidator<TBase> _validator;

        public CollectionBase(IRepository<TBase,TDto> repository, IEnumerable<TDto> dataObjects)
        {
            if (dataObjects == null)
                throw new ArgumentNullException("dataObjects");

            _dataObjects = dataObjects.ToList();
            _repository = repository;
        }

        public CollectionBase(IRepository<TBase, TDto> repository, IEnumerable<TBase> businessObjects)
        {
            if(businessObjects == null)
                throw new ArgumentNullException("businessObjects");

            _businessObjects = businessObjects.ToList();
            _dataObjects = _businessObjects.Select(b => b.GetDataObject()).ToList();
            _repository = repository;
        }

        public TBase this[int index]
        {
            get
            {
                EnsureBusinessObjects();

                return _businessObjects[index];
            }
            set {
                _dataObjects[index] = value.GetDataObject();
                if (_businessObjects != null)
                    _businessObjects[index] = value;
            }
        }

        public int Count => _dataObjects.Count;

        public bool IsReadOnly => false;


        private CollectionValidator<TBase> Validator
        {
            get
            {
                if (_validator == null)
                    _validator = new CollectionValidator<TBase>(this);
                return _validator;
            }
        }

        public bool IsValid => Validator.IsValid;

        public ReadOnlyCollection<MessageLine> ValidationMessages => Validator.ValidationMessages;

        public void Add(TBase item)
        {
            _dataObjects.Add(item.GetDataObject());
            if (_businessObjects != null)
                _businessObjects.Add(item);
        }

        public void Clear()
        {
            _dataObjects.Clear();
            _businessObjects = null;
        }

        public bool Contains(TBase item)
        {
            if (_businessObjects != null)
                return _businessObjects.Contains(item);

            return _dataObjects.Contains(item.GetDataObject());
        }

        public void CopyTo(TBase[] array, int arrayIndex)
        {
            EnsureBusinessObjects();

            for (int i = 0; i < _dataObjects.Count; i++)
            {
                array[arrayIndex + i] = _businessObjects[i];                
            }
        }

        public IEnumerator<TBase> GetEnumerator()
        {
            EnsureBusinessObjects();

            return _businessObjects.GetEnumerator();
        }

        private readonly object _businessLock = new object();

        private void EnsureBusinessObjects()
        {
            if (_businessObjects == null)
            {
                lock (_businessLock)
                {
                    if (_businessObjects == null)
                        _businessObjects = _dataObjects.Select(d => _repository.ConstructBusinessEntity(d)).ToList();
                }
            }
        }

        public int IndexOf(TBase item)
        {
            if (_businessObjects != null)
                return _businessObjects.IndexOf(item);

            return _dataObjects.IndexOf(item.GetDataObject());
        }

        public void Insert(int index, TBase item)
        {
            _dataObjects.Insert(index, item.GetDataObject());
            if (_businessObjects != null)
                _businessObjects.Insert(index, item);
        }

        public bool Remove(TBase item)
        {
            bool success = _dataObjects.Remove(item.GetDataObject());
            if (success)
                _businessObjects = null;
            return success;
        }

        public void RemoveAt(int index)
        {
            _dataObjects.RemoveAt(index);
            if (_businessObjects != null)
                _businessObjects.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
