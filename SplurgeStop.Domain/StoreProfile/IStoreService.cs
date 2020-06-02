using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile.DTO;

namespace SplurgeStop.Domain.StoreProfile
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreStripped>> GetAllStoresStripped();
        Task<Store> GetDetailedStore(StoreId id);
        Task Handle(object command);
    }
}
