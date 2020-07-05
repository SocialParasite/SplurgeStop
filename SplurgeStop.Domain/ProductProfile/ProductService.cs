using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.ProductProfile.Commands;

namespace SplurgeStop.Domain.ProductProfile
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IProductRepository repository,
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
                ProductProfile.Commands.UpdateProduct cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateProductName(cmd.Name)),
                ProductProfile.Commands.ChangeBrand cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateBrandAsync(c, cmd.BrandId)),
                ProductProfile.Commands.ChangeProductType cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateProductTypeAsync(c, cmd.ProductTypeId)),
                ProductProfile.Commands.ChangeSize cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateSizeAsync(c, cmd.SizeId)),
                ProductProfile.Commands.DeleteProduct cmd
                    => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveProductAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task UpdateSizeAsync(Product product, Guid sizeId)
        {
            await repository.ChangeSize(product, sizeId);
        }

        private async Task UpdateProductTypeAsync(Product product, Guid productTypeId)
        {
            await repository.ChangeProductType(product, productTypeId);
        }

        private async Task UpdateBrandAsync(Product product, BrandId brandId)
        {
            await repository.ChangeBrand(product, brandId);
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var brand = await repository.GetBrandAsync(cmd.BrandId);

            var newProduct = Product.Create(cmd.Id, cmd.Name, brand);

            await repository.AddProductAsync(newProduct);

            if (newProduct.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid productId, Func<Product, Task> operation)
        {
            var product = await repository.LoadProductAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"Entity with id {productId} cannot be found");

            await operation(product);

            if (product.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid productId, Action<Product> operation)
        {
            var product = await repository.LoadProductAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"Entity with id {productId} cannot be found");

            operation(product);

            if (product.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductDtoAsync()
        {
            return await repository.GetAllProductDtoAsync();
        }

        public async Task<Product> GetProductAsync(ProductId id)
        {
            return await repository.GetProductAsync(id);
        }
    }
}
