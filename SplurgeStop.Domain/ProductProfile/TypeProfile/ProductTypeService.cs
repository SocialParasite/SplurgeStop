using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.TypeProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public sealed class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService(IProductTypeRepository repository,
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
                DeleteProductType cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveProductTypeAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newProductType = ProductType.Create(cmd.Id, cmd.Name);

            await _repository.AddProductTypeAsync(newProductType);

            if (newProductType.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid productTypeId, Func<ProductType, Task> operation)
        {
            var productType = await _repository.LoadProductTypeAsync(productTypeId);

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
            var productType = await _repository.LoadProductTypeAsync(productTypeId);

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
            return await _repository.GetAllProductTypeDtoAsync();
        }

        public async Task<ProductType> GetProductTypeAsync(ProductTypeId id)
        {
            return await _repository.GetProductTypeAsync(id);
        }
    }
}
