using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.BrandProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public sealed class BrandService : IBrandService
    {
        private readonly IBrandRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public BrandService(IBrandRepository repository,
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
                Commands.SetBrandName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateBrandName(cmd.Name)),
                Commands.DeleteBrand cmd => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveBrandAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newBrand = Brand.Create(cmd.Id, cmd.Name);

            await repository.AddBrandAsync(newBrand);

            if (newBrand.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid brandId, Func<Brand, Task> operation)
        {
            var brand = await repository.LoadBrandAsync(brandId);

            if (brand == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            await operation(brand);

            if (brand.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid brandId, Action<Brand> operation)
        {
            var brand = await repository.LoadBrandAsync(brandId);

            if (brand == null)
                throw new InvalidOperationException($"Entity with id {brandId} cannot be found");

            operation(brand);

            if (brand.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandDtoAsync()
        {
            return await repository.GetAllBrandDtoAsync();
        }

        public async Task<Brand> GetBrandAsync(BrandId id)
        {
            return await repository.GetBrandAsync(id);
        }
    }
}
