using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.ProductProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await service.GetAllProductDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProduct(Guid id)
        {
            return await service.GetProductAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<ProductCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new ProductId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new ProductCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Product was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new product =-O");

                throw;
            }
        }

        [Route("ProductInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.UpdateProduct request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("productBrand")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.ChangeBrand request)
            => RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<ProductDeleted>> DeleteProduct(Commands.DeleteProduct request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new ProductDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
