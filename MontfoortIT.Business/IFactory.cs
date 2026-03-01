using MontfoortIT.Business.DataInterfaces;
using System.Collections;
using System.Collections.Generic;

namespace MontfoortIT.Business
{
    public interface IFactory<TBase, TDto>
        where TDto : IDto
        where TBase : Base<TDto>
    {
        IRepository<TBase,TDto> Repository { get; set; }

        TBase CreateNew();

        TBase CreateExisting(TDto dto);

        CollectionBase<TBase, TDto> CreateCollection(IEnumerable<TDto> dataObjects);
    }
}