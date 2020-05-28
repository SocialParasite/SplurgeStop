using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IStoreRepository
    {
        Task AddStoreAsync(Store store);
        Task<bool> ExistsAsync(StoreId id);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreFullAsync(StoreId id);
        Task<Store> LoadFullStoreAsync(StoreId id);
        Task<Store> LoadStoreAsync(StoreId id);
    }
}