using MontfoortIT.Business.DataInterfaces;
using MontfoortIT.Business.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MontfoortIT.Business
{
    public class Factory<TBase, TCollection, TDto> : IFactory<TBase, TDto>
        where TBase : Base<TDto> where TDto : IDto where TCollection : CollectionBase<TBase, TDto>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISecurityConnection _securityConnection;
        private ConstructorInfo _tBaseConstructor;
        private ConstructorInfo _tCollectionConstructor;

        public IRepository<TBase, TDto> Repository { get; set; }

        public Factory(IServiceProvider serviceProvider, ISecurityConnection securityConnection)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;
            _securityConnection = securityConnection;

            var baseType = typeof(TBase);
            var constructor = baseType.GetConstructor([typeof(ISecurityConnection), typeof(TDto)]);
            if (constructor == null)
                throw new NotImplementedException($"Constructor not found in type {baseType.FullName} with ISecurityConnection and TDto");
            _tBaseConstructor = constructor;

            var collType = typeof(TCollection);
            var constructorCol = collType.GetConstructor([typeof(IRepository<TBase, TDto>), typeof(IList<TDto>)]);
            if (constructorCol == null)
                throw new NotImplementedException($"Constructor not found in collection type {collType.FullName} with repository and IList<TDto>");
            _tCollectionConstructor = constructorCol;
        }


        public CollectionBase<TBase, TDto> CreateCollection(IEnumerable<TDto> dataObjects)
        {
            return (TCollection)_tCollectionConstructor.Invoke([Repository, dataObjects]);
        }

        public virtual TBase CreateExisting(TDto dto)
        {
            return (TBase)_tBaseConstructor.Invoke([_securityConnection, dto]);
        }

        public virtual TBase CreateNew()
        {
            return CreateNew<TBase>();
        }

        protected T CreateNew<T>()
            where T : notnull
        {
            object? ser = _serviceProvider.GetService(typeof(T));
            if(ser == null)
                throw new NotImplementedException($"Service not found for type {typeof(T).FullName}");

            return (T)ser;
        }

    }
}
