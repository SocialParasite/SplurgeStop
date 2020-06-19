using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.CountryProfile.DTO;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.CountryProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService service;

        public CountryController(ICountryService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<CountryDto>> GetCountries()
        {
            return await service.GetAllCountryDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Country> GetCountry(Guid id)
        {
            return await service.GetCountryAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<CountryCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new CountryId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Name))
                {
                    return new CountryCreated { Id = (Guid)request.Id, Name = request.Name };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Country was not in valid state! Name should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new country =-O");

                throw;
            }
        }

        [Route("CountryInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetCountryName request)
            => await RequestHandler.HandleCommand(request, service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<CountryDeleted>> DeleteCountry(Commands.DeleteCountry request)
        {
            var result = await RequestHandler.HandleCommand(request, service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new CountryDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
