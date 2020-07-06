using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using static SplurgeStop.Domain.StoreProfile.Commands;

namespace SplurgeStop.Domain.StoreProfile
{
    public sealed class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public StoreService(IStoreRepository repository,
                            IUnitOfWork unitOfWork)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                UpdateStore cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateStore(cmd.LocationId, c),
                    c => c.UpdateStoreName(cmd.Name)),
                ChangeLocation cmd
                    => HandleUpdateAsync(cmd.Id, async c => await ChangeLocationAsync(c, cmd.Location.Id)),
                DeleteStore cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveStoreAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task UpdateStore(LocationId cmd, Store store)
        {
            await ChangeLocationAsync(store, cmd);
        }


        private async Task ChangeLocationAsync(Store store, LocationId locationId)
        {
            await _repository.ChangeLocation(store, locationId);
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            Location location = null;

            if (cmd.LocationId != null)
            {
                location = await _repository.GetLocationAsync(cmd.LocationId);
            }

            var newStore = Store.Create(cmd.Id, cmd.Name, location);

            await _repository.AddStoreAsync(newStore);

            if (newStore.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid storeId, Func<Store, Task> operation, Action<Store> op2 = null)
        {
            var store = await _repository.LoadStoreAsync(storeId);

            if (store == null)
                throw new InvalidOperationException($"Entity with id {storeId} cannot be found");

            await operation(store);
            op2?.Invoke(store);

            if (store.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        //private async Task HandleUpdate(Guid storeId, Action<Store> operation)
        //{
        //    var store = await _repository.LoadStoreAsync(storeId);

        //    if (store == null)
        //        throw new InvalidOperationException($"Entity with id {storeId} cannot be found");

        //    operation(store);

        //    if (store.EnsureValidState())
        //    {
        //        await _unitOfWork.Commit();
        //    }
        //}

        public async Task<IEnumerable<StoreStripped>> GetAllStoresStripped()
        {
            return await _repository.GetAllStoresStrippedAsync();
        }

        public async Task<Store> GetDetailedStore(StoreId id)
        {
            return await _repository.GetStoreFullAsync(id);
        }
    }
}
