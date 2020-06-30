using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.ProductProfile.BrandProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService service;

        public BrandController(IBrandService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<BrandDto>> GetBrands()
        {
            return await service.GetAllBrandDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Brand> GetBrand(Guid id)
        {
            return await service.GetBrandAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<BrandCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new BrandId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new BrandCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Brand was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new brand =-O");

                throw;
            }
        }

        [Route("BrandInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetBrandName request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<BrandDeleted>> DeleteBrand(Commands.DeleteBrand request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new BrandDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
