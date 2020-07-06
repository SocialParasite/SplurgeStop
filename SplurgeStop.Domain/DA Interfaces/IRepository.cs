using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IRepository<T, TDto, in TId>
    {
        // Queries
        Task<T> LoadAsync(TId id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<TDto>> GetAllDtoAsync();
        Task<T> GetAsync(TId id);
        Task<bool> ExistsAsync(TId id);

        // Commands
        Task AddAsync(T size);
        Task RemoveAsync(TId id);
    }
}
