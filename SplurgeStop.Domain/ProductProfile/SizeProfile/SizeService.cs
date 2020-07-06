using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.SizeProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public sealed class SizeService : ISizeService
    {
        private readonly ISizeRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public SizeService(ISizeRepository repository,
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
                Commands.SetSizeAmount cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateSizeAmount(cmd.Amount)),
                Commands.DeleteSize cmd => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveSizeAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newSize = Size.Create(cmd.Id, cmd.Amount);

            await repository.AddSizeAsync(newSize);

            if (newSize.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid brandId, Func<Size, Task> operation)
        {
            var size = await repository.LoadSizeAsync(brandId);

            if (size == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            await operation(size);

            if (size.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid sizeId, Action<Size> operation)
        {
            var size = await repository.LoadSizeAsync(sizeId);

            if (size == null)
                throw new InvalidOperationException($"Entity with id {sizeId} cannot be found");

            operation(size);

            if (size.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync()
        {
            return await repository.GetAllSizeDtoAsync();
        }

        public async Task<Size> GetSizeAsync(SizeId id)
        {
            return await repository.GetSizeAsync(id);
        }
    }
}
