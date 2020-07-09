using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuidHelpers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.UI.WebApi.Common;
using static SplurgeStop.Domain.ProductProfile.SizeProfile.Events;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _service;

        public SizeController(ISizeService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<SizeDto>> GetSizes()
        {
            return await _service.GetAllSizeDtoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Size> GetSize(Guid id)
        {
            return await _service.GetSizeAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<SizeCreated>> Post(Commands.Create request)
        {
            try
            {
                request.Id = new SizeId(SequentialGuid.NewSequentialGuid());

                await RequestHandler.HandleCommand(request, _service.Handle);

                // HACK: Future me, do something clever instead...
                if (!string.IsNullOrEmpty(request.Amount))
                {
                    if (request.Id != null) return new SizeCreated { Id = (Guid)request.Id, Amount = request.Amount };
                }
                return new BadRequestObjectResult(
                    new
                    {
                        error = "Size was not in valid state! Amount should not be empty."
                    }
                );
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new size =-O");

                throw;
            }
        }

        [Route("SizeInfo")]
        [HttpPut]
        public async Task<IActionResult> Put(Commands.SetSizeAmount request)
            => await RequestHandler.HandleCommand(request, _service.Handle);

        [Route("Delete")]
        [HttpPost]
        public async Task<ActionResult<SizeDeleted>> DeleteSize(Commands.DeleteSize request)
        {
            var result = await RequestHandler.HandleCommand(request, _service.Handle);

            if (result.GetType() == typeof(OkResult))
                return new SizeDeleted { Id = request.Id };

            else
                return new BadRequestObjectResult(new { error = "Error occurred during delete attempt." });

        }
    }
}
