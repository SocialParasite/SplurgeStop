using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.TypeProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public sealed class ProductTypeService : IProductTypeService
    {
        private readonly IRepository<ProductType, ProductTypeDto, ProductTypeId> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService(IRepository<ProductType, ProductTypeDto, ProductTypeId> repository,
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
                SetProductTypeName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateProductTypeName(cmd.Name)),
                DeleteProductType cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newProductType = ProductType.Create(cmd.Id, cmd.Name);

            await _repository.AddAsync(newProductType);

            if (newProductType.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid productTypeId, Func<ProductType, Task> operation)
        {
            var productType = await _repository.LoadAsync(productTypeId);

            if (productType == null)
                throw new InvalidOperationException($"Entity with id {productTypeId} cannot be found");

            await operation(productType);

            if (productType.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid productTypeId, Action<ProductType> operation)
        {
            var productType = await _repository.LoadAsync(productTypeId);

            if (productType == null)
                throw new InvalidOperationException($"Entity with id {productTypeId} cannot be found");

            operation(productType);

            if (productType.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypeDtoAsync()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<ProductType> GetProductTypeAsync(ProductTypeId id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
