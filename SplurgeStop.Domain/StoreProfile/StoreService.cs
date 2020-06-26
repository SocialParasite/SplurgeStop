using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.StoreProfile.DTO;
using static SplurgeStop.Domain.StoreProfile.Commands;

namespace SplurgeStop.Domain.StoreProfile
{
    public sealed class StoreService : IStoreService
    {
        private readonly IStoreRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public StoreService(IStoreRepository repository,
                            IUnitOfWork unitOfWork)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                SetStoreName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateStoreName(cmd.Name)),
                ChangeLocation cmd
                    => HandleUpdateAsync(cmd.Id, async c => await ChangeLocationAsync(c, cmd.Location.Id)),
                DeleteStore cmd => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveStoreAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task ChangeLocationAsync(Store store, LocationId locationId)
        {
            await repository.ChangeLocation(store, locationId);
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            Location location = null;

            if (cmd.LocationId != null)
            {
                location = await repository.GetLocationAsync(cmd.LocationId);
            }

            var newStore = Store.Create(cmd.Id, cmd.Name, location);

            await repository.AddStoreAsync(newStore);

            if (newStore.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid storeId, Func<Store, Task> operation)
        {
            var store = await repository.LoadStoreAsync(storeId);

            if (store == null)
                throw new InvalidOperationException($"Entity with id {storeId} cannot be found");

            await operation(store);

            if (store.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid storeId, Action<Store> operation)
        {
            var store = await repository.LoadStoreAsync(storeId);

            if (store == null)
                throw new InvalidOperationException($"Entity with id {storeId} cannot be found");

            operation(store);

            if (store.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<StoreStripped>> GetAllStoresStripped()
        {
            return await repository.GetAllStoresStrippedAsync();
        }

        public async Task<Store> GetDetailedStore(StoreId id)
        {
            return await repository.GetStoreFullAsync(id);
        }
    }
}
