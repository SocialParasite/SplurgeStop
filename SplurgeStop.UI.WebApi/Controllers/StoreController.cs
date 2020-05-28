using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Common;

namespace SplurgeStop.UI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService service;

        public StoreController(IStoreService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<Store>> GetStores()
        {
            return await service.GetAllStores();
        }

        [HttpGet("{id}")]
        public async Task<Store> GetStore(Guid id)
        {
            return await service.GetDetailedStore(id);
        }

        [HttpPost]
        public Task<IActionResult> Post(Commands.Create request)
        {
            try
            {
                return RequestHandler.HandleCommand(request, service.Handle);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Somehow something happened while creating a new Store =-O");

                throw;
            }
        }

        [Route("storeName")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.SetStoreName request) 
            => RequestHandler.HandleCommand(request, service.Handle);
    }
}
