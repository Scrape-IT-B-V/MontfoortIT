using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MontfoortIT.Business.DataInterfaces;
using MontfoortIT.Business.Security;
using System.Collections;
using System.Threading.Tasks;

namespace MontfoortIT.Business
{
    /// <summary>
    /// The repository contains functions for fetching data from the database
    /// </summary>
    /// <typeparam name="TBase">The type that the collection stores</typeparam>
    /// <typeparam name="TTable">The data table class that contains base functionality</typeparam>
    /// <typeparam name="TDto">The data definition for the type within the collection</typeparam>
    public class RepositoryBase<TBase, TTable, TDto> : IRepository<TBase, TDto>
        where TTable : ITable<TDto>
        where TDto : IDto
        where TBase : Base<TDto>
    {
        private TTable _table;
        private IFactory<TBase, TDto> _factory;
        private BusinessUser _currentUser;

        /// <summary>
        /// The helper to use
        /// </summary>        
        public TTable Table
        {
            get { return _table; }            
        }

        public ISecurityConnection SecurityConnection { get; set; }

        
        /// <summary>
        /// The default constructor
        /// </summary>
        public RepositoryBase(ISecurityConnection securityConnection, TTable table, IFactory<TBase, TDto> factory)
        {
            SecurityConnection = securityConnection;
            _table = table;
            _factory = factory;
            _factory.Repository = this;
        }
        
        /// <summary>
        /// Gets an object by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected async Task<TBase> GetByIdBaseAsync(int id)
        {
            TDto dataObject = await Table.GetByIdAsync(id);
            if (dataObject == null)
                return null;

            return ConstructBusinessEntity(dataObject);
        }

        public Task<TBase> ConstructBusinessEntityAsync(Task<TDto> dataObjectTask)
        {
            return dataObjectTask.ContinueWith(d => ConstructBusinessEntity(d.Result));
        }

        /// <summary>
        /// Constructs a new TBase for an existing dataObject
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public TBase ConstructBusinessEntity(TDto dataObject)
        {
            if (dataObject == null)
                return null;

            TBase item;
            item = _factory.CreateExisting(dataObject);           

            return item;
        }


        public TBase ConstructBusinessEntity<TDto1>(TDto1 entity)
        {
            throw new NotImplementedException();
        }


        protected async Task<BusinessUser> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            _currentUser = await BusinessUser.GetCurrentUserAsync(SecurityConnection);
            return _currentUser;
        }

        /// <summary>
        /// Constructs a business collection based on data items
        /// </summary>
        /// <typeparam name="TCollection"></typeparam>
        /// <param name="dataItems"></param>
        /// <returns></returns>
        public TCollection ConstructBusinessCollection<TCollection>(IEnumerable<TDto> dataItems)
            where TCollection : CollectionBase<TBase, TDto>
        {
            return ConstructBusinessCollectionIntern<TCollection>(dataItems);
        }

        public Task<TCollection> ConstructBusinessCollectionAsync<TCollection>(Task<IEnumerable<TDto>> dataItemsTask)
            where TCollection : CollectionBase<TBase, TDto>
        {
            return ConstructBusinessCollectionAsync<TCollection>(dataItemsTask.ContinueWith(res => res.Result.ToList()));
        }

        public async Task<TCollection> ConstructBusinessCollectionAsync<TCollection>(IAsyncEnumerable<TDto> dataItemsTask)
            where TCollection : CollectionBase<TBase, TDto>
        {
            List<TDto> dataItems = new List<TDto>();
            await foreach (var dataItem in dataItemsTask)
            {
                dataItems.Add(dataItem);
            }

            return ConstructBusinessCollection<TCollection>(dataItems);
        }

        public Task<TCollection> ConstructBusinessCollectionAsync<TCollection>(Task<List<TDto>> dataItemsTask)
            where TCollection : CollectionBase<TBase, TDto>
        {
            return dataItemsTask.ContinueWith(result => ConstructBusinessCollection<TCollection>(result.Result));
        }

        private TCollection ConstructBusinessCollectionIntern<TCollection>(IEnumerable<TDto> dataItems) where TCollection : CollectionBase<TBase, TDto>
        {
            return (TCollection)_factory.CreateCollection(dataItems);            
        }
        

        /// <summary>
        /// Constructs a new business entity of the correct type
        /// </summary>
        /// <returns></returns>
        public TBase ConstructNewBusinessEntity()
        {
            return _factory.CreateNew();
        }

        public virtual async Task SaveAsync(IEnumerable<TBase> objectsToSave)
        {
            foreach (var obj in objectsToSave)
            {
                await SaveAsync(obj);
            }
        }

        public virtual async Task SaveAsync(CollectionBase<TBase, TDto> collection)
        {            
            foreach (TBase t in collection)
            {
                await SaveAsync(t);
            }
        }

        public virtual Task SaveAsync(TBase objectToSave)
        {
            if (objectToSave == null)
                throw new ArgumentNullException("objectToSave");

            if (!objectToSave.IsValid)
                throw new Validation.ValidationException("Validatio failed", objectToSave.ValidationMessages);

            return Table.SaveAsync(objectToSave.GetDataObject());
        }

        public virtual Task DeleteAsync(TBase objectToDelete)
        {
            return Table.DeleteAsync(objectToDelete.GetDataObject());
        }
        

        public virtual async Task DeleteAsync(IEnumerable<TBase> objectsToDelete)
        {
            foreach (var obj in objectsToDelete)
            {
                await DeleteAsync(obj);
            }
        }

    }
}
