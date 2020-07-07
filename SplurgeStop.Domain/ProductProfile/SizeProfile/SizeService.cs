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
        private readonly IRepository<Size, SizeDto, SizeId> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SizeService(IRepository<Size, SizeDto, SizeId> repository,
                           IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                SetSizeAmount cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateSizeAmount(cmd.Amount)),
                DeleteSize cmd => HandleUpdateAsync(cmd.Id, _ => _repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newSize = Size.Create(cmd.Id, cmd.Amount);

            await _repository.AddAsync(newSize);

            if (newSize.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid brandId, Func<Size, Task> operation)
        {
            var size = await _repository.LoadAsync(brandId);

            if (size == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            await operation(size);

            if (size.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid sizeId, Action<Size> operation)
        {
            var size = await _repository.LoadAsync(sizeId);

            if (size == null)
                throw new InvalidOperationException($"Entity with id {sizeId} cannot be found");

            operation(size);

            if (size.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<Size> GetSizeAsync(SizeId id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
