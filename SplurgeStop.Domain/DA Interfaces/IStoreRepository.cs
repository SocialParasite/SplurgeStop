using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IStoreRepository : IRepository<Store, StoreStripped, StoreId>
    {
        Task<Store> GetTrackedAsync(StoreId id);
        Task<Location> GetLocationAsync(LocationId id);
        Task ChangeLocation(Store store, LocationId locationId);
    }
}