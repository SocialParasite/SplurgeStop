using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.StoreProfile.Location.CityProfile;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.CityProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService service;

        public CityController(ICityService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<CityDto>> GetCities()
        {
            return await service.GetAllCityDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<City> GetCity(Guid id)
        {
            return await service.GetCityAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<CityCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new CityId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new CityCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "City was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new city =-O");

                throw;
            }
        }

        [Route("CityInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetCityName request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<CityDeleted>> DeleteCity(Commands.DeleteCity request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new CityDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
