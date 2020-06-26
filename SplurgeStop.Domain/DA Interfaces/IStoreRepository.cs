using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.DTO;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IStoreRepository
    {
        Task AddStoreAsync(Store store);
        Task<bool> ExistsAsync(StoreId id);
        Task<Store> LoadStoreAsync(StoreId id);
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreFullAsync(StoreId id);
        Task<Store> LoadFullStoreAsync(StoreId id);

        Task<IEnumerable<StoreStripped>> GetAllStoresStrippedAsync();
        Task<Location> GetLocationAsync(LocationId id);


        Task RemoveStoreAsync(StoreId id);
        Task ChangeLocation(Store store, LocationId locationId);
    }
}