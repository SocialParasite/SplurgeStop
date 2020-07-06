using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository repository,
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
                UpdateProduct cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateProductName(cmd.Name)),
                ChangeBrand cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateBrandAsync(c, cmd.BrandId)),
                ChangeProductType cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateProductTypeAsync(c, cmd.ProductTypeId)),
                ChangeSize cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateSizeAsync(c, cmd.SizeId)),
                DeleteProduct cmd
                    => HandleUpdateAsync(cmd.Id, _ => _repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task UpdateSizeAsync(Product product, Guid sizeId)
        {
            await _repository.ChangeSize(product, sizeId);
        }

        private async Task UpdateProductTypeAsync(Product product, Guid productTypeId)
        {
            await _repository.ChangeProductType(product, productTypeId);
        }

        private async Task UpdateBrandAsync(Product product, BrandId brandId)
        {
            await _repository.ChangeBrand(product, brandId);
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var brand = await _repository.GetBrandAsync(cmd.BrandId);

            var newProduct = Product.Create(cmd.Id, cmd.Name, brand);

            await _repository.AddAsync(newProduct);

            if (newProduct.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid productId, Func<Product, Task> operation)
        {
            var product = await _repository.LoadAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"Entity with id {productId} cannot be found");

            await operation(product);

            if (product.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid productId, Action<Product> operation)
        {
            var product = await _repository.LoadAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"Entity with id {productId} cannot be found");

            operation(product);

            if (product.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductDtoAsync()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<Product> GetProductAsync(ProductId id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
