using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Exceptions;
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
        public async Task<IEnumerable<Store>> GetAllStores()
        {
            return await repository.GetAllStoresAsync();
        }

        public async Task<Store> GetDetailedStore(StoreId id)
        {
            return await repository.GetStoreFullAsync(id);
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                SetStoreName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateStoreName(cmd.Name)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newStore = Store.Create(cmd.Id, cmd.Name);

            await repository.AddStoreAsync(newStore);
            
            if (newStore.EnsureValidState())
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
    }
}
