using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.BrandProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public sealed class BrandService : IBrandService
    {
        private readonly IRepository<Brand, BrandDto, BrandId> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IRepository<Brand, BrandDto, BrandId> repository,
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
                SetBrandName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateBrandName(cmd.Name)),
                DeleteBrand cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newBrand = Brand.Create(cmd.Id, cmd.Name);

            await _repository.AddAsync(newBrand);

            if (newBrand.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid brandId, Func<Brand, Task> operation)
        {
            var brand = await _repository.LoadAsync(brandId);

            if (brand == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            await operation(brand);

            if (brand.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid brandId, Action<Brand> operation)
        {
            var brand = await _repository.LoadAsync(brandId);

            if (brand == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            operation(brand);

            if (brand.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandDtoAsync()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<Brand> GetBrandAsync(BrandId id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
