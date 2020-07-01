using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.ProductProfile.TypeProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService service;

        public ProductTypeController(IProductTypeService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<ProductTypeDto>> GetProductTypes()
        {
            return await service.GetAllProductTypeDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<ProductType> GetProductType(Guid id)
        {
            return await service.GetProductTypeAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<ProductTypeCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new ProductTypeId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new ProductTypeCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "ProductType was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new product type =-O");

                throw;
            }
        }

        [Route("ProductTypeInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetProductTypeName request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<ProductTypeDeleted>> DeleteProductType(Commands.DeleteProductType request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new ProductTypeDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
