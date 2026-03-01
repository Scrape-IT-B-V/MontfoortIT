using System.Threading.Tasks;

namespace MontfoortIT.Business
{
    public interface IRepository<TBase, TDto>
    {
        Task SaveAsync(TBase entity);

        Task DeleteAsync(TBase entity);
        TBase ConstructBusinessEntity(TDto entity);
    }
}
