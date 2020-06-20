using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.LocationProfile.DTO;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.LocationProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService service;

        public LocationController(ILocationService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<LocationDto>> GetLocations()
        {
            return await service.GetAllLocationDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Location> GetLocation(Guid id)
        {
            return await service.GetLocationAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<LocationCreated>> Post(Commands.Create request)
        {
            if (request.CityId == default || request.CountryId == default)
            {
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Location was not in valid state! Name should not be empty."
                    }
                );
            }

            try
            {
                request.Id = new LocationId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                return new LocationCreated { Id = (Guid)request.Id };
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new location =-O");

                throw;
            }
        }

        [Route("LocationInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.ChangeCountry request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("LocationInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.ChangeCity request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<LocationDeleted>> DeleteLocation(Commands.DeleteLocation request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new LocationDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
